using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumberManager : MonoBehaviour
{
    public static FloatingNumberManager Instance;

    public GameObject floatingTextPrefab;

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

    public void ShowFloatingText(Transform target, int damageAmount, Camera cam)
    {
        //create the floating txt object
        GameObject floatingText = Instantiate(floatingTextPrefab);
        floatingText.SetActive(true);

        //set the text to the damage amount
        TextMeshPro damageText = floatingText.GetComponent<TextMeshPro>();
        damageText.text = damageAmount.ToString();

        //position it above the target
        floatingText.transform.position = target.position + new Vector3(0, 0f, 0);

        //destroy after x time
        Destroy(floatingText, 0.5f);
    }
}
