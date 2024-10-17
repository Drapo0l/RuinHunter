using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public Quest requiredQuest;
    public Teleporter Teleporter;
    public Quest completeThisQuest;

    private void OnTriggerEnter(Collider other)
    {
        if (completeThisQuest != null)
        {
            QuestManager.instance.CompleteQuest(completeThisQuest);
        }
        if ( other.CompareTag("Player"))
        {
            Quest quest = QuestManager.instance.completedQuests.Find(q => q == requiredQuest);
            if (quest != null)
            {                           
                 Teleporter.TeleportPlayer(other.transform); 
            }
            else if(requiredQuest == null)
            {
                Teleporter.TeleportPlayer(other.transform);
            }
        }
    }

}
