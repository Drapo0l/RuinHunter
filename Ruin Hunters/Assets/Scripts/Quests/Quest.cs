using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string description;
    public bool isCompleted;

    public string[] Objectives;
    public int experienceReward;
    public int goldReward;
}
