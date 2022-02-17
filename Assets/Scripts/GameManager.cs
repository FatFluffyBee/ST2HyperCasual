using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public void Awake()
    {
        instance = this;
    }

    public GameObject playerBall;
    private Transform startPoint;
    private List<CollectibleCube> consoHUD = new List<CollectibleCube>();

    public RectTransform collectiblePivot;
    private Text collectibleText;
    private float collectibleCount = 0;



    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.Find("PlayerStartPoint").transform;
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("Conso");
        collectibleText = collectiblePivot.transform.GetChild(0).GetChild(0).GetComponent<Text>();

        for (int i = 0; i < tmp.Length; i++)
        {
            consoHUD.Add(tmp[i].GetComponent<CollectibleCube>());
        }

        collectiblePivot.localPosition = new Vector3(Screen.width/5f - Screen.width /2, Screen.height/15f - Screen.height/2, 0f);
        collectibleText.text = "0";

        Debug.Log(Screen.width + " " + Screen.height);
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
        Camera.main.GetComponent<CameraBehavior>().recentering = true;
        //feedback visuel et possible arret 0.5sec
        ResetConso();
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Instantiate(playerBall, startPoint.position, Quaternion.identity);
    }

    public void ResetConso()
    {
        foreach (CollectibleCube e in consoHUD)
        {
            e.Reset();
            //effacer sur le HUD
        }
    }

    public void AddCollectible()
    {
        collectibleCount++;
        collectibleText.text = collectibleCount.ToString();
    }
}
