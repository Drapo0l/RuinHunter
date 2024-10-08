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

    public IEnumerator ShowFloatingText(Transform target, int damageAmount, Camera cam)
    {
        //create the floating txt object
        floatingTextPrefab.SetActive(true);
        

        //set the text to the damage amount
        TextMeshPro damageText = floatingTextPrefab.GetComponent<TextMeshPro>();
        damageText.text = damageAmount.ToString();

        //position it above the target
        floatingTextPrefab.transform.position = target.position + new Vector3(0, 0f, 0);

        //destroy after x time
        yield return new WaitForSeconds(0.5f);
        floatingTextPrefab.SetActive(false);
    }
}
