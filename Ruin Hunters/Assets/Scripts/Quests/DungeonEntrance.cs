using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public Quest requiredQuest;
    public Teleporter Teleporter;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player"))
        {
            Quest quest = QuestManager.instance.activeQuests.Find(q => q == requiredQuest);
            if (quest != null && quest.isCompleted) 
            {
                Teleporter.gameObject.SetActive(true);
            }
            else
            {
                Teleporter.gameObject.SetActive(false);
                //add your code to show textbox with "finish quest.name"
            }
        }
    }

}
