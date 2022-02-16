using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.Find("StartPoint").transform;
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("Conso");

        for(int i = 0; i < tmp.Length; i++)
        {
            consoHUD.Add(tmp[i].GetComponent<CollectibleCube>());
        }

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
}
