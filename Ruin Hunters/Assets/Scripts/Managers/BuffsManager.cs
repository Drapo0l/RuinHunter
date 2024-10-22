using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffsManager : MonoBehaviour
{
    public static BuffsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    public List<Sprite> buffs;
    public List<Sprite> deBuffs;

    public GameObject buffPrefab;

    Camera battleCamera;

    private List<GameObject> gameObjects = new List<GameObject>();


    private void PlaceBuff(GameObject buff, Vector3 worldPosition)
    {
        Vector3 screenPosition = battleCamera.ScreenToWorldPoint(worldPosition);

        buff.transform.position = screenPosition;

        buff.transform.position += new Vector3 (-100, 0, 0);

        if(gameObjects.Count != 0)
        {
            buff.transform.position += new Vector3(0, 60, 0);
        }

        gameObjects.Add(buff);
    }

    public void AttackUp(Vector3 pos)
    {
        GameObject buff = Instantiate(buffPrefab);
        buff.GetComponent<Image>().sprite = buffs[0];
        PlaceBuff(buff, pos);  
    }

    public void DefenseUp(Vector3 pos)
    {
        GameObject buff = Instantiate(buffPrefab);
        buff.GetComponent<Image>().sprite = buffs[1];
        PlaceBuff(buff, pos);
    }

    public void SkillUp(Vector3 pos)
    {
        GameObject buff = Instantiate(buffPrefab);
        buff.GetComponent<Image>().sprite = buffs[2];
        PlaceBuff(buff, pos);
    }



    public void AttackDown(Vector3 pos)
    {
        GameObject buff = Instantiate(buffPrefab);
        buff.GetComponent<Image>().sprite = deBuffs[0];
        PlaceBuff(buff, pos);
    }

    public void DefenseDown(Vector3 pos)
    {
        GameObject buff = Instantiate(buffPrefab);
        buff.GetComponent<Image>().sprite = deBuffs[1];
        PlaceBuff(buff, pos);
    }

    public void SkillDown(Vector3 pos)
    {
        GameObject buff = Instantiate(buffPrefab);
        buff.GetComponent<Image>().sprite = deBuffs[2];
        PlaceBuff(buff, pos);
    }
}
