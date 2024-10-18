using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
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
    public int attackDamage;
    public int critChance;
    public int effectChance;
    public int Defence;
    public int maxHealthOG;
    public int maxManaOG;
    public int combatSpeedOG;
    public int skillDamageOG;
    public int attackDamageOG;
    public int critChanceOG;
    public int effectChanceOG;
    public int DefenceOG;
    public int expGive;
    public List<Skill> skills; // List of skills specific to the character    
    public PublicEnums.Regions regions;
    public bool isTurn = false;
    public bool isStuned = false;
    public bool special = false;
    public int special_count;
    public WeaponItem weapon;
    public EquipmentItem equipment;

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
            CheckLevelUP();
        }
    }

    private void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel =+ 500;
        maxHealth += 500;
        health = maxHealth;
        maxMana += 200;
        mana = maxMana;
        attackDamage += 500;
        Defence += 200;
        combatSpeed += 2;
    }

    public CharacterAttributes deepCopy()
    {
        CharacterAttributes copy = ScriptableObject.CreateInstance<CharacterAttributes>();
        copy.level = this.health;
        copy.currentXP = this.currentXP;
        copy.xpToNextLevel = this.xpToNextLevel;
        copy.nameOfCharacter = this.nameOfCharacter;
        copy.health = this.health;
        copy.maxHealth = this.maxHealthOG;
        copy.maxMana = this.maxMana;
        copy.mana = this.mana;
        copy.combatSpeed = this.combatSpeed;
        copy.skillDamage = this.skillDamage;
        copy.attackDamage = this.attackDamage;
        copy.critChance = this.critChance;
        copy.effectChance = this.effectChance;
        copy.Defence = this.Defence;
        copy.expGive = this.expGive;
        copy.skills = this.skills; // List 
        copy.regions = this.regions;
        copy.isTurn = this.isTurn; 
        copy.isStuned = this.isStuned;
        copy.special =this.special;
        copy.special_count = this.special_count;
        copy.weapon = this.weapon;
        copy.equipment = this.equipment;

        return copy;
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

