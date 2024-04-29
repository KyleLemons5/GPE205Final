using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject levelPrefab;
    public float levelOffsetX = 0f;
    public float levelOffsetZ = 60f;
    public PawnSpawnPoint[] AISpawns;
    public List<PlayerSpawn> playerSpawns;
    public GameObject level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Generates the map
    public void GenerateMap()
    {
        Vector3 pos = new Vector3(levelOffsetX, 0, levelOffsetZ);

        level = Instantiate(levelPrefab, pos, Quaternion.identity);

        AISpawns = level.GetComponentsInChildren<PawnSpawnPoint>();
    }
}
