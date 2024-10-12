using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        if (GameManager.Instance.combat && enemyStats.isTurn && !animatingAttack)
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

        while (timeElapsed < duration)
        {
            enemyModel.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += duration;
            yield return null;
        }

        enemyModel.transform.position = targetPosition;
    }

    private void Elite_AI() // for eliets and bosses 
    {
        if (ty == PublicEnums.EnemyTypes.Elite_forest_1) // forest elite will be a blue cloaked assassin 
        {
            if (enemyStats.special_count == 0) // will start the match with 1 of 4 diffrent special moves and will do so every 3 turns after 
            {
                enemyStats.special_count = 3;
                enemyStats.special = true;
            }
            if (enemyStats.special == true)
            {
                Skill chosenSkill = availableSkills[Random.Range(0,4)];
                if (chosenSkill == availableSkills[0] | chosenSkill == availableSkills[1])
                {
                    int kc;
                    kc = GameManager.Instance.battleParty.Count - 1;
                    GameObject target = null;
                    for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)
                    {
                        target = GameManager.Instance.battleParty[i];
                        if (target.GetComponent<playerController>().playerStats.health <= 0)
                        {
                            if (target != null)
                            {
                                if (kc < GameManager.Instance.battleParty.Count - 1)
                                {
                                    i--;
                                    kc--;
                                }
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
                }
                else
                {
                    UseAttackSkill(chosenSkill); // the last 2 are direct attacks 1 with a high crit rate another that drops the speed of the target 
                    PerformBasicAttack(); // then do a basic attack 
                }

                enemyStats.special = false;
            }
            else // then the basic attacks with some skills that fit him
            {
                int ran2;
                ran2 = Random.Range(0, 101);
                if (ran2 < 0)
                {
                    ran2 = 1;
                }
                if (ran2 < 70)
                {
                    Skill chosenSkill2 = availableSkills[Random.Range(4, availableSkills.Count)];
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
                enemyStats.special_count--;
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
                    int kc;
                    kc = GameManager.Instance.battleParty.Count - 1;
                    GameObject target = null;
                    for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)
                    {
                        target = GameManager.Instance.battleParty[i];
                        if (target.GetComponent<playerController>().playerStats.health <= 0)
                        {
                            if (target != null)
                            {
                                if (kc < GameManager.Instance.battleParty.Count - 1)
                                {
                                    i--;
                                    kc--;
                                }
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
                    int kc;
                    kc = GameManager.Instance.battleParty.Count - 1;
                    GameObject target = null;
                    for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)// and will do an aoe that lowers the defence of the party this will then switch imbetween both for the remainder of the fight
                    {
                        target = GameManager.Instance.battleParty[i];
                        if (target.GetComponent<playerController>().playerStats.health <= 0)
                        {
                            if (target != null)
                            {
                                if (kc < GameManager.Instance.battleParty.Count - 1)
                                {
                                    i--;
                                    kc--;
                                }
                                // Calculate skill damage using any multipliers
                                float multiplier = GetSkillMultiplier(availableSkills[4].elementType);

                                // Activate the skill, passing the player as the target
                                availableSkills[4].ActivateSkill(target, enemyStats.attackDamage, multiplier, enemyStats.critChance, enemyStats.effectChance); // Attacker power is set to 10 for now
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
            if (enemyStats.health < enemyStats.maxHealthOG / 2) // under 50% hp he attacks 2 times
            {
                int ran2;
                ran2 = Random.Range(0, 101);
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
            ran = Random.Range(0, 101);
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
                int kc;
                kc = GameManager.Instance.battleParty.Count - 1;
                GameObject target = null;
                for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)
                {
                    target = GameManager.Instance.battleParty[i];
                    if (target.GetComponent<playerController>().playerStats.health <= 0)
                    {
                        if (target != null)
                        {
                            if (kc < GameManager.Instance.battleParty.Count - 1)
                            {
                                i--;
                                kc--;
                            }
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
            else if (enemyStats.special_count != 5 | enemyStats.special_count != 0) // normaly tho he has a 50/50 chance to use a skill or normal attack
            {
                enemyStats.special_count++;
                int ran;
                ran = Random.Range(0, 101);
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
            if (enemyStats.special_count == 0) // his special is to do 5 basic attacks and lower his defence for 3 turns 
            {
                enemyStats.special_count = 10;
                enemyStats.special = true;

            }
            if (enemyStats.special == true)
            {
                PerformBasicAttack();
                PerformBasicAttack();
                PerformBasicAttack();
                PerformBasicAttack();
                PerformBasicAttack();
                float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword);
                availableSkills[0].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance); // this lowers his own defence
            }
            else
            {
                enemyStats.special_count--;
                int ran;
                ran = Random.Range(0, 101);
                if (ran < 0)
                {
                    ran = 1;
                }
                if (ran < 40)
                {
                    Skill chosenSkill2 = availableSkills[Random.Range(2, availableSkills.Count)];
                    if (chosenSkill2.Ptargit == 1)
                    {


                        UseSupportSkill(chosenSkill2);


                    }
                    if (chosenSkill2.Ptargit == 0)
                    {

                        UseAttackSkill(chosenSkill2);

                    }
                    PerformBasicAttack(); // and after any skill he basic attacks
                }
                else
                {
                    // Perform a basic attack 2 times 
                    PerformBasicAttack();
                    PerformBasicAttack();
                }
            }
            if (enemyStats.special_count == 3)
            {
                float weaponMultiplier = GetWeaponMultiplier(PublicEnums.WeaponType.Sword);
                availableSkills[1].ActivateSkill(GameManager.Instance.enemyObj[0], enemyStats.attackDamage, weaponMultiplier, enemyStats.critChance, enemyStats.effectChance); // after that he will clense his debuffs 
            }

        }
        if (ty == PublicEnums.EnemyTypes.Boss_1_Main) // the main boss is a ruin beast its 1 great gem in the shape of an eye that then makes 2 diffrent arms 1 being made of red gems being the left arm and a blue one made of blue gems
        {
            Skill chosenSkill2 = availableSkills[Random.Range(0, availableSkills.Count)];
            if (chosenSkill2.Ptargit == 1)
            {


                UseSupportSkill(chosenSkill2);


            }
            if (chosenSkill2.Ptargit == 0)
            {

                UseAttackSkill(chosenSkill2);

            }
        }
        if (ty == PublicEnums.EnemyTypes.Boss_1_L_arm) // the left arm is pure basic attack with a 50% chance to stun when below 50% health 
        {
            if (enemyStats.health <= enemyStats.maxHealth)
            {
                int ran;
                ran = Random.Range(0, 101);
                if (ran < 0)
                {
                    ran = 1;
                }
                if (ran < 50)
                {
                    UseAttackSkill(availableSkills[0]);
                }
            }
            PerformBasicAttack();
            PerformBasicAttack();
        }
        if (ty == PublicEnums.EnemyTypes.Boss_1_R_arm) // the right arm is a spell caster only casting spells when below 50% hp they have a 50% chance to use a defence droppting skill 
        {
            if (enemyStats.health <= enemyStats.maxHealth)
            {
                int ran;
                ran = Random.Range(0, 101);
                if (ran < 0)
                {
                    ran = 1;
                }
                if (ran < 50)
                {
                    UseAttackSkill(availableSkills[0]);
                }
            }
            Skill chosenSkill2 = availableSkills[Random.Range(0, availableSkills.Count )];
            if (chosenSkill2.Ptargit == 1)
            {


                UseSupportSkill(chosenSkill2);


            }
            if (chosenSkill2.Ptargit == 0)
            {

                UseAttackSkill(chosenSkill2);

            }
        }
    }

    private void HandleCombatActions()
    {
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
    }
    private void UseSupportSkill(Skill skill) // used to target the enemys for buffs  and such
    {
        int ran;
        GameObject target;
        if (skill.AOE == true)
        {
            int kc;
            kc = GameManager.Instance.battleParty.Count - 1;
            for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)
            {
                target = GameManager.Instance.battleParty[i];
                if (target.GetComponent<playerController>().playerStats.health <= 0)
                {
                    if (target != null)
                    {
                        if (kc < GameManager.Instance.battleParty.Count - 1)
                        {
                            i--;
                            kc--;
                        }
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
                ran = Random.Range(1, GameManager.Instance.enemyObj.Count);
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

            int kc;
            kc = GameManager.Instance.battleParty.Count-1;
            for (int i = 0; i < GameManager.Instance.battleParty.Count; i++)
            {
                target = GameManager.Instance.battleParty[i];
                if (target.GetComponent<playerController>().playerStats.health <= 0)
                {
                    if (target != null)
                    {
                        if(kc < GameManager.Instance.battleParty.Count - 1)
                        {
                            i--;
                            kc--;
                        }
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
                ran = Random.Range(0, GameManager.Instance.battleParty.Count);
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
            ran = Random.Range(0, PartyManager.Instance.startingPlayerParty.Count);
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
        Vector3 targetPosition = transform.position;
        DamageNumberManager.Instance.ShowNumbers(targetPosition, damage);

        

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
        Vector3 targetPosition = transform.position;
        DamageNumberManager.Instance.ShowNumbers(targetPosition, damage);

        

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

