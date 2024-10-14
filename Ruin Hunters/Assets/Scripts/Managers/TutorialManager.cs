using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

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

    public bool isTutorialActive = true;
    private int currentStep = 0;

   

    public void ShowNextStep()
    {
        currentStep++;
        ShowTutorialStep(currentStep);
    }

    private void EndTutorial()
    {
        isTutorialActive = false; // Signal the end of the tutorial
    }

    public void ShowTutorialStep(int step)
    {
        switch(step)
        {
            case 1:

                break;
            case 2:

                break;
        }
    }
}
