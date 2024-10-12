using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public void ShowNumbers(Vector3 target, int damage)
    {
        GameObject nums = Instantiate(floatingNumbers);  
        nums.transform.position = target;
        nums.transform.SetParent(null);
        nums.GetComponent<TextMeshPro>().text = damage.ToString();
        Destroy(nums, 1f);

    }

}
