using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Search;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public Quest requiredQuest;
    public Quest completeQuest;
    public Teleporter Teleporter;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player"))
        {
            Quest quest = QuestManager.instance.completedQuests.Find(q => q == requiredQuest);
            if (quest != null)
            {
                if (quest.isCompleted)
                {
                    Teleporter.TeleportPlayer(other.transform);
                }
                else
                {                    
                    //set a text the say that you need to finish x quest use requiredQuest.name 
                }
            }
            else if(requiredQuest == null)
            {
                if (completeQuest != null) 
                {
                    QuestManager.instance.CompleteQuest(completeQuest);
                }
                Teleporter.TeleportPlayer(other.transform);
            }
        }
    }

}
