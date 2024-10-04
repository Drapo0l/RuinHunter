using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "NewCharacterAttributes", menuName = "Character/Attributes")]
public class CharacterAttributes : ScriptableObject
{
    public int health;
    public int maxHealth;
    public int maxMana;
    public int mana;
    public string nameOfCharacter;
    public int combatSpeed;
    public int skillDamage;
    public int attackDamage;
    public int critChance;
    public int effectChance;
    public int Defence;
    public List<Skill> skills; // List of skills specific to the character    
    public PublicEnums.Regions regions;
    public bool isTurn = false;
    public bool isStuned = false;
   

}

public class CharacterComponent : MonoBehaviour
{
    public CharacterAttributes stats;
    public CharacterComponent(CharacterAttributes _stats)
    {
        stats = _stats;
    }
}




[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class AddSkill : ScriptableObject
{
    public string skillName;
    public PublicEnums.ElementType elementType;
    public int baseDamage;
    public int manaCost;
}

