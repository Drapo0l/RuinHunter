using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public static DamageNumberManager Instance;

    [SerializeField] GameObject floatingNumbers;

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

    public void ShowNumbers(Vector3 target, int damage, Color color)
    {
        GameObject nums = Instantiate(floatingNumbers);  
        nums.transform.position = target;
        nums.transform.SetParent(null);
        nums.GetComponent<TextMeshPro>().text = damage.ToString();
        nums.GetComponent<TextMeshPro>().color = color;
        Destroy(nums, 1f);

    }

    public void ShowString(Vector3 target, string words, Color color)
    {
        GameObject nums = Instantiate(floatingNumbers);
        nums.transform.position = target;
        nums.transform.SetParent(null);
        nums.GetComponent<TextMeshPro>().text = words;
        nums.GetComponent<TextMeshPro>().color = color;
        Destroy(nums, 1f);

    }

}
