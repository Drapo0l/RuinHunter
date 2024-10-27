using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour, IDamage
{
    public Sprite Sprite;
    public string characterName;
    public CharacterAttributes playerStats;

    public float speed;
    public float GroundDist;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    public LayerMask ignorePlayerLayer;

    public PublicEnums.WeaponType playerWeapon;

    // Create List to hold strengths and weaknesses
    public List<WeaponCalc> weaponsWeakness = new List<WeaponCalc>();
    public List<ElementCalc> elementWeakness = new List<ElementCalc>();

    public PlayerActionSelector actionSelector; // refernece to action selector

    public Camera cam;

    public int defended = 0;

    public Animator playerAnimator;

    // New variables for footstep sounds
    public AudioSource footstepSource;
    public List<AudioClip> grassFootstepClips;
    public List<AudioClip> desertFootstepClips;
    public List<AudioClip> snowFootstepClips;
    public List<AudioClip> ruinFootstepClips;
    public float footstepDelay = 0.5f;
    private float nextFootstepTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(!GameManager.Instance.leveling && !GameManager.Instance.combat && !CharacterMenuManager.Instance.inUI)
        {
            CharacterMovement();
        }
        else if (GameManager.Instance.combat)
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



    //// Polo Angel's code
    //public void EquipWeapon(InventoryItem item)
    //{
    //    if (item == null) // if null, you can't equip it and gives a error message
    //    {
    //        Debug.LogError("Cannot equip a null item!");
    //        return;
    //    }
    //    // equips the item on the player



    //    equippedWeapon = item;
    //    playerWeapon = equippedWeapon.weaponType;

    //}

    //public void EquipArmour(InventoryItem item)
    //{
    //    if (item == null) // if null, you can't equip it and gives a error message
    //    {
    //        Debug.LogError("Cannot equip a null item!");
    //        return;
    //    }
    //    // equips the item on the player
    //    equippedArmour = item;
    //    playerArmour = equippedArmour.ArmourType;
    //    Debug.Log($"Equipped: {item.label}");
    //}
    //public void EquipAccessory(InventoryItem item)
    //{
    //    if (item == null) // if null, you can't equip it and gives a error message
    //    {
    //        Debug.LogError("Cannot equip a null item!");
    //        return;
    //    }
    //    // equips the item on the player
    //    equippedAccessory = item;
    //    playerAccessory = equippedAccessory.AccessoryType;
    //    Debug.Log($"Equipped: {item.label}");
    //}
    public void CharacterMovement()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position + Vector3.up;
        if (Physics.Raycast(castPos, -Vector3.up, out hit, Mathf.Infinity, terrainLayer & ~ignorePlayerLayer))
        {
            if (hit.collider != null)
            {
               // Debug.Log("Terrain hit detected: " + hit.collider.gameObject.tag); // Debug log for raycast hit
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + GroundDist;
                transform.position = movePos;

                // Ensure we get the correct tag from the terrain collider
                string terrainTag = hit.collider.gameObject.tag;
                PlayFootsteps(terrainTag);
            }
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y);
        rb.velocity = moveDir * speed;

        if (rb.velocity.magnitude != 0f)
        {
            playerAnimator.SetBool("moving", true);
        }
        else
        {
            playerAnimator.SetBool("moving", false);
        }

        if (x != 0 && x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }
        else if (x != 0 && x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void PlayFootsteps(string terrainTag)
    {
        if (footstepSource != null)
        {
            if (rb.velocity.magnitude > 0f && Time.time > nextFootstepTime)
            {
                nextFootstepTime = Time.time + footstepDelay;

                AudioClip[] footstepClips;

                //Debug.Log("Playing footsteps for terrain tag: " + terrainTag); // Debug log for terrain tag

                switch (terrainTag)
                {
                    case "Grass":
                        footstepClips = grassFootstepClips.ToArray();
                        break;
                    case "Desert":
                        footstepClips = desertFootstepClips.ToArray();
                        break;
                    case "Snow":
                        footstepClips = snowFootstepClips.ToArray();
                        break;
                    case "Ruin":
                        footstepClips = ruinFootstepClips.ToArray();
                        break;
                    default:
                        footstepClips = grassFootstepClips.ToArray(); // Default to grass
                        break;
                }

                //Debug.Log("Selected footstep clip: " + footstepClips[Random.Range(0, footstepClips.Length)].name); // Debug log for selected clip
                if (footstepClips != null)
                {
                    footstepSource.clip = footstepClips[Random.Range(0, footstepClips.Length)];
                    footstepSource.Play();
                }
            }
        }
    }
        public void AttackAnimation()
    {
        playerAnimator.SetTrigger("Attack");
    }

    public void TakeSkillDamage(int damage, PublicEnums.ElementType elementType)
    {
        float multiplier = GetSkillMultiplier(elementType);
        damage = Mathf.FloorToInt(damage * multiplier);
        playerStats.health -= damage;
        Vector3 targetPosition = transform.position;
        if (damage < 0)
        {
            damage = damage * -1;
            DamageNumberManager.Instance.ShowNumbers(targetPosition, damage, Color.green);
        }
        else
        {
            DamageNumberManager.Instance.ShowNumbers(targetPosition, damage, Color.red);
        }

        if (playerStats.health <= 0)
        {
            playerAnimator.SetBool("death", true);
            GameManager.Instance.PlayerDeath(gameObject);
        }
        else
        {
            playerAnimator.SetTrigger("TakeDamage");
        }
    }

    public void TakeMeleeDamage(int damage, PublicEnums.WeaponType weaponType)
    {
        float multiplier = GetMeleeMultiplier(weaponType);
        damage = Mathf.FloorToInt(damage * multiplier);
        playerStats.health -= damage; 
        Vector3 targetPosition = transform.position;
        if (damage < 0)
        {
            damage = damage * -1;
            DamageNumberManager.Instance.ShowNumbers(targetPosition, damage, Color.green);
        }
        else
        {
            DamageNumberManager.Instance.ShowNumbers(targetPosition, damage, Color.red);
        }


        if (playerStats.health <= 0)
        {
            playerAnimator.SetBool("death", true);
            GameManager.Instance.PlayerDeath(gameObject);
        }
        else
        {
            playerAnimator.SetTrigger("TakeDamage");
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

    public void revive()
    {
        playerAnimator.SetFloat("SpeedMultiplier", -1f);
        playerAnimator.Play("witch_death", 0, 1f);
    }
   
}
