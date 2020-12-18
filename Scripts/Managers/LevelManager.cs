using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public MapGenerator mapM;
    public BattleManager battleM;
    public int layers = 10, width = 4;

    public NodeObject curNodeObj;
    public NodeObject beginNodeObj, endNodeObj;
    public Dictionary<Vector2, NodeObject> NodeObjDic = new Dictionary<Vector2, NodeObject>();

    public List<EnemyEncounter> enemy;
    public EnemyEncounter boss;

    public GameObject[] mapScreen;

    public bool ready = false;

    public void CallEnemyEncounter(NodeObject nodeObj)
    {
        curNodeObj = nodeObj;
        if (nodeObj == endNodeObj) StartCoroutine(battleM.Begin(boss));
        else
        {
            EnemyEncounter nextEncounter = enemy[(int)(Random.value * enemy.Count)];
            StartCoroutine(battleM.Begin(nextEncounter));
            enemy.Remove(nextEncounter);
        }
        ClearNodeAvailability();
        RefreshNodeDisplay();
    }

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        mapM = FindObjectOfType<MapGenerator>();
        battleM = FindObjectOfType<BattleManager>();
        ready = false;
        HideMap();
        mapM.GenerateNewMap(layers, width);
        while (!mapM.ready) yield return null;
        GetNodes();
        curNodeObj = beginNodeObj;
        RefreshAvailablePath();
        ready = true;
        ShowMap();
    }

    public void RefreshAvailablePath()
    {
        ClearNodeAvailability();
        if(curNodeObj == endNodeObj)
        {
            Debug.Log("You reached the end :)");
        }
        else
        {
            foreach(Node node in curNodeObj.node.nextNode)
            {
                NodeObject tmp = NodeObjDic[node.position];
                tmp.node.available = true;
                tmp.GetComponent<Button>().interactable = true;
            }
        }
        RefreshNodeDisplay();
    }
    private void RefreshNodeDisplay()
    {
        foreach (NodeObject nodeObj in NodeObjDic.Values)
        {
            nodeObj.RefreshColor(curNodeObj);
        }
    }

    private void ClearNodeAvailability()
    {
        beginNodeObj.node.available = false;
        endNodeObj.node.available = false;

        foreach(NodeObject nodeObj in NodeObjDic.Values)
        {
            nodeObj.node.available = false;
            nodeObj.GetComponent<Button>().interactable = false;
        }
    }

    private void GetNodes()
    {
        mapM.GetNodes(out beginNodeObj, out endNodeObj);  
    }


    public void ShowMap() {
        foreach(GameObject obj in mapScreen)
        {
            obj.SetActive(true);
        }
    }
    public void HideMap() {
        foreach (GameObject obj in mapScreen)
        {
            obj.SetActive(false);
        }
    }
}
