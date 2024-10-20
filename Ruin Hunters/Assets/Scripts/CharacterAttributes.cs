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
    public AudioSource attacker;
    public AudioSource target_BA;
    public AudioClip[] Activation_Sound;
    public float Activation_SoundV;
    public AudioClip[] Hit_Sound;
    public float Hit_SoundV;
    public List<AudioClip[]> special_sounds = new List<AudioClip[]>();
    public float special_soundsV;
    public  ParticleManager ParticleForBasicAttack;
    public ParticleManager ParticleForSpecial;
   

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
}



[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class AddSkill : ScriptableObject
{
    public string skillName;
    public PublicEnums.ElementType elementType;
    public int baseDamage;
    public int manaCost;
}

