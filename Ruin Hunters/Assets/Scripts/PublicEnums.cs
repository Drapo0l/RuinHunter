using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEnums : MonoBehaviour
{
    
    public enum WeaponType
    {
        None,
        Sword,
        Dagger,
        Bow,
        Lance,
        
    }

    public enum ElementType
    {
        None,
        Fire,
        Ice,
        Lightning,
        Earth,
        
    }

    public enum Regions
    {
        None,
        desert,
        plains,
        caves,
    }

    public enum Effects // these are all of the effects attacks and skills can do
    {
        None,
        Crit,
        Heal,
        Stun,
        AttackUp,
        AttackDown,
        DefenceUp,
        DefenceDown,
        SpeedUp,
        SpeedDown,
        SkillPDown,
        SkillPUP,
    }

    public enum EnemyTypes // these are the enemy types 
    {
        Normal,
        Agressive,
        CasterA,
        CasterS,

    }
}
