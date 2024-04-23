using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionObject : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private List<string> _levels;

    [Header("Boss Level")]
    [SerializeField] private string _bossLevel;
    [SerializeField, Tooltip("number of rooms that must be cleared before able to fight boss")] private int _roomsBeforeBoss = 5;

    [Header("Tracking Number of Runs")]
    [SerializeField, Tooltip("Used by the exit patch in the hub to determine if to increment number of runs tracker")] private bool _isStartPatch = false;

    private string _sceneToLoad;

    public delegate void OnSceneTransition();
    public static event OnSceneTransition onSceneTransition; // Event for when the bullet fires

    // Start is called before the first frame update
    void Start()
    {
        // check if it is time for the boss
        if (GameManager.Instance.PlayerData.NumRooms >= _roomsBeforeBoss)
            _sceneToLoad = _bossLevel;
        else // determine random scene to load
        {
            // increment number of rooms traversed
            GameManager.Instance.PlayerData.NumRooms++;

            _sceneToLoad = "null";
            while(_sceneToLoad == "null")
            {
                int rand = Random.Range(0, 5);
                if(rand == 0) // 0 - Combat small
                {
                    if (!GameManager.Instance.PlayerData.HadSmallCombatRoom)
                    {  // already done
                        _sceneToLoad = _levels[0];
                        GameManager.Instance.PlayerData.HadSmallCombatRoom = true;
                    }
                }
                else if(rand == 1) // 1 - Minecarts 
                {
                    if (!GameManager.Instance.PlayerData.HadMinecartRoom)
                    {  // already done
                        _sceneToLoad = _levels[1];
                        GameManager.Instance.PlayerData.HadMinecartRoom = true;
                    }
                }
                else if(rand == 2) // 2 - Barrels
                {
                    if (!GameManager.Instance.PlayerData.HadBarrelRoom) {  // already done
                        _sceneToLoad = _levels[2];
                        GameManager.Instance.PlayerData.HadBarrelRoom = true;
                    }
                }
                else if(rand == 3) // 3 - Combat large
                {
                    if (!GameManager.Instance.PlayerData.HadLargeCombatRoom)
                    {  // already done
                        _sceneToLoad = _levels[3];
                        GameManager.Instance.PlayerData.HadLargeCombatRoom = true;
                    }
                }
                else // 4 - Maze
                {
                    if (!GameManager.Instance.PlayerData.HadMazeRoom)
                    {  // already done
                        _sceneToLoad = _levels[4];
                        GameManager.Instance.PlayerData.HadMazeRoom = true;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player")
        {
            StartCoroutine(DoSceneChange(other.gameObject));
        }
    }

    private IEnumerator DoSceneChange(GameObject player)
    {
        // Invoke the scene transtion event, which
        //      Sets velocity to 0 and disables the player controller
        //      Triggers the burrow down animation  
        //      Pulls down the UI curtain
        onSceneTransition?.Invoke();

        // Wait until the player animation and UI scene transition animation are done
        yield return new WaitForSeconds(player.GetComponent<PlayerAnimator>().BurrowDownDuration + 1f);
        
        // on scene load, increment number of runs if necessary
        if (_isStartPatch)
            GameManager.Instance.AddRun();

        SceneManager.LoadScene(_sceneToLoad);
        yield return null;
    }
}
