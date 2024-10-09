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


        if (ty == PublicEnums.EnemyTypes.Agressive | ty == PublicEnums.EnemyTypes.CasterA | ty == PublicEnums.EnemyTypes.CasterP | ty == PublicEnums.EnemyTypes.Normal | ty == PublicEnums.EnemyTypes.Support)
        {
            if (ShouldUseSkill())
            {
                // Choose a random skill from available skills
                Skill chosenSkill = availableSkills[Random.Range(0, availableSkills.Count)];
                if (chosenSkill.Ptargit == 1)
                {


                    UseSupportSkill(chosenSkill);


                }
                if (chosenSkill.Ptargit == 0)
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
        else
        {
            Elite_AI();
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
    private void Elite_AI() // for eliets and bosses 
    {
        if(ty== PublicEnums.EnemyTypes.Elite_forest_1) // forest elite will be a blue cloaked assassin 
        {
            if(enemyStats.special_count == 0)
            {
                enemyStats.special_count = 3;
                enemyStats.special = true;
            }
            if (enemyStats.special == true)
            {
                Skill chosenSkill = availableSkills[0];


                enemyStats.special = false;
            }
            else
            {
                Skill chosenSkill = availableSkills[Random.Range(1, availableSkills.Count)];
            }
            
        }
        if (ty == PublicEnums.EnemyTypes.Elite_dessert_1) // desert elite will be an ice wolf 
        {
            if (enemyStats.special_count == 0) // at the start he will houl 
            {
               
                if (enemyStats.special == false) // this will change the day time dessert to a full moon blizzard 
                {
                    enemyStats.special_count = 4;
                    float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword);
                    availableSkills[5].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance); // this clenses all his effects
                    availableSkills[0].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance); // and gives him an attack buff 
                    for (int i = 0; i < GameManager.Instance.battleParty.Count; i++) // then will do an aoe on the party doing ice dmg 
                    {
                        GameObject target = null;
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
                                float multiplier = GetSkillMultiplier(availableSkills[3].elementType);

                                // Activate the skill, passing the player as the target
                                availableSkills[3].ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
                                targetIndicatorE.SetActive(false);
                            }
                        }
                        else
                        {

                        }

                    }
                    enemyStats.special_count++;
                    enemyStats.special = true;
                }
                if (enemyStats.special == true) // if the full moon is out it will change to a new moon the blizzard will stop 
                {
                    enemyStats.special_count = 4;
                    float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword);
                    availableSkills[5].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance); // he will clense his debuffs 
                    availableSkills[2].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance); // and heal himself 
                    for (int i = 0; i < GameManager.Instance.battleParty.Count; i++) // and will do an aoe that lowers the defence of the party this will then switch imbetween both for the remainder of the fight
                    {
                        GameObject target = null;
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
                                float multiplier = GetSkillMultiplier(availableSkills[4].elementType);

                                // Activate the skill, passing the player as the target
                                availableSkills[2].ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
                                targetIndicatorE.SetActive(false);
                            }
                        }
                        else
                        {

                        }

                    }
                    enemyStats.special_count++;
                    enemyStats.special = false;
                }


            }
            if (enemyStats.health < enemyStats.maxHealthOG/2) // under 50% hp he attacks 2 times
            {
                int ran2;
                ran2 = Random.Range(0, 100);
                if (ran2 < 0)
                {
                    ran2 = 1;
                }
                if (ran2 < 55)
                {
                    Skill chosenSkill2 = availableSkills[Random.Range(6, availableSkills.Count)];
                    if (chosenSkill2.Ptargit == 1)
                    {


                        UseSupportSkill(chosenSkill2);


                    }
                    if (chosenSkill2.Ptargit == 0)
                    {

                        UseAttackSkill(chosenSkill2);

                    }
                }
                else
                {
                    // Perform a basic attack
                    PerformBasicAttack();
                }
            }
            enemyStats.special_count--;
            int ran;
            ran = Random.Range(0, 100);
            if (ran < 0)
            {
                ran = 1;
            }
            if (ran < 55)
            {
                Skill chosenSkill = availableSkills[Random.Range(6, availableSkills.Count)];
                if (chosenSkill.Ptargit == 1)
                {


                    UseSupportSkill(chosenSkill);


                }
                if (chosenSkill.Ptargit == 0)
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
        if (ty == PublicEnums.EnemyTypes.Elite_ice_1) // ice elite will be a steam mariachi bot
        {
            if (enemyStats.special_count == 0) // this will start at 1 but at 0 he will clense his debuffs from his explosive finish
            {

                float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword);
                availableSkills[3].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance);
                enemyStats.special_count++;
            }
           
                if (enemyStats.special_count == 5) // at 5 he over clocks doing 4 basic attacks then he explodes doing masive dmg to the whole party
            {
                float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword);
                PerformBasicAttack();
                PerformBasicAttack();
                PerformBasicAttack();
                PerformBasicAttack();
                for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)
                {
                    GameObject target = null;
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
                            float multiplier = GetSkillMultiplier(availableSkills[3].elementType);

                            // Activate the skill, passing the player as the target
                            availableSkills[2].ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
                            targetIndicatorE.SetActive(false);
                        }
                    }
                    else
                    {

                    }

                }
                enemyStats.special_count = 0;


                enemyStats.special = true;
            }
                if (enemyStats.special == true) // after that tho he loses defence and speed
            {
                Skill chosenSkill = availableSkills[0];
                float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword);
                availableSkills[0].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance);
                availableSkills[1].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance);

                enemyStats.special = false;
            }
            else if(enemyStats.special_count != 5 | enemyStats.special_count != 0) // normaly tho he has a 50/50 chance to use a skill or normal attack
            {
                enemyStats.special_count++;
                int ran;
                ran = Random.Range(0, 100);
                if (ran < 0)
                {
                    ran = 1;
                }
                if (ran < 55)
                {
                    Skill chosenSkill = availableSkills[Random.Range(4, availableSkills.Count)];
                    if (chosenSkill.Ptargit == 1)
                    {


                        UseSupportSkill(chosenSkill);


                    }
                    if (chosenSkill.Ptargit == 0)
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
           
              
            
        }
        if (ty == PublicEnums.EnemyTypes.Elite_ruin_1) // ruin elite will be a booster worrior  
        {
            if (enemyStats.special_count == 0)
            {
                enemyStats.special_count = 10;
                enemyStats.special = true;
                if (enemyStats.special == true)
                {

                }
            }
            Skill chosenSkill = availableSkills[Random.Range(0, availableSkills.Count)];
        }
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

