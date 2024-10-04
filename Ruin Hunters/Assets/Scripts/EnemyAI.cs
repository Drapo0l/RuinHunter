using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public string enemyName;
    public CharacterAttributes enemyStats;

    // Weaknesses
    public List<WeaponCalc> weaponsWeakness = new List<WeaponCalc>();
    public List<ElementCalc> elementWeakness = new List<ElementCalc>();
    List<Skill> availableSkills;
    public PublicEnums.EnemyTypes ty;

    void Start()
    {
        
    }

    void Update()
    {
        if(GameManager.Instance.combat)
        {
            if(enemyStats.isTurn)
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
            if (chosenSkill.Ptargit == 1)
            {
                UseAttackSkill(chosenSkill);
            }
            if(chosenSkill.Ptargit == 0)
            {

            }
            }
            else
            {
                // Perform a basic attack
                PerformBasicAttack();
            }
            
        
        EndTurn();
    }
    private void UseSupportSkill(Skill skill) // used to target the enemys for buffs  and such
    {
        int ran;
        GameObject target;
        while (true)
        {
            ran = Random.Range(1, GameManager.Instance.enemyObj.Count) - 1;
            target = GameManager.Instance.enemyObj[ran];
            if (target.GetComponent<playerController>().playerStats.health <= 0)
            {

            }
            else
            {
                break;
            }
        }
        if (target != null)
        {
            // Calculate skill damage using any multipliers
            float multiplier = GetSkillMultiplier(skill.elementType);

            // Activate the skill, passing the player as the target
            skill.ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
        }
    }
    private void UseAttackSkill(Skill skill) // used for attacking the players
    {
        // Find the player to target
        int ran;
        GameObject target;
        while (true)
        {
            ran = Random.Range(1, PartyManager.Instance.startingPlayerParty.Count) - 1;
            target = PartyManager.Instance.startingPlayerParty[ran];
            if (target.GetComponent<playerController>().playerStats.health <= 0)
            {

            }
            else
            {
                break;
            }
        }
        if (target != null)
        {
            // Calculate skill damage using any multipliers
            float multiplier = GetSkillMultiplier(skill.elementType);

            // Activate the skill, passing the player as the target
            skill.ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
        }
    }

    private bool ShouldUseSkill()
    {

        // Randomly decide if the enemy should use a skill, for now it's 50/50
        if (ty == PublicEnums.EnemyTypes.Agressive)
        {
            return Random.value > 0.3f;
        }
        if (ty == PublicEnums.EnemyTypes.CasterA)
        {
            return Random.value > 0.6f;
        }
        if (ty == PublicEnums.EnemyTypes.CasterP)
        {
            return Random.value > 0.8f;
        }
        if (ty == PublicEnums.EnemyTypes.Support)
        {
            return Random.value > 1f;
        }
            return Random.value > 0.5f;
    }

    private void PerformBasicAttack()
    {
        int ran;
        GameObject target;
        while (true)
        {
             ran = Random.Range(1, PartyManager.Instance.startingPlayerParty.Count) - 1;
             target = PartyManager.Instance.startingPlayerParty[ran];
            if (target.GetComponent<playerController>().playerStats.health <= 0)
            {

            }
            else
            {
                break;
            }
        }
        if (target != null)
        {
            // Get the weapon weakness multiplier based on player's weaknesses
            float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword); // Example weapon
            
            // Activate the weapon attack
            Skill weaponAttack = new Skill(); 
            weaponAttack.ActivateWeaponAttack(target, enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance); // Example power 10
        }
    }

    public void TakeMeleeDamage(int damage, PublicEnums.WeaponType weaponType)
    {
        float multiplier = GetWeaponMultiplier(weaponType);
        damage = Mathf.FloorToInt(damage * multiplier);
        enemyStats.health -= damage;

        GameManager.Instance.EndTurn();

        if (enemyStats.health <= 0)
        {
            //dead
        }
    }

    public void TakeSkillDamage(int damage, PublicEnums.ElementType elementType)
    {
        float multiplier = GetSkillMultiplier(elementType);
        damage = Mathf.FloorToInt(damage * multiplier);
        enemyStats.health -= damage;

        GameManager.Instance.EndTurn();

        if (enemyStats.health <= 0)
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
        enemyStats.isTurn = true;
    }

    public void EndTurn()
    {
        enemyStats.isTurn = false;
    }
}

