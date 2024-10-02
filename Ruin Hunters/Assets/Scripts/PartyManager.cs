using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance; // singleton

    public List<GameObject> startingPlayerParty;
    private List<CharacterComponent> playerParty = new List<CharacterComponent>();
    public int maxPartySize = 4; //max num of members

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //keep it accross scenes
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
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
            CharacterComponent characterAttributes = new CharacterComponent(playerObj.GetComponent<playerController>().playerStats);

            if (characterAttributes.stats != null) 
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
            playerParty.Add(newMember.GetComponent<CharacterComponent>());
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
            playerParty.Add(memberToMember.GetComponent<CharacterComponent>());
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<CharacterComponent> GetCurrentPartyComponent()
    {
        return playerParty;
    }

    public List<GameObject> GetPlayeGameObj() 
    {
        return startingPlayerParty;
    }

    public bool IsPartyFull()
    {
        return playerParty.Count >= maxPartySize;
    }

    public bool IsCharacterInParty(CharacterComponent character) 
    {
        return playerParty.Contains(character);
    }


}
