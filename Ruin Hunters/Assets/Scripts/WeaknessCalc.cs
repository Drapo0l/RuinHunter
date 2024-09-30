using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponCalc
{
    public PublicEnums.WeaponType weaponType;
    public float weaponMultiplier = 1f;
}

[System.Serializable]
public class ElementCalc
{
    public PublicEnums.ElementType elementType;
    public float elementMultiplier = 1f;
}
