using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance; // singleton

    public CinemachineVirtualCamera playerCamera;

    public List<GameObject> startingPlayerParty;
    private List<CharacterAttributes> playerParty = new List<CharacterAttributes>();
    public int maxPartySize = 4; //max num of members
    public int selectedPartyMember = 0;
    private int shownPartyMember; // holds the index of the party member that will be changed to get their position

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

        if (!GameManager.Instance.combat && !GameManager.Instance.leveling) 
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                shownPartyMember = selectedPartyMember;
                selectedPartyMember--;
                if (selectedPartyMember < 0) selectedPartyMember = playerParty.Count - 1;
                SwitchCharacter(selectedPartyMember);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                shownPartyMember = selectedPartyMember;
                selectedPartyMember++;
                if (selectedPartyMember >= playerParty.Count) selectedPartyMember = 0;
                SwitchCharacter(selectedPartyMember);
            }
        }
    }

    private void SwitchCharacter(int index)
    {
        foreach (GameObject characters in startingPlayerParty)
        {
            characters.SetActive(false);
        }
        startingPlayerParty[index].SetActive(true);
        startingPlayerParty[index].transform.position = startingPlayerParty[shownPartyMember].transform.position;
        playerCamera.Follow = startingPlayerParty[index].transform;
        playerCamera.LookAt = startingPlayerParty[index].transform;

    }

    private void Start()
    {
        //SwitchCharacter(0);
        StartCoroutine(DelayForPartySetup());
    }

    private IEnumerator DelayForPartySetup()
    {
        yield return new WaitForEndOfFrame(); // small delay

        InitializePlayerParty();
    }

    private void InitializePlayerParty()
    {
        foreach(GameObject playerObj in startingPlayerParty) 
        {
            CharacterAttributes characterAttributes = playerObj.GetComponent<playerController>().playerStats;

            if (characterAttributes != null) 
            {
                playerParty.Add(characterAttributes);
            }
        }
       
    }

    public bool AddPartyMember(GameObject newMember)
    {
        if(playerParty.Count >= maxPartySize) 
        {
            return false;        
        }
        else if (!startingPlayerParty.Contains(newMember))
        {
            startingPlayerParty.Add(newMember); // add char
            playerParty.Add(newMember.GetComponent<CharacterAttributes>());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemovePartyMember(GameObject memberToMember) 
    {
        if (startingPlayerParty.Contains(memberToMember))
        {
            startingPlayerParty.Add(memberToMember); // add char
            playerParty.Add(memberToMember.GetComponent<CharacterAttributes>());
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<CharacterAttributes> GetCurrentPartyComponent()
    {
        return new List<CharacterAttributes>(playerParty);
    }

    public List<GameObject> GetPlayeGameObj() 
    {
        return new List<GameObject>(startingPlayerParty);
    }

    public bool IsPartyFull()
    {
        return playerParty.Count >= maxPartySize;
    }

    public bool IsCharacterInParty(CharacterAttributes character) 
    {
        return playerParty.Contains(character);
    }

    public GameObject CurrentActiveCharacter()
    {
        return startingPlayerParty[selectedPartyMember];
    }

}
