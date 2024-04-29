using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject playerControllerPrefab;
    public GameObject pawnPrefab;
    public Transform playerSpawnTransform;
    public List<PlayerController> players;
    public List<AIController> bots;
    public MapGenerator map;
    public GameObject TitleScreenState;
    public GameObject OptionsScreenState;
    public GameObject InitGameState;
    public GameObject GameplayState;
    public GameObject GameOverScreenState;
    public GameObject MenuCameraPrefab;
    public GameObject MenuCamera;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120; // My game was running at 600+ fps
    }

    // Start is called before the first frame update
    void Start()
    {
        DeactivateAllStates();
        ActivateTitleScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameplayState.activeSelf)
        {
            if(players[0].lives <= 0)
            {
                ActivateGameOver();
            }
            if(bots.Count > 1)
            {
                UpdateEnemyHealthUI();
            }
        }
    }

    // Spawns the player at a random spawn point in the map
    public void SpawnPlayerPawn()
    {
        // Debug.Log("Spawn " + players.Count);
        // Check player controller number of lives
        if(players[0].pawn == null && players[0].lives > 0){
            // if players[i]] has lives spawn them else switch their screen to game over

            playerSpawnTransform = map.level.GetComponentInChildren<PlayerSpawn>().transform;

            GameObject newPawnObj = Instantiate(pawnPrefab, playerSpawnTransform.position, Quaternion.identity) as GameObject;
            Debug.Log("Spawn Pawn");

            Controller newPlayerController = players[0].GetComponent<Controller>();

            Pawn newPlayerPawn = newPawnObj.GetComponent<Pawn>();

            newPawnObj.AddComponent<Noisemaker>();
            newPlayerPawn.noiseMaker = newPawnObj.GetComponent<Noisemaker>();
            newPlayerPawn.noiseMakerVolume = 3;

            newPlayerController.pawn = newPlayerPawn;
            newPlayerPawn.controller = newPlayerController;

            Camera cam = newPlayerPawn.GetComponentInChildren<Camera>();
        }
    }

    private void DeactivateAllStates()
    {
        TitleScreenState.SetActive(false);
        OptionsScreenState.SetActive(false);
        InitGameState.SetActive(false);
        GameplayState.SetActive(false);
        GameOverScreenState.SetActive(false);
    }

    public void ActivateTitleScreen()
    {
        DeactivateAllStates();
        TitleScreenState.SetActive(true);
        if(MenuCamera == null)
            MenuCamera = Instantiate(MenuCameraPrefab, instance.transform.position, Quaternion.identity) as GameObject;
        else{
            MenuCamera.SetActive(true);
        }
    }
    
    public void InitGame()
    {
        DeactivateAllStates();
        InitGameState.SetActive(true);

        players = new List<PlayerController>();

        map.GenerateMap();

        GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        
        bots = new List<AIController>();

        Invoke("SpawnAI", 0.1F);

        MenuCamera.SetActive(false);

        Invoke("SpawnPlayerPawn", 0.1F);

        Invoke("Gameplay", 0.1F);
    }

    public void Gameplay()
    {
        DeactivateAllStates();
        GameplayState.SetActive(true);
        Invoke("AddEnemyHealthUI", 0.15F);
    }

    // Activates the gameover state
    public void ActivateGameOver()
    {
        DeactivateAllStates();
        if(MenuCamera == null){
            MenuCamera = Instantiate(MenuCameraPrefab, instance.transform.position, Quaternion.identity) as GameObject;
        }
        else{
            MenuCamera.SetActive(true);
        }
        CleanUp();
        GameOverScreenState.SetActive(true);
    }

    public void CleanUp()
    {
        AIController[] conAIList = FindObjectsOfType<AIController>();
        for(int i = 0; i < conAIList.Length; i++){
            if(conAIList[i].gameObject != null)
                RemoveAI(conAIList[i]);
        }
        Controller[] conList = FindObjectsOfType<Controller>();
        for(int i = 0; i < conList.Length; i++){
            if(conList[i].gameObject != null)
                Destroy(conList[i].gameObject);
        }
        Pawn[] pawnList = FindObjectsOfType<Pawn>();
        for(int i = 0; i < pawnList.Length; i++){
            if(pawnList[i].gameObject != null)
                Destroy(pawnList[i].gameObject);
        }
    }

    // Activates the options menu
    public void ActivateOptionsMenu()
    {
        DeactivateAllStates();
        OptionsScreenState.SetActive(true);
    }

    // Quits the game
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    // Removes AI from relevant lists
    public void RemoveAI(AIController remove)
    {
        Debug.Log("Removing" + remove);
        for(int i = 0; i < bots.Count; i++){
            if(bots[i] == remove){
                bots.RemoveAt(i);
                Destroy(remove.gameObject);
                Canvas worldSpaceCanvas = GameplayState.GetComponentInChildren<Canvas>();
                RectTransform[] rectList = worldSpaceCanvas.GetComponentsInChildren<RectTransform>();
                Destroy(rectList[i + 1].gameObject);
                return;
            }
        }
    }

    // Adds the the enemy health to the world space canvas
    public void AddEnemyHealthUI()
    {
        Canvas worldSpaceCanvas = GameplayState.GetComponentInChildren<Canvas>();
        Debug.Log("Bots " + bots.Count);
        for(int i = 0; i < bots.Count; i++)
        {
            GameObject rect = new GameObject();
            rect.AddComponent<RectTransform>();
            rect.AddComponent<CanvasRenderer>();
            rect.AddComponent<TextMeshProUGUI>();
            TextMeshProUGUI newText = rect.GetComponent<TextMeshProUGUI>();
            if(bots[i].pawn != null){
                newText.text = newText.text + "Health: " + bots[i].pawn.GetComponent<Health>().currentHealth;
                rect.GetComponent<RectTransform>().transform.position = bots[i].pawn.transform.position;
            }
            newText.fontSize = 0.7F;
            newText.alignment = TextAlignmentOptions.Center;
            rect.transform.SetParent(worldSpaceCanvas.transform, false);
        }
        worldSpaceCanvas.GetComponent<RectTransform>().transform.position = new Vector3(0, 0, 0);
    }

    // Updates the UI on the world space canvas
    public void UpdateEnemyHealthUI()
    {
        if(GameplayState.activeSelf){
            Canvas worldSpaceCanvas = GameplayState.GetComponentInChildren<Canvas>();
            RectTransform[] rectList = worldSpaceCanvas.GetComponentsInChildren<RectTransform>();
            for(int i = 0; i < rectList.Length - 1; i++)
            {
                if(bots[i].pawn != null){
                    rectList[i + 1].transform.position = bots[i].pawn.transform.position + new Vector3(0, 2, 0);
                    TextMeshProUGUI healthText = rectList[i + 1].GetComponent<TextMeshProUGUI>();
                    healthText.text = "Health: " + bots[i].pawn.GetComponent<Health>().currentHealth;
                }
            }
            if(players[0].pawn != null){
                worldSpaceCanvas.GetComponent<RectTransform>().transform.rotation = players[0].pawn.transform.rotation;
            }
        }
    }

    public void SpawnAI()
    {
        foreach(PawnSpawnPoint p in map.AISpawns)
        {
            Debug.Log("Spawn random");
            p.spawnRandomAI();
        }
    }
}
