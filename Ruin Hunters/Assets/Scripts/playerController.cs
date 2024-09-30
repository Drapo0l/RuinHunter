using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    public string characterName;
    public CharacterAttributes characterAttributes;

    public float speed;
    public float GroundDist;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    public LayerMask ignorePlayerLayer;

    // Create List to hold strengths and weaknesses
    public List<WeaponCalc> weaponsWeakness = new List<WeaponCalc>();
    public List<ElementCalc> elementWeakness = new List<ElementCalc>();

    public PlayerActionSelector actionSelector; // refernece to action selector

    private bool isTurn;   

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        characterAttributes = new CharacterAttributes(characterName, true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!GameManager.Instance.combat)
        {
            CharacterMovement();
        }
        else
        {
            if(isTurn) 
            {
                // when it's the player's turn, show the menu
                actionSelector.ShowMenu(transform);
            }
            else
            {
                actionSelector.HideMenu();
            }
        }
        

    }

    private void CharacterMovement()
    {
        // raycast to check ground
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;

        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer & ~ignorePlayerLayer))
        {
            if (hit.collider != null)
            {
                //only adjust player y based on the distance from gound
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + GroundDist;
                transform.position = movePos;
            }
        }

        //horizontal movement
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y);
        rb.velocity = moveDir * speed;

        //flip sprite depending on direction
        if (x != 0 && x < 0)
        {
            sr.flipX = false;
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = true;
        }
    }

    private void HandleCombatActions()
    {
        // Turn-based combat logic (attacks, skills, etc.)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Example combat action, like a basic attack
            PerformBasicAttack();
        }

        // Once the player finishes their turn, set isTurn to false
       
    }

    private void PerformBasicAttack()
    {
        // Handle basic attack logic here
    }

    public void TakeDamage(int damage, PublicEnums.ElementType elementType)
    {
        float multiplier = GetDamageMultiplier(elementType);
        damage = Mathf.FloorToInt(damage * multiplier);
        characterAttributes.health -= damage;

        if (characterAttributes.health <= 0)
        {
            //died
        }
    }

    public void UseItem(int index)
    {
        List<Item> inventoryItems = InventoryManager.instance.GetItems();
        if (index < inventoryItems.Count)
        {
            Item itemToUse = inventoryItems[index];
            // Implement logic for using the item, e.g., healing
            Debug.Log($"Using {itemToUse.itemName}");
            InventoryManager.instance.RemoveItem(itemToUse); // Optionally remove the item after use
        }
    }

    private void UseSkill(int index)
    {
        if (index < characterAttributes.skills.Count)
        {
            Skill skillToUse = characterAttributes.skills[index];
            // Implement logic for using the skill, e.g., apply damage
        }
    }

    public float GetDamageMultiplier(PublicEnums.ElementType elementType)
    {
        foreach (var weakness in elementWeakness)
        {
            if(weakness.elementType == elementType)
            {
                return weakness.elementMultiplier;
            }
        }
        return 1f;
    }

}