using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    public void ActivateSkill(GameObject target, int attackerPower, float multiplier,int crit, int effectC)
    {
        // Simple damage calculation (adjust as necessary)
        int damage = Mathf.FloorToInt(baseDamage * multiplier) + attackerPower;
        damage = Seffect(target, crit, effectC, damage, effect);
        if (effect != PublicEnums.Effects.Heal)
        {
            damage = damage - target.GetComponent<playerController>().playerStats.Defence;
            if(damage < 0)
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
        damage = damage - target.GetComponent<playerController>().playerStats.Defence;
        damage = damage - target.GetComponent<playerController>().playerStats.Defence;
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
         int chance = Random.Range(1, 100);
            if(chance <= C)
            {
                D = D * 2;
            }
        }
        if (EN == PublicEnums.Effects.Stun)
        {
            T.GetComponent<playerController>().playerStats.isStuned = true;
        }
        if (EN == PublicEnums.Effects.Heal)
        {
            D = D * -1;
        }
        if (EN == PublicEnums.Effects.AttackDown)
        {
            T.GetComponent<playerController>().playerStats.attackDamage = T.GetComponent<playerController>().playerStats.attackDamage / 2;
        }
        if (EN == PublicEnums.Effects.AttackUp)
        {
            T.GetComponent<playerController>().playerStats.attackDamage = T.GetComponent<playerController>().playerStats.attackDamage * 2;
        }
        if (EN == PublicEnums.Effects.DefenceDown)
        {
            T.GetComponent<playerController>().playerStats.Defence = T.GetComponent<playerController>().playerStats.Defence / 2;
        }
        if (EN == PublicEnums.Effects.DefenceUp)
        {
            T.GetComponent<playerController>().playerStats.Defence = T.GetComponent<playerController>().playerStats.Defence * 2;
        }
       
        if (EN == PublicEnums.Effects.SpeedDown)
        {
            T.GetComponent<playerController>().playerStats.combatSpeed = T.GetComponent<playerController>().playerStats.combatSpeed / 2;
        }
        if (EN == PublicEnums.Effects.SpeedUp)
        {
            T.GetComponent<playerController>().playerStats.combatSpeed = T.GetComponent<playerController>().playerStats.combatSpeed * 2;
        }
        if (EN == PublicEnums.Effects.SkillPDown)
        {
            T.GetComponent<playerController>().playerStats.skillDamage = T.GetComponent<playerController>().playerStats.skillDamage / 2;
        }
        if (EN == PublicEnums.Effects.SkillPUP)
        {
            T.GetComponent<playerController>().playerStats.skillDamage = T.GetComponent<playerController>().playerStats.skillDamage * 2;
        }
        if (EN == PublicEnums.Effects.Clense)
        {
            T.GetComponent<playerController>().playerStats.maxMana = T.GetComponent<playerController>().playerStats.maxManaOG;
            T.GetComponent<playerController>().playerStats.maxHealth = T.GetComponent<playerController>().playerStats.maxHealthOG;
            T.GetComponent<playerController>().playerStats.Defence =  T.GetComponent<playerController>().playerStats.DefenceOG;
            T.GetComponent<playerController>().playerStats.combatSpeed = T.GetComponent<playerController>().playerStats.combatSpeedOG;
             T.GetComponent<playerController>().playerStats.skillDamage = T.GetComponent<playerController>().playerStats.skillDamageOG;
              T.GetComponent<playerController>().playerStats.attackDamage = T.GetComponent<playerController>().playerStats.attackDamageOG;
             T.GetComponent<playerController>().playerStats.critChance = T.GetComponent<playerController>().playerStats.critChanceOG;
              T.GetComponent<playerController>().playerStats.effectChance = T.GetComponent<playerController>().playerStats.effectChanceOG;
          
        }
        return D;
    }
}
