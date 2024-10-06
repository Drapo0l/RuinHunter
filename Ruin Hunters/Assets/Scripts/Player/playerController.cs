using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    public string characterName;
    public CharacterAttributes playerStats;

    public float speed;
    public float GroundDist;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    public LayerMask ignorePlayerLayer;

    public PublicEnums.WeaponType playerWeapon;
    // Angel's polo angel equip Item
    public InventoryItem equippedItem;  // to show if the player has the equpied item or not and have it be equpied

    // Create List to hold strengths and weaknesses
    public List<WeaponCalc> weaponsWeakness = new List<WeaponCalc>();
    public List<ElementCalc> elementWeakness = new List<ElementCalc>();

    public PlayerActionSelector actionSelector; // refernece to action selector

    public Camera cam;

    public int defended = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
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
            if(playerStats.isTurn) 
            {
                if (defended != 0)
                {
                   
                    playerStats.Defence = defended;
                    defended = 0;
                }
                
                // when it's the player's turn, show the menu
                actionSelector.ShowMenu(transform, this, playerStats.skills);                
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

    

    public void TakeSkillDamage(int damage, PublicEnums.ElementType elementType)
    {
        float multiplier = GetSkillMultiplier(elementType);
        damage = Mathf.FloorToInt(damage * multiplier);
        playerStats.health -= damage;

        FloatingNumberManager.Instance.ShowFloatingText(transform, damage, cam);

        

        if (playerStats.health <= 0)
        {
            //died
        }
    }

    public void TakeMeleeDamage(int damage, PublicEnums.WeaponType weaponType)
    {
        float multiplier = GetMeleeMultiplier(weaponType);
        damage = Mathf.FloorToInt(damage * multiplier);
        playerStats.health -= damage;

        FloatingNumberManager.Instance.ShowFloatingText(transform, damage, cam);

       

        if (playerStats.health <= 0)
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
        if (index < playerStats.skills.Count)
        {
            Skill skillToUse = playerStats.skills[index];
            // Implement logic for using the skill, e.g., apply damage
        }
    }

    public float GetSkillMultiplier(PublicEnums.ElementType elementType)
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

    public float GetMeleeMultiplier(PublicEnums.WeaponType weaponType)
    {
        foreach (var weakness in weaponsWeakness)
        {
            if (weakness.weaponType == weaponType)
            {
                return weakness.weaponMultiplier;
            }
        }
        return 1f;
    }

    // Polo Angel's code
    public void Equip(InventoryItem item)  
    {
        if (item == null) // if null, you can't equip it and gives a error message
        {
            Debug.LogError("Cannot equip a null item!");
            return;
        }
        // equips the item on the player
        equippedItem = item;
        Debug.Log($"Equipped: {item.label}");
    }
}
