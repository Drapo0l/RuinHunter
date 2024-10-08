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
    public Vector3 postionOG;
    public Camera cam;
    public GameObject enemyModel;
    public GameObject targetIndicatorE;
    private bool animatingAttack;
    void Start()
    {
        availableSkills = enemyStats.skills;
        animatingAttack = false;
    }

    void Update()
    {
        if(GameManager.Instance.combat && enemyStats.isTurn && !animatingAttack)
        {
            StartCoroutine(HandleTurnSequence());
        }
    }
    IEnumerator HandleTurnSequence()
    {
       
        //move forward
        animatingAttack = true;
        postionOG = enemyModel.transform.position;
        Vector3 attackPosition = new Vector3(postionOG.x + 2, postionOG.y, postionOG.z);
        yield return StartCoroutine(MoveToPosition(attackPosition, 1f));

        //attack
        HandleCombatActions();
        yield return new WaitForSeconds(1f);

        //move back
        yield return StartCoroutine(MoveToPosition(postionOG, 1f));

        animatingAttack = false;
        GameManager.Instance.EndTurn();
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        float timeElapsed = 0f;
        Vector3 startPosition = enemyModel.transform.position;

        while(timeElapsed < duration) 
        {
            enemyModel.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += duration;
            yield return null;
        }

        enemyModel.transform.position = targetPosition;
    }

    private void HandleCombatActions()
    {
        // Determine whether to use a skill or basic attack
       
        

            if (ShouldUseSkill() && availableSkills.Count > 0)
            {
                // Choose a random skill from available skills
                Skill chosenSkill = availableSkills[Random.Range(0, availableSkills.Count)];
            if (chosenSkill.Ptargit == 1)
            {
                
                
                    UseAttackSkill(chosenSkill);
                
               
            }
            if(chosenSkill.Ptargit == 0)
            {
                
                    UseAttackSkill(chosenSkill);
                
            }
            }
            else
            {
                // Perform a basic attack
                PerformBasicAttack();
            }
            
        
        
    }
    private void UseSupportSkill(Skill skill) // used to target the enemys for buffs  and such
    {
        int ran;
        GameObject target;
        if(skill.AOE == true)
        {
            for(int i = 0; i < GameManager.Instance.enemyObj.Count; i++)
            {
                target = GameManager.Instance.enemyObj[i];
                if (target.GetComponent<playerController>().playerStats.health <= 0)
                {
                    if (target != null)
                    {
                        // Calculate skill damage using any multipliers
                        float multiplier = GetSkillMultiplier(skill.elementType);

                        // Activate the skill, passing the player as the target
                        skill.ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
                        targetIndicatorE.SetActive(false);
                    }
                }
                else
                {
                   
                }
               
            }
           
        }
        else
        {
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
                targetIndicatorE.SetActive(false);
            }

        }
    }
    private void UseAttackSkill(Skill skill) // used for attacking the players
    {
        // Find the player to target
        int ran;
        GameObject target;
        if (skill.AOE == true)
        {
            for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)
            {
                target = GameManager.Instance.battleParty[i];
                if (target.GetComponent<playerController>().playerStats.health <= 0)
                {
                    if (target != null)
                    {
                        // Calculate skill damage using any multipliers
                        float multiplier = GetSkillMultiplier(skill.elementType);

                        // Activate the skill, passing the player as the target
                        skill.ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
                        targetIndicatorE.SetActive(false);
                    }
                }
                else
                {

                }

            }

        }
        else
        {
            while (true)
            {
                ran = Random.Range(1, GameManager.Instance.battleParty.Count) - 1;
                target = GameManager.Instance.battleParty[ran];
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
                targetIndicatorE.SetActive(false);
            }

        }
    }

    

    private bool ShouldUseSkill()
    {

        // Randomly decide if the enemy should use a skill, for now it's 50/50
        if (ty == PublicEnums.EnemyTypes.Agressive)
        {
            return Random.value > 0.7f;
        }
        if (ty == PublicEnums.EnemyTypes.CasterA)
        {
            return Random.value > 0.4f;
        }
        if (ty == PublicEnums.EnemyTypes.CasterP)
        {
            return Random.value > 0.2f;
        }
        if (ty == PublicEnums.EnemyTypes.Support)
        {
            return Random.value > 0f;
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
            targetIndicatorE.SetActive(false);
        }
    }

    public void TakeMeleeDamage(int damage, PublicEnums.WeaponType weaponType)
    {
        float multiplier = GetWeaponMultiplier(weaponType);
        damage = Mathf.FloorToInt(damage * multiplier);
        enemyStats.health -= damage;

        //FloatingNumberManager.Instance.ShowFloatingText(transform, damage, cam);

        GameManager.Instance.EndTurn();

        if (enemyStats.health <= 0)
        {
            GameManager.Instance.EnemyDeath(gameObject);
            Destroy(gameObject);
        }
    }

    public void TakeSkillDamage(int damage, PublicEnums.ElementType elementType)
    {
        float multiplier = GetSkillMultiplier(elementType);
        damage = Mathf.FloorToInt(damage * multiplier);
        enemyStats.health -= damage;

        //FloatingNumberManager.Instance.ShowFloatingText(transform, damage, cam);

        GameManager.Instance.EndTurn();

        if (enemyStats.health <= 0)
        {
            GameManager.Instance.EnemyDeath(gameObject);
            Destroy(gameObject);
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

    
}

