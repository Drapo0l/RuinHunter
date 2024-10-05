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
        damage = damage - target.GetComponent<CharacterAttributes>().Defence;
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
        damage = damage - target.GetComponent<CharacterAttributes>().Defence;
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
            T.GetComponent<CharacterAttributes>().isStuned = true;
        }
        if (EN == PublicEnums.Effects.Heal)
        {
            D = D * -1;
        }
        if (EN == PublicEnums.Effects.AttackDown)
        {
            T.GetComponent<CharacterAttributes>().attackDamage = T.GetComponent<CharacterAttributes>().attackDamage / 2;
        }
        if (EN == PublicEnums.Effects.AttackUp)
        {
            T.GetComponent<CharacterAttributes>().attackDamage = T.GetComponent<CharacterAttributes>().attackDamage * 2;
        }
        if (EN == PublicEnums.Effects.DefenceDown)
        {
            T.GetComponent<CharacterAttributes>().Defence = T.GetComponent<CharacterAttributes>().Defence / 2;
        }
        if (EN == PublicEnums.Effects.DefenceUp)
        {
            T.GetComponent<CharacterAttributes>().Defence = T.GetComponent<CharacterAttributes>().Defence * 2;
        }
       
        if (EN == PublicEnums.Effects.SpeedDown)
        {
            T.GetComponent<CharacterAttributes>().combatSpeed = T.GetComponent<CharacterAttributes>().combatSpeed / 2;
        }
        if (EN == PublicEnums.Effects.SpeedUp)
        {
            T.GetComponent<CharacterAttributes>().combatSpeed = T.GetComponent<CharacterAttributes>().combatSpeed * 2;
        }
        if (EN == PublicEnums.Effects.SkillPDown)
        {
            T.GetComponent<CharacterAttributes>().skillDamage = T.GetComponent<CharacterAttributes>().skillDamage / 2;
        }
        if (EN == PublicEnums.Effects.SkillPUP)
        {
            T.GetComponent<CharacterAttributes>().skillDamage = T.GetComponent<CharacterAttributes>().skillDamage * 2;
        }
        if (EN == PublicEnums.Effects.Clense)
        {
            T.GetComponent<CharacterAttributes>().maxManaOG = T.GetComponent<CharacterAttributes>().maxMana;
            T.GetComponent<CharacterAttributes>().maxHealthOG =  T.GetComponent<CharacterAttributes>().maxHealth;
            T.GetComponent<CharacterAttributes>().DefenceOG = T.GetComponent<CharacterAttributes>().Defence;
            T.GetComponent<CharacterAttributes>().combatSpeedOG = T.GetComponent<CharacterAttributes>().combatSpeed;
            T.GetComponent<CharacterAttributes>().skillDamageOG = T.GetComponent<CharacterAttributes>().skillDamage;
            T.GetComponent<CharacterAttributes>().attackDamageOG = T.GetComponent<CharacterAttributes>().attackDamage;
            T.GetComponent<CharacterAttributes>().critChanceOG = T.GetComponent<CharacterAttributes>().critChance;
            T.GetComponent<CharacterAttributes>().effectChanceOG = T.GetComponent<CharacterAttributes>().effectChance;
          
        }
        return D;
    }
}
