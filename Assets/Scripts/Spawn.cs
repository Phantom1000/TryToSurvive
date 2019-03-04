using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    public GameObject enemy;
    [SerializeField]
    private Transform spawnPositions;
    [SerializeField]
    private GameObject prefMedhit;
    [SerializeField]
    private GameObject prefAmmopack;
    private Transform[] positions;
    public bool[] FillPoint;
    private float timer;
    private float timeSpawn;
    private int countPoints = 0;
    private void Start()
    {
        timer = Time.time + 1f;
        timeSpawn = Time.time + 3f;
        FillPoint = new bool[spawnPositions.childCount];
        positions = new Transform[spawnPositions.childCount];
        int i = 0;
        foreach (Transform t in spawnPositions)
        {
            FillPoint[i] = false;
            positions[i++] = t;
        }
    }
    private void Update()
    {
        if (Time.time > timer)
        {
            timer = Time.time + 1f;
            int chance = Random.Range(0, 2);
            if (chance == 0)
            {
                GameObject medhit = Instantiate(prefMedhit, new Vector3(Random.Range(0, 250), 102, Random.Range(0, 250)), Quaternion.identity);
            }
            else
            {
                GameObject ammopack = Instantiate(prefAmmopack, new Vector3(Random.Range(0, 250), 102, Random.Range(0, 250)), Quaternion.identity);
            }
        }
        Create();
    }

    public void Create()
    {
        if (Time.time > timeSpawn)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if (!FillPoint[i])
                {
                    GameObject creep = Instantiate(enemy, positions[i].position, Quaternion.identity);
                    creep.GetComponent<EnemyController>().Path = positions[i];
                    creep.GetComponent<EnemyController>().SetPaths();
                    creep.GetComponent<EnemyController>().Number = i;
                    FillPoint[i] = true;
                    break;
                }
            }
            /*if (!FillPoint[countPoints])
            {
                 GameObject creep = Instantiate(enemy, positions[countPoints].position, Quaternion.identity);
                 creep.GetComponent<EnemyController>().Path = positions[countPoints];
                 creep.GetComponent<EnemyController>().SetPaths();
                 creep.GetComponent<EnemyController>().Number = countPoints;
                 FillPoint[countPoints] = true;
             }*/
            countPoints = (countPoints + 1) % positions.Length;
            timeSpawn = Time.time + 3f;
        }
    }
}
