using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class WeknessManager : MonoBehaviour
{
    public GameObject weaknessBar;

    public List<Sprite> weaponWeaknessIcons;
    public List<Sprite> elementWeaknessIcons;
    
    private int index = 0;

    public void ClearWeakness()
    {
        for (int i = 0; i < weaknessBar.transform.childCount; i++)
        {
            weaknessBar.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = null;
        }
        index = 0;
    }
    
    public void SettupWeakness(List<Sprite> weaknesses, Vector3 position, Camera cam, EnemyAI enemy)
    {
        Vector3 screenPosition = cam.GetComponent<Camera>().WorldToScreenPoint(position);

        weaknessBar.transform.position = screenPosition + new Vector3(0, -100, 0);

        int i = 0;
        while(i < weaknesses.Count)
        {
            weaknessBar.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = weaknesses[i];
            i++;
        }

        int weaknessAmount = enemy.elementWeakness.Count + enemy.weaponsWeakness.Count - 1;
        if (weaknessAmount < 0)
            weaknessAmount = 0;
        i = weaknessBar.transform.childCount - 1;
        while (i > weaknessAmount)
        {
            weaknessBar.transform.GetChild(i).gameObject.SetActive(false);
            i--;
        }
        index = weaknesses.Count;
    }

    public Sprite ShowWeakness(PublicEnums.ElementType Weakness, EnemyAI enemy)
    {
        CharacterAttributes stats = enemy.enemyStats;
        CharacterAttributes ogStats = enemy.scriptableStats;
        if (Weakness == PublicEnums.ElementType.Fire)
        {
            if (!stats.weaknessIcons.Contains(elementWeaknessIcons[0]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = elementWeaknessIcons[0];
                index++;
                return elementWeaknessIcons[0];
            }            
        }
        else if (Weakness == PublicEnums.ElementType.Ice)
        {
            if (!stats.weaknessIcons.Contains(elementWeaknessIcons[1]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = elementWeaknessIcons[1];
                index++;
                return elementWeaknessIcons[1];
            }
        }
        else if (Weakness == PublicEnums.ElementType.Lightning)
        {
            if (!stats.weaknessIcons.Contains(elementWeaknessIcons[2]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = elementWeaknessIcons[2];
                index++;
                return elementWeaknessIcons[2];
            }
        }
        else if (Weakness == PublicEnums.ElementType.Earth)
        {
            if (!stats.weaknessIcons.Contains(elementWeaknessIcons[3]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = elementWeaknessIcons[3];
                index++;
                return elementWeaknessIcons[3];
            }
        }
        else if (Weakness == PublicEnums.ElementType.Water)
        {
            if (!stats.weaknessIcons.Contains(elementWeaknessIcons[4]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = elementWeaknessIcons[4];
                index++;
                return elementWeaknessIcons[4];
            }
        }
        
        
        return null;
        
    }
    
    public Sprite ShowWeakness(PublicEnums.WeaponType Weakness, EnemyAI enemy)
    {
        CharacterAttributes stats = enemy.enemyStats;
        if (Weakness == PublicEnums.WeaponType.Bow)        
        {
            if (!stats.weaknessIcons.Contains(weaponWeaknessIcons[0]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = weaponWeaknessIcons[0];
                index++;
                return weaponWeaknessIcons[0];
            }
        }
        else if (Weakness == PublicEnums.WeaponType.Lance)        
        {
            if (!stats.weaknessIcons.Contains(weaponWeaknessIcons[1]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = weaponWeaknessIcons[1];
                index++;
                return weaponWeaknessIcons[1];
            }
        }
        else if (Weakness == PublicEnums.WeaponType.Sword)        
        {
            if (!stats.weaknessIcons.Contains(weaponWeaknessIcons[2]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = weaponWeaknessIcons[2];
                index++;
                return weaponWeaknessIcons[2];
            }
        }
        else if (Weakness == PublicEnums.WeaponType.Dagger)        
        {
            if (!stats.weaknessIcons.Contains(weaponWeaknessIcons[3]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = weaponWeaknessIcons[3];
                index++;
                return weaponWeaknessIcons[3];
            }
        }
        else if (Weakness == PublicEnums.WeaponType.Axe)        
        {
            if (!stats.weaknessIcons.Contains(weaponWeaknessIcons[4]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = weaponWeaknessIcons[4];
                index++;
                return weaponWeaknessIcons[4];
            }
        }
        else if (Weakness == PublicEnums.WeaponType.Staff)        
        {
            if (!stats.weaknessIcons.Contains(weaponWeaknessIcons[5]))
            {
                weaknessBar.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = weaponWeaknessIcons[5];
                index++;
                return weaponWeaknessIcons[5];
            }
        }
         
        
        return null;
        
    }


}
