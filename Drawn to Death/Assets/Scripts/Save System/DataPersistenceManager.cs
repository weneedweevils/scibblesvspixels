﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    [Header("Scene Variables and References")]
    [SerializeField] private Scene scene;
    [SerializeField] private GameObject cutsceneObject;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject tutorialObject;
    [SerializeField] private GameObject tutorialObjectOnEnemy;
    [SerializeField] private GameObject tutorialToggle;

    [Header("On Start Options")]
    [SerializeField] private bool newGame;
    [SerializeField] private bool loadGame;
    [SerializeField] private bool saveGame;
    public bool skipCutsceneOnLoad = true;
    public bool skipTutorialOnLoad = true;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public UnityEvent SkipCutscene; // event that is fired when user skips cutscene

    GameObject skipCutsceneObject;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene");
        }
        instance = this;


        skipCutsceneObject = GameObject.FindGameObjectWithTag("SkipCutsceneObject");
        if (skipCutsceneObject != null)
        {
            SkipCutscene.AddListener(skipCutsceneObject.GetComponent<SkipCutscene>().OnDisable);
        }
    }

    private void Start()
    {
        /*  Paths:
        *   Windows Store Apps: Application.persistentDataPath points to C:\Users\<user>\AppData\LocalLow\<company name>.
        *   Windows Editor and Standalone Player: Application.persistentDataPath usually points to %userprofile%\AppData\LocalLow\<companyname>\<productname>. It is resolved by SHGetKnownFolderPath with FOLDERID_LocalAppDataLow, or SHGetFolderPathW with CSIDL_LOCAL_APPDATA if the former is not available.
        *   WebGL: Application.persistentDataPath points to /idbfs/<md5 hash of data path> where the data path is the URL stripped of everything including and after the last '/' before any '?' components.
        *   Linux: Application.persistentDataPath points to $XDG_CONFIG_HOME/unity3d or $HOME/.config/unity3d.
        *   iOS: Application.persistentDataPath points to /var/mobile/Containers/Data/Application/<guid>/Documents.
        *   tvOS: Application.persistentDataPath is not supported and returns an empty string.
        *   Android: Application.persistentDataPath points to /storage/emulated/<userid>/Android/data/<packagename>/files on most devices (some older phones might point to location on SD card if present), the path is resolved using android.content.Context.getExternalFilesDir.
        *   Mac: Application.persistentDataPath points to the user Library folder. (This folder is often hidden.) In recent Unity releases user data is written into ~/Library/Application Support/company name/product name. Older versions of Unity wrote into the ~/Library/Caches folder, or ~/Library/Application Support/unity.company name.product name. These folders are all searched for by Unity. The application finds and uses the oldest folder with the required data on your system.
        */
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        if (newGame) { NewGame(); }
        if (loadGame) { LoadGame(); }
        if (saveGame) { SaveGame(); }

       
    }

    [ContextMenu("New Game")]
    public void NewGame()
    {
        this.gameData = new GameData(tutorialToggle.GetComponent<Toggle>().isOn);
        dataHandler.Save(gameData);
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        //Load saved data from a file w/ data handler
        this.gameData = dataHandler.Load();

        //Start a new game if no data could be loaded
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        } 
        else
        {
            //Disable opening cutscene
            if (gameData.skipCutscene && skipCutsceneOnLoad)
            {
                if (cutsceneObject != null) { cutsceneObject.SetActive(false); }
                if (cutsceneObject != null) { hud.SetActive(true); }
                SkipCutscene.Invoke();
                
            }

            // Disable tutorial
            if (gameData.skipTutorial && skipTutorialOnLoad)
            {
                if (tutorialObject != null) { tutorialObject.SetActive(false); }
                if (tutorialObjectOnEnemy != null) { tutorialObjectOnEnemy.SetActive(false); }
            }

            //pass data to other scripts so they can update it
            foreach (IDataPersistence obj in dataPersistenceObjects)
            {
                obj.LoadData(gameData);
            }
        }
    }

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        //pass data to other scripts so they can update it
        gameData.skipCutscene = true;
        gameData.scene = scene;

        Debug.Log("----- Collecting Data -----");
        
        foreach (IDataPersistence obj in dataPersistenceObjects)
        {
            obj.SaveData(ref gameData);
        }

        Debug.Log("----- Saving Data -----");

        //save the data to a file using the data handler
        dataHandler.Save(gameData);

        Debug.Log("----- Data Saved -----");

    }

    public GameData GetGameData()
    {
        return gameData;
    }

    public void UpdateGame()
    {
        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPerObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPerObjects);
    }

  
}
