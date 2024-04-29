using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PawnSpawnPoint : MonoBehaviour
{
    public PawnSpawnPoint nextWayPoint;
    public GameObject AggAICon;
    public GameObject AggPawn;
    public GameObject GuaAICon;
    public GameObject GuaPawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void spawnRandomAI()
    {
        if(Random.Range(1, 3) == 1)
        {
            SpawnAggressiveAI(this);
        }
        else
        {
            SpawnGuardAI(this);
        }
    }

    // Spawns an aggressive AI at spawnPoint
    public void SpawnAggressiveAI(PawnSpawnPoint spawnPoint)
    {
        GameObject newAIObj = Instantiate(AggAICon, Vector3.zero, Quaternion.identity) as GameObject;

        GameObject newPawnObj = Instantiate(AggPawn, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

        Controller newController = newAIObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        newController.pawn = newPawn;
        newPawn.controller = newController;

        newAIObj.GetComponent<AIController>().waypoints[0] = spawnPoint.transform;
        newAIObj.GetComponent<AIController>().waypoints[1] = spawnPoint.nextWayPoint.transform;
        newAIObj.GetComponent<AIController>().waypoints[2] = spawnPoint.nextWayPoint.nextWayPoint.transform;
    }

    // Spawns an guard AI at spawnPoint
    public void SpawnGuardAI(PawnSpawnPoint spawnPoint)
    {
        GameObject newAIObj = Instantiate(GuaAICon, Vector3.zero, Quaternion.identity) as GameObject;

        GameObject newPawnObj = Instantiate(GuaPawn, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;

        Controller newController = newAIObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        newController.pawn = newPawn;
        newPawn.controller = newController;

        newAIObj.GetComponent<AIController>().waypoints[0] = spawnPoint.transform;
        newAIObj.GetComponent<AIController>().waypoints[1] = spawnPoint.nextWayPoint.transform;
        newAIObj.GetComponent<AIController>().waypoints[2] = spawnPoint.nextWayPoint.nextWayPoint.transform;
        newAIObj.GetComponent<AIController>().waypoints[3] = spawnPoint.nextWayPoint.nextWayPoint.nextWayPoint.transform;
    }
}
