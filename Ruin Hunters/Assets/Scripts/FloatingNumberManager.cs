using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumberManager : MonoBehaviour
{
    public static FloatingNumberManager Instance;

    public GameObject floatingTextPrefab;
    public GameObject canvas;
    public Transform canvasTransform;

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
        floatingText.transform.SetParent(canvas.transform);

        //set the text to the damage amount
        TextMeshProUGUI damageText = floatingText.GetComponent<TextMeshProUGUI>();
        damageText.text = damageAmount.ToString();

        //position it above the target
        Vector3 worldPosition = target.position + new Vector3(0, 2f, 0);
        Vector3 screenPosition = cam.WorldToScreenPoint(worldPosition);

        //set the floating text position on canvas
        floatingText.transform.position = screenPosition;

        //destroy after x time
        Destroy(floatingText, 1.5f);
    }
}
