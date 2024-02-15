using System.Collections;
using System.IO;
using UnityEngine;

/// <summary>
/// Manages player data saved between scenes
/// Manages save data saved between sessions
/// </summary>
public class GameManager : MonoBehaviour
{
    // singleton instance
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            // setup SavePointManager as a singleton class
            if (_instance == null)
            {
                // create new game manager object
                GameObject newManager = new();
                newManager.AddComponent<GameManager>();
                DontDestroyOnLoad(newManager);
                _instance = newManager.GetComponent<GameManager>();

                // initialize and load save data
                _instance.SaveData = new();
                string path = Application.persistentDataPath + "/savedata.json";
                if (File.Exists(path))
                {
                    // read json file into data object
                    string json = File.ReadAllText(path);
                    _instance.SaveData = JsonUtility.FromJson<Data>(json);
                }
                else // default save file configuration (no save data found)
                {
                    _instance.SaveData.NumOfRuns = 0;
                }
            }
            return _instance;
        }
    }

    #region PLAYER DATA
    // Player Data (saved between scenes)
    [System.Serializable]
    public class Stats
    {
        // Health
        public int MaxHealth; // Base Health + Health Upgrades
        public int CurrHealth; // Base Health + Health Upgrades - Damage Taken
        // Armor
        public int Armor; // Armor Upgrades (temporary/consumable)
        // Damage
        public float BulletDamage; // Base Damage + Damage Upgrades
        // Fire Speed
        public float BulletCooldown; // Base Cooldown + Fire Speed Upgrades
        // Move Speed
        public float MoveSpeed; // Base MoveSpeed + MoveSpeed Upgrades
        public float DashDamage; // Basse Damage + MoveSpeed Upgrades
        // Light
        public float LightFOV; // Base FOV + Light Upgrades
        public float LightIntensity; // Base Intensity + Light Upgrades
    }
    private Stats _playerData;

    public Stats PlayerData 
    {
        get
        {
            // initialize if necessary and possible
            if (_playerData == null)
            {
                ResetPlayerStats();
            }

            return _playerData;
        }
        private set
        {
            _playerData = value;
        }
    }

    /// <summary>
    /// initializes base stats of player data (use before starting a new run)
    /// </summary>
    public void ResetPlayerStats()
    {
        if (GameObject.Find("Player"))
        {
            Instance._playerData = GameObject.Find("Player").GetComponent<PlayerStats>().GetBasePlayerData();
        }
        else
            Debug.LogError("Error: trying to access PlayerStats with no player in scene");
    }

    /// <summary>
    /// 
    /// </summary>
    public void ApplyUpgradeToStats(UpgradeController.UpgradeType upgradeType)
    {
        if (GameObject.Find("Player"))
        {
            Instance._playerData = GameObject.Find("Player").GetComponent<PlayerStats>().ApplyUpgrade(upgradeType);
        }
        else
            Debug.LogError("Error: trying to access PlayerStats with no player in scene");
    }
    #endregion

    #region SAVE DATA
    // save data (saved between sessions)
    public class Data
    {
        public int NumOfRuns; // adds one each time the player dies
    }
    public Data SaveData { get; private set; }

    private void OnApplicationQuit()
    {
        // save SavePointData to json file
        string json = JsonUtility.ToJson(SaveData);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    /// <summary>
    /// increments NumOfRuns counter in Data
    /// </summary>
    public void AddRun()
    {
        SaveData.NumOfRuns++;
    }
    #endregion

    private void Update()
    {
        // TODO: remove this in place of some sort of a pause menu with a quit button
        // quit application from any scene with escape
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }*/
    }
}