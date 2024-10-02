using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage 
{
    void TakeSkillDamage(int damage, PublicEnums.ElementType elementType);

    float GetDamageMultiplier(PublicEnums.ElementType elementType);
}
