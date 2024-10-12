using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "NewCharacterAttributes", menuName = "Character/Attributes")]
public class CharacterAttributes : ScriptableObject
{
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public string nameOfCharacter;
    public int health;
    public int maxHealth;
    public int maxMana;
    public int mana;
    public int combatSpeed;
    public int skillDamage;
    public int attackDamage =0;
    public int critChance;
    public int effectChance;
    public int Defence;
    public int maxHealthOG;
    public int maxManaOG;
    public int combatSpeedOG;
    public int skillDamageOG;
    public int attackDamageOG =0;
    public int critChanceOG;
    public int effectChanceOG;
    public int DefenceOG;
    public int expGive;
    public List<Skill> skills; // List of skills specific to the character    
    public PublicEnums.Regions regions;
    public bool isTurn = false;
    public bool isStuned = false;
    internal int special_count;

    public void AddExperience (int xpAmount)
    {
        currentXP += xpAmount;
        CheckLevelUP();
    }

    private void CheckLevelUP()
    {
        if(currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel += 100;
        maxHealth += 10;
        health = maxHealth;
        maxMana += 5;
        mana = maxMana;
        attackDamage += 5;
        Defence += 5;
        combatSpeed += 5;
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

