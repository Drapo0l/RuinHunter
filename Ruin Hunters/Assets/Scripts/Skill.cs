using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.Experimental.GraphView;


[System.Serializable]
public class Skill 
{
    public Sprite sprite;
    public string description;
    public string skillName;
    public PublicEnums.ElementType elementType;
    public PublicEnums.WeaponType weaponType;
    public int baseDamage;
    public int manaCost;
    public PublicEnums.Effects effect;
    public int Ptargit; // set as 1 for enemys set as 0 for party members
    public bool AOE; // is it an aoe
    private ParticleManager ParticleForSkill;
    public AudioSource caster;
    public AudioSource target;
    public AudioClip[] Activation_Sound;
    public float Activation_SoundV;
    public AudioClip[] Hit_Sound;
    public float Hit_SoundV;
    


    public void ActivateSkill(GameObject target, int attackerPower, float multiplier,int crit, PublicEnums.Effects effects)
    {
        caster.PlayOneShot(Activation_Sound[Activation_Sound.Length], Activation_SoundV);
        // Simple damage calculation (adjust as necessary)
        int damage = Mathf.FloorToInt(baseDamage * multiplier) + attackerPower;
        damage = Seffect(target, crit, damage, effects);
        if (effect != PublicEnums.Effects.Heal)
        {
            if (target.tag.Equals("Player"))
            {
                damage = damage - target.GetComponent<playerController>().playerStats.Defence;
            }
            else
            {
                damage = damage - target.GetComponent<EnemyAI>().enemyStats.Defence;
            }
        }

        if (damage < 0)
        {
            damage = 0;
        }

        // Apply damage to the target, e.g., calling the target's TakeDamage() method.
        IDamage targetHit = target.GetComponent<IDamage>();
        if (targetHit != null) 
        {
            caster.PlayOneShot(Hit_Sound[Hit_Sound.Length], Hit_SoundV);
          
            targetHit.TakeSkillDamage(damage, elementType);
        }
    }
   
    public void ActivateWeaponAttack(GameObject target, int attackerPower, float multiplier,int crit, PublicEnums.Effects effects, CharacterAttributes attacker)
    {
        attacker.attacker.PlayOneShot(attacker.Activation_Sound[attacker.Activation_Sound.Length], attacker.Activation_SoundV);
        // Simple damage calculation (adjust as necessary)
        int damage = Mathf.FloorToInt(baseDamage * multiplier) + attackerPower;
        damage = Seffect(target, crit, damage, effects);
        if (target.tag.Equals("Player"))
        {
            damage = damage - target.GetComponent<playerController>().playerStats.Defence;
        }
        else
        {
            damage = damage - target.GetComponent<EnemyAI>().enemyStats.Defence;
        }
       
        
        if (damage < 0)
        {
            damage = 0;
        }
        // Apply damage to the target, e.g., calling the target's TakeDamage() method.
        IDamage targetHit = target.GetComponent<IDamage>();
        if (targetHit != null)
        {
            attacker.target_BA.PlayOneShot(attacker.Hit_Sound[attacker.Hit_Sound.Length], attacker.Hit_SoundV);
            targetHit.TakeMeleeDamage(damage, PublicEnums.WeaponType.None);
        }
    }
    
    int Seffect(GameObject T,int C, int D, PublicEnums.Effects EN)
    {
        EnemyAI Pause = new EnemyAI();
        if (EN == PublicEnums.Effects.Crit)
        {
         int chance = Random.Range(1, 101);
            if(chance <= C)
            {
                DamageNumberManager.Instance.ShowString(T.transform.position, "CRIT", Color.yellow);
                T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[0]);
                D = D * 2;
            }
        }
        
            if (EN == PublicEnums.Effects.Stun)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "STUN!", Color.yellow);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[2]);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.isStuned = true;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.isStuned = true;
                }


            }
            if (EN == PublicEnums.Effects.Heal)
            {
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[1]);
            D = D * -1;
            }
            if (EN == PublicEnums.Effects.AttackDown)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "ATT DOWN", Color.black);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[4]);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.attackDamage = T.GetComponent<playerController>().playerStats.attackDamage / 2;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.attackDamage = T.GetComponent<EnemyAI>().enemyStats.attackDamage / 2;
                }

                T.GetComponent<EnemyAI>().enemyStats.attackDamage = T.GetComponent<EnemyAI>().enemyStats.attackDamage / 2;
            }
            if (EN == PublicEnums.Effects.AttackUp)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "ATT UP", Color.red);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[3]);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.attackDamage = T.GetComponent<playerController>().playerStats.attackDamage * 2;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.attackDamage = T.GetComponent<EnemyAI>().enemyStats.attackDamage * 2;
                }


            }
            if (EN == PublicEnums.Effects.DefenceDown)
            {
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[6]);
            DamageNumberManager.Instance.ShowString(T.transform.position, "DEF DOWN", Color.black);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.Defence = T.GetComponent<playerController>().playerStats.Defence / 2;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.Defence = T.GetComponent<EnemyAI>().enemyStats.Defence / 2;
                }


            }
            if (EN == PublicEnums.Effects.DefenceUp)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "DEF UP", Color.blue);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[5]);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.Defence = T.GetComponent<playerController>().playerStats.Defence * 2;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.Defence = T.GetComponent<EnemyAI>().enemyStats.Defence * 2;
                }


            }

            if (EN == PublicEnums.Effects.SpeedDown)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "SPD DOWN", Color.black);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[8]);
            //Pause.Epause();

            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.combatSpeed = T.GetComponent<playerController>().playerStats.combatSpeed / 2;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.combatSpeed = T.GetComponent<EnemyAI>().enemyStats.combatSpeed / 2;
                }


            }
            if (EN == PublicEnums.Effects.SpeedUp)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "SPD UP", Color.green);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[7]);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.combatSpeed = T.GetComponent<playerController>().playerStats.combatSpeed * 2;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.combatSpeed = T.GetComponent<EnemyAI>().enemyStats.combatSpeed * 2;
                }


            }
            if (EN == PublicEnums.Effects.SkillPDown)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "SKL DOWN", Color.black);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[10]);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.skillDamage = T.GetComponent<playerController>().playerStats.skillDamage / 2;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.skillDamage = T.GetComponent<EnemyAI>().enemyStats.skillDamage / 2;
                }


            }
            if (EN == PublicEnums.Effects.SkillPUP)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "SKL UP", Color.cyan);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[9]);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.skillDamage = T.GetComponent<playerController>().playerStats.skillDamage * 2;
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.skillDamage = T.GetComponent<EnemyAI>().enemyStats.skillDamage * 2;
                }


            }
            if (EN == PublicEnums.Effects.Clense)
            {
            DamageNumberManager.Instance.ShowString(T.transform.position, "Clense", Color.gray);
            T.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.Effect_Sounds[11]);
            //Pause.Epause();
            if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.maxMana = T.GetComponent<playerController>().playerStats.maxManaOG;
                    T.GetComponent<playerController>().playerStats.maxHealth = T.GetComponent<playerController>().playerStats.maxHealthOG;
                    T.GetComponent<playerController>().playerStats.Defence = T.GetComponent<playerController>().playerStats.DefenceOG;
                    T.GetComponent<playerController>().playerStats.combatSpeed = T.GetComponent<playerController>().playerStats.combatSpeedOG;
                    T.GetComponent<playerController>().playerStats.skillDamage = T.GetComponent<playerController>().playerStats.skillDamageOG;
                    T.GetComponent<playerController>().playerStats.attackDamage = T.GetComponent<playerController>().playerStats.attackDamageOG;
                    T.GetComponent<playerController>().playerStats.critChance = T.GetComponent<playerController>().playerStats.critChanceOG;
                    
                }
                else
                {
                    T.GetComponent<EnemyAI>().enemyStats.maxMana = T.GetComponent<EnemyAI>().enemyStats.maxManaOG;
                    T.GetComponent<EnemyAI>().enemyStats.maxHealth = T.GetComponent<EnemyAI>().enemyStats.maxHealthOG;
                    T.GetComponent<EnemyAI>().enemyStats.Defence = T.GetComponent<EnemyAI>().enemyStats.DefenceOG;
                    T.GetComponent<EnemyAI>().enemyStats.combatSpeed = T.GetComponent<EnemyAI>().enemyStats.combatSpeedOG;
                    T.GetComponent<EnemyAI>().enemyStats.skillDamage = T.GetComponent<EnemyAI>().enemyStats.skillDamageOG;
                    T.GetComponent<EnemyAI>().enemyStats.attackDamage = T.GetComponent<EnemyAI>().enemyStats.attackDamageOG;
                    T.GetComponent<EnemyAI>().enemyStats.critChance = T.GetComponent<EnemyAI>().enemyStats.critChanceOG;
                   
                }



            }
        
        
        return D;
    }
}
