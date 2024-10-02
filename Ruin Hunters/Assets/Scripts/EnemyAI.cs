using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public string enemyName;
    public CharacterComponent enemyAttributes;
    public CharacterAttributes enemyStats;

    // Weaknesses
    public List<WeaponCalc> weaponsWeakness = new List<WeaponCalc>();
    public List<ElementCalc> elementWeakness = new List<ElementCalc>();
    List<Skill> availableSkills;

    void Start()
    {
        enemyAttributes = new CharacterComponent(enemyStats);
        availableSkills = enemyAttributes.stats.skills;
    }

    void Update()
    {
        if(GameManager.Instance.combat)
        {
            if(enemyAttributes.stats.isTurn)
            {
                HandleCombatActions();
            }
        }
    }

    private void HandleCombatActions()
    {
        // Determine whether to use a skill or basic attack
        if (ShouldUseSkill())
        {
            // Choose a random skill from available skills
            Skill chosenSkill = availableSkills[Random.Range(0, availableSkills.Count)];
            UseSkill(chosenSkill);
        }
        else
        {
            // Perform a basic attack
            PerformBasicAttack();
        }

        EndTurn();
    }

    private void UseSkill(Skill skill)
    {
        // Find the player to target
        playerController player = FindObjectOfType<playerController>();
        if (player != null)
        {
            // Calculate skill damage using any multipliers
            float multiplier = GetSkillMultiplier(skill.elementType);

            // Activate the skill, passing the player as the target
            skill.ActivateSkill(player.gameObject, enemyAttributes.stats.skillDamage, multiplier); // Attacker power is set to 10 for now
        }
    }

    private bool ShouldUseSkill()
    {
        // Randomly decide if the enemy should use a skill, for now it's 50/50
        return Random.value > 0.5f;
    }

    private void PerformBasicAttack()
    {
        playerController player = FindObjectOfType<playerController>();
        if (player != null)
        {
            // Get the weapon weakness multiplier based on player's weaknesses
            float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword); // Example weapon

            // Activate the weapon attack
            Skill weaponAttack = new Skill(); 
            weaponAttack.ActivateWeaponAttack(player.gameObject, enemyAttributes.stats.attackDamage, weaponMultiplier); // Example power 10
        }
    }

    public void TakeSkillDamage(int damage, PublicEnums.ElementType elementType)
    {
        float multiplier = GetSkillMultiplier(elementType);
        damage = Mathf.FloorToInt(damage * multiplier);
        enemyAttributes.stats.health -= damage;
       

        if (enemyAttributes.stats.health <= 0)
        {
            //dead
        }
    }

    public void TakeMeleeDamage(int damage, PublicEnums.WeaponType weaponType)
    {
        float multiplier = GetWeaponMultiplier(weaponType);
        damage = Mathf.FloorToInt(damage * multiplier);
        enemyAttributes.stats.health -= damage;


        if (enemyAttributes.stats.health <= 0)
        {
            //dead
        }
    }

    public float GetSkillMultiplier(PublicEnums.ElementType elementType)
    {
        foreach (var weakness in elementWeakness)
        {
            if (weakness.elementType == elementType)
            {
                return weakness.elementMultiplier; // This is your weakness multiplier
            }
        }
        return 1f; // Neutral damage
    }

    public float GetWeaponMultiplier(PublicEnums.WeaponType weaponType)
    {
        foreach (var weakness in weaponsWeakness)
        {
            if (weakness.weaponType == weaponType)
            {
                return weakness.weaponMultiplier; // Apply weapon weakness multiplier
            }
        }
        return 1f; // Neutral weapon damage
    }

    public void StartTurn()
    {
        enemyAttributes.stats.isTurn = true;
    }

    public void EndTurn()
    {
        enemyAttributes.stats.isTurn = false;
    }
}

