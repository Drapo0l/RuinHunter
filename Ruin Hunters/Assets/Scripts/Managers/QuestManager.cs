using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests;
    public List<Quest> completedQuests;

    public GameObject questParent;
    public GameObject questPanel;
    private List<GameObject> questPanels = new List<GameObject>();

    public static QuestManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateQuestDisplay(); 
    }

    public void UpdateQuestDisplay()
    {
        if (questPanels != null)
        {
            foreach (GameObject panel in questPanels)
            {
                Destroy(panel);
            }
            questPanels.Clear();
        }

        float spacing = 0;
        questParent.SetActive(true);

        foreach (Quest quest in activeQuests)
        {
            GameObject questCopy = Instantiate(questPanel);
            questCopy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.questName;
            questCopy.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = quest.description;
            questCopy.transform.SetParent(questParent.transform);
            questCopy.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0, spacing);
            spacing -= 120;
            questPanels.Add(questCopy);
        }

    }

    public void CompleteQuest(Quest quest)
    {
        if(activeQuests.Contains(quest)) 
        {
            
            quest.isCompleted = true;
            activeQuests.Remove(quest);
            completedQuests.Add(quest);

            GrantQuestRewards(quest);

            UpdateQuestDisplay();
        }
    }

    private void GrantQuestRewards(Quest quest)
    {
        InventoryManager.instance.Gold += quest.goldReward;
        List<CharacterAttributes> playerParty = PartyManager.Instance.GetCurrentPartyComponent();
        foreach (CharacterAttributes character in playerParty) 
        {
            character.currentXP += quest.experienceReward;
        }
    }

    public void AddQuest(Quest newQuest)
    {
        if(!activeQuests.Contains(newQuest))
        {
            activeQuests.Add(newQuest);

            UpdateQuestDisplay();
        }
    }
}
