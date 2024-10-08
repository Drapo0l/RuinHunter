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
    void Start()
    {
        availableSkills = enemyStats.skills;
    }

    void Update()
    {
        if(GameManager.Instance.combat)
        {
            if(enemyStats.isTurn)
            {
                EndTurn();
                availableSkills = enemyStats.skills;
                postionOG = enemyModel.transform.position;
                StartCoroutine(combatpause());
                enemyModel.transform.position = new Vector3(enemyModel.transform.position.x + 2, enemyModel.transform.position.y, enemyModel.transform.position.z);
                HandleCombatActions();
                



            }
        }
    }
    IEnumerator combatpause()
    {
        yield return new WaitForSeconds(1f);

        

       
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
                
                
                    UseSupportSkill(chosenSkill);
                
               
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
            
        
        EndTurn();
    }
    private void UseSupportSkill(Skill skill) // used to target the enemys for buffs  and such
    {
        int ran;
        GameObject target = null;
        if(skill.AOE == true)
        {
            for(int i = 0; i < GameManager.Instance.enemyObj.Count; i++)
            {
                target = GameManager.Instance.enemyObj[i];
                if (target.GetComponent<playerController>().playerStats.health <= 0)
                {
                    if (target != null)
                    {
                        targetIndicatorE.transform.position = target.transform.position;
                        targetIndicatorE.transform.position = new Vector3(targetIndicatorE.transform.position.x, targetIndicatorE.transform.position.y + 9, targetIndicatorE.transform.position.x);
                        targetIndicatorE.SetActive(true);
                        StartCoroutine(combatpause());
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
            bool pick = true;
            while (pick == true)
            {
                ran = Random.Range(0, GameManager.Instance.battleParty.Count) - 1;
                if (ran < 0)
                {
                    ran = 0;
                }
                target = GameManager.Instance.battleParty[ran];
                if (target.GetComponent<playerController>().playerStats.health <= 0)
                {

                }
                else
                {
                    pick = false;
                }
            }
            targetIndicatorE.transform.position = target.transform.position;
            targetIndicatorE.transform.position = new Vector3(targetIndicatorE.transform.position.x, targetIndicatorE.transform.position.y + 9, targetIndicatorE.transform.position.x);
            targetIndicatorE.SetActive(true);
            StartCoroutine(combatpause());
            if (target != null)
            {
                // Calculate skill damage using any multipliers
                float multiplier = GetSkillMultiplier(skill.elementType);

                // Activate the skill, passing the player as the target
                skill.ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
                targetIndicatorE.SetActive(false);
            }

        }
        StartCoroutine(combatpause());
        enemyModel.transform.position = postionOG;
        GameManager.Instance.EndTurn();
        

    }
    private void UseAttackSkill(Skill skill) // used for attacking the players
    {
        // Find the player to target
        int ran;
        GameObject target = null;
        if (skill.AOE == true)
        {
            for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)
            {
                target = GameManager.Instance.battleParty[i]; 
                if (target.GetComponent<playerController>().playerStats.health <= 0)
                {
                    targetIndicatorE.transform.position = target.transform.position;
                    targetIndicatorE.transform.position = new Vector3(targetIndicatorE.transform.position.x, targetIndicatorE.transform.position.y + 9, targetIndicatorE.transform.position.x);
                    targetIndicatorE.SetActive(true);
                    StartCoroutine(combatpause());
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
            bool pick = true;
            while (pick == true)
            {
                ran = Random.Range(0, GameManager.Instance.battleParty.Count) - 1;
                if(ran < 0)
                {
                    ran = 0;
                }
                target = GameManager.Instance.battleParty[ran];
                if (target.GetComponent<playerController>().playerStats.health <= 0)
                {

                }
                else
                {
                    pick = false;
                }
            }

            targetIndicatorE.transform.position = target.transform.position;
            targetIndicatorE.transform.position = new Vector3(targetIndicatorE.transform.position.x, targetIndicatorE.transform.position.y + 9, targetIndicatorE.transform.position.x);
            targetIndicatorE.SetActive(true);
            StartCoroutine(combatpause());

            if (target != null)
            {
                // Calculate skill damage using any multipliers
                float multiplier = GetSkillMultiplier(skill.elementType);

                // Activate the skill, passing the player as the target
                skill.ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
                targetIndicatorE.SetActive(false);
            }

        }
        StartCoroutine(combatpause());
        enemyModel.transform.position = postionOG;
        GameManager.Instance.EndTurn();
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
        GameObject target = null;
        bool pick = true;
        while (pick == true)
        {
             ran = Random.Range(0, GameManager.Instance.battleParty.Count) - 1;
            if (ran < 0)
            {
                ran = 0;
            }
            target = GameManager.Instance.battleParty[ran];
            if (target.GetComponent<playerController>().playerStats.health <= 0)
            {

            }
            else
            {
                pick = false;
            }
        }
        targetIndicatorE.transform.position = target.transform.position;
        targetIndicatorE.transform.position = new Vector3(targetIndicatorE.transform.position.x, targetIndicatorE.transform.position.y + 9, targetIndicatorE.transform.position.x);
        targetIndicatorE.SetActive(true);
        StartCoroutine(combatpause());
        if (target != null)
        {
            // Get the weapon weakness multiplier based on player's weaknesses
            float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword); // Example weapon
            
            // Activate the weapon attack
            Skill weaponAttack = new Skill(); 
            weaponAttack.ActivateWeaponAttack(target, enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance); 
            targetIndicatorE.SetActive(false);
        }
        StartCoroutine(combatpause());
        enemyModel.transform.position = postionOG;
        GameManager.Instance.EndTurn();
    }

    public void TakeMeleeDamage(int damage, PublicEnums.WeaponType weaponType)
    {
        float multiplier = GetWeaponMultiplier(weaponType);
        damage = Mathf.FloorToInt(damage * multiplier);
        enemyStats.health -= damage;

        FloatingNumberManager.Instance.ShowFloatingText(transform, damage, cam);

        GameManager.Instance.EndTurn();

        if (enemyStats.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeSkillDamage(int damage, PublicEnums.ElementType elementType)
    {
        float multiplier = GetSkillMultiplier(elementType);
        damage = Mathf.FloorToInt(damage * multiplier);
        enemyStats.health -= damage;

        FloatingNumberManager.Instance.ShowFloatingText(transform, damage, cam);

        GameManager.Instance.EndTurn();
        
        if (enemyStats.health <= 0)
        {
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

    public void EndTurn()
    {
        enemyStats.isTurn = false;
    }
}

