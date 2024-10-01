using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance; // singleton

    public List<CharacterAttributes> playerParty = new List<CharacterAttributes>();
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

    public bool AddPartyMember(CharacterAttributes newMember)
    {
        if(playerParty.Count >= maxPartySize) 
        {
            return false;        
        }
        else if (!playerParty.Contains(newMember))
        {
            playerParty.Add(newMember); // add char
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemovePartyMember(CharacterAttributes memberToMember) 
    {
        if (playerParty.Contains(memberToMember))
        {
            playerParty.Remove(memberToMember);
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<CharacterAttributes> GetCurrentParty()
    {
        return playerParty;
    }

    public bool IsPartyFull()
    {
        return playerParty.Count >= maxPartySize;
    }

    public bool IsCharacterInParty(CharacterAttributes character) 
    {
        return playerParty.Contains(character);
    }


}
