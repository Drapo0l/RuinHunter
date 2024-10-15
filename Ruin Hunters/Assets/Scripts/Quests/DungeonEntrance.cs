using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public Quest requiredQuest;
    public BoxCollider BoxCollider;
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
                //set a text the say that you need to finish x quest use requiredQuest.name 
            }
        }
    }

}
