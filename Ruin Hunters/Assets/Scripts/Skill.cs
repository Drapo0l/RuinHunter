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

    public void ActivateSkill(GameObject target, int attackerPower, float multiplier)
    {
        // Simple damage calculation (adjust as necessary)
        int damage = Mathf.FloorToInt(baseDamage * multiplier) + attackerPower;

        // Apply damage to the target, e.g., calling the target's TakeDamage() method.
        IDamage targetHit = target.GetComponent<IDamage>();
        if (targetHit != null) 
        {
            targetHit.TakeDamage(damage, elementType);
        }
    }

    public void ActivateWeaponAttack(GameObject target, int attackerPower, float multiplier)
    {
        // Simple damage calculation (adjust as necessary)
        int damage = Mathf.FloorToInt(baseDamage * multiplier) + attackerPower;

        // Apply damage to the target, e.g., calling the target's TakeDamage() method.
        IDamage targetHit = target.GetComponent<IDamage>();
        if (targetHit != null)
        {
            targetHit.TakeDamage(damage, PublicEnums.ElementType.None);
        }
    }
}
