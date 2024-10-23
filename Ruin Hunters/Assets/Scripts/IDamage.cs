using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage 
{
    void TakeSkillDamage1(int damage, PublicEnums.ElementType elementType);
    void TakeMeleeDamage1(int damage, PublicEnums.WeaponType weaponType);   
}
