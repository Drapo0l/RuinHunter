using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill 
{
    public string skillName;
    public PublicEnums.ElementType elementType;
    public PublicEnums.WeaponType weaponType;
    public int baseDamage;
    public int manaCost;
    public PublicEnums.Effects effect;
    public int Ptargit; // set as 1 for enemys set as 0 for party members
    public bool AOE; // is it an aoe


    public void ActivateSkill(GameObject target, int attackerPower, float multiplier,int crit, int effectC)
    {
        // Simple damage calculation (adjust as necessary)
        int damage = Mathf.FloorToInt(baseDamage * multiplier) + attackerPower;
        damage = Seffect(target, crit, effectC, damage, effect);
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
            {
                damage = 0;
            }
        }
       
        // Apply damage to the target, e.g., calling the target's TakeDamage() method.
        IDamage targetHit = target.GetComponent<IDamage>();
        if (targetHit != null) 
        {
            targetHit.TakeSkillDamage(damage, elementType);
        }
    }

    public void ActivateWeaponAttack(GameObject target, int attackerPower, float multiplier,int crit, int effectC)
    {
        // Simple damage calculation (adjust as necessary)
        int damage = Mathf.FloorToInt(baseDamage * multiplier) + attackerPower;
        damage = Seffect(target, crit, effectC, damage, effect);
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
            targetHit.TakeMeleeDamage(damage, PublicEnums.WeaponType.None);
        }
    }

    int Seffect(GameObject T,int C, int E, int D, PublicEnums.Effects EN)
    {
        
        if (EN == PublicEnums.Effects.Crit)
        {
         int chance = Random.Range(1, 101);
            if(chance <= C)
            {
                D = D * 2;
            }
        }
        int chance2 = Random.Range(1, 101);
        if(chance2 >= E)
        {
            if (EN == PublicEnums.Effects.Stun)
            {
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
                D = D * -1;
            }
            if (EN == PublicEnums.Effects.AttackDown)
            {
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
                if (T.tag.Equals("Player"))
                {
                    T.GetComponent<playerController>().playerStats.maxMana = T.GetComponent<playerController>().playerStats.maxManaOG;
                    T.GetComponent<playerController>().playerStats.maxHealth = T.GetComponent<playerController>().playerStats.maxHealthOG;
                    T.GetComponent<playerController>().playerStats.Defence = T.GetComponent<playerController>().playerStats.DefenceOG;
                    T.GetComponent<playerController>().playerStats.combatSpeed = T.GetComponent<playerController>().playerStats.combatSpeedOG;
                    T.GetComponent<playerController>().playerStats.skillDamage = T.GetComponent<playerController>().playerStats.skillDamageOG;
                    T.GetComponent<playerController>().playerStats.attackDamage = T.GetComponent<playerController>().playerStats.attackDamageOG;
                    T.GetComponent<playerController>().playerStats.critChance = T.GetComponent<playerController>().playerStats.critChanceOG;
                    T.GetComponent<playerController>().playerStats.effectChance = T.GetComponent<playerController>().playerStats.effectChanceOG;
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
                    T.GetComponent<EnemyAI>().enemyStats.effectChance = T.GetComponent<EnemyAI>().enemyStats.effectChanceOG;
                }



            }
        }
        
        return D;
    }
}
