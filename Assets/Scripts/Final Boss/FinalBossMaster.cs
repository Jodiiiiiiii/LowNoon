using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossMaster : MonoBehaviour
{
    private bool _playerWon;            // Whether the player has won or lost
    private bool _timerActive;          // If we want our timer running
    private float _timerValue = 24.75f;    // How long the timer is
    public float _finalTimer;          // The value representing the timer

    [SerializeField] private CutsceneCharacterController _worm;
    [SerializeField] private CutsceneCharacterController _mole;
    [SerializeField] private ManualCameraController _camera;

    [SerializeField] private Animator _clockMin;
    [SerializeField] private Animator _clockHour;

    private AudioSource _source;
    [SerializeField] private AudioClip _finalTrack;
    [SerializeField] private AudioClip _playerShot;
    [SerializeField] private AudioClip _enemyShot;
    [SerializeField] private AudioClip _laugh1;
    [SerializeField] private AudioClip _laugh2;
    [SerializeField] private AudioClip _defeat;
    void Start()
    {
        _finalTimer = _timerValue;
        _timerActive = false;
        _source = GetComponent<AudioSource>();

        StartCoroutine(DoFinalBoss());
    }

    // TODO
    // All sounds - add correct GameManager volume

    // 11 - Fade to credits

    void Update()
    {
        if (_timerActive && _finalTimer > 0) // Counting the timer down
        {
            _finalTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _finalTimer > 0 && _finalTimer <= .55f && !GameManager.IsPaused) // Checking for if the player satisfies the win condition
        {
            _playerWon = true;      // The player has won!
            _timerActive = false;
        }
    }

    private IEnumerator DoFinalBoss()
    {
        // 1 - Play the worm entrance animation & wait for it to finish before swapping to idle. The camera is positioned behind the worm.
        _worm.SetCurrentAnimation("RoomEnter");
        yield return new WaitForSeconds(1.2083f);
        _worm.SetCurrentAnimation("Idle");

        yield return new WaitForSeconds(2f); // Buffer for player to view what they're being shown

        // 2 - Move the camera over to the mole and wait for it to get to the mole...
        Transform camPos1 = GameObject.Find("CameraPos1").transform;
        _camera.moveToGivenPos(camPos1.position, camPos1.rotation.eulerAngles, 2f);
        yield return new WaitForSeconds(4f);

        // 4 - Move the worm model over to a position near the mole (use SetPositionAndRotation because otherwise the worm's light makes things very obvious)
        Transform wormPos1 = GameObject.Find("WormPos1").transform;
        _worm.TravelToPositionAndRotation(wormPos1.position, wormPos1.rotation.eulerAngles, 3f);

        // 3 - TODO
        // ...who does a dramatic turnaround and laughs.
        // (To do this, play the step animation while rotating the mole with another animation)
        _mole.SetCurrentAnimation("Move");
        _mole.TravelToPositionAndRotation(_mole.transform.position, new Vector3(0, 0, 0), 1.7f);

        
        yield return new WaitForSeconds(4f); // Wait for the mole to turn around
        _mole.SetCurrentAnimation("Move");  // Set mole back to idle
        yield return new WaitForSeconds(1f);
        _source.PlayOneShot(_laugh1, .5f);
        yield return new WaitForSeconds(3f);

        // 5 - Pull the camera out so that it's centered between them (and give it a little time to do so)
        Transform camPos2 = GameObject.Find("CameraPos2").transform;
        _camera.moveToGivenPos(camPos2.position, camPos2.rotation.eulerAngles, 1.5f);
        yield return new WaitForSeconds(5f);

        // 6 - Tell FinalBossView to show the text, "Use the mouse to fire at the stroke of low noon!"
        ViewManager.GetView<FinalBossView>().ShowText();
        // Start the music and the timer at the same time
        _source.PlayOneShot(_finalTrack, .5f); //  GameManager.Instance.GetMusicVolume()
        _timerActive = true;
        _clockHour.Play("HourTurn", 0, 0);
        _clockMin.Play("MinuteTurn", 0, 0);

        // 7 - Between the beginning of the music and the first stroke of the bell, turn the characters around so they face away from each other
        _worm.TravelToPositionAndRotation(wormPos1.position, new Vector3(0, 90, 0), 2f);
        _mole.SetCurrentAnimation("Move");
        _mole.TravelToPositionAndRotation(_mole.transform.position, new Vector3(0, 180, 0), 2f);
        // Wait so that we don't start the movement until the right time in the song
        yield return new WaitForSeconds(4f);

        // 8 - Start the camera moving and the duelers walking
        _camera.lerpToFOV(75, 8.1f);
        Transform camPos3 = GameObject.Find("CameraPos3").transform;
        _camera.moveToGivenPos(camPos3.position, camPos2.rotation.eulerAngles, 6.1f);

        // Set both of them moving and playing their walk animations at an appropriate speed
        Transform wormPos2 = GameObject.Find("WormPos2").transform;
        _worm.TravelToPositionAndRotation(wormPos2.position, wormPos2.rotation.eulerAngles, 12f);
        _worm.SetCurrentAnimation("Move");

        // Mole version of that
        Transform molePos1 = GameObject.Find("MolePos1").transform;
        _mole.TravelToPositionAndRotation(molePos1.position, _mole.transform.rotation.eulerAngles, 12f);


        // Slowly fade out the instruction text
        ViewManager.GetView<FinalBossView>().FadeText();

        // 8.5 - Whip the duelers around after a certain amount of time has passed
        yield return new WaitUntil(() => (_finalTimer <= 12.6));
        _worm.SetCurrentAnimation("Move");
        _worm.TravelToPositionAndRotation(_worm.transform.position, new Vector3(0, -90, 0), 1f);
        _mole.TravelToPositionAndRotation(_mole.transform.position, new Vector3(0, 0, 0), 1f);
        yield return new WaitForSeconds(4f);
        _mole.SetCurrentAnimation("Move");

        // 9 - Wait until either the player has correctly fired, or the time has passed for them to do so
        yield return new WaitUntil(() => (_playerWon || _finalTimer <= 0));
        // Cut to black and play a gunshot
        ViewManager.GetView<FinalBossView>().ShowPanel();
        
        if (_playerWon)
        {
            _source.PlayOneShot(_playerShot, .5f);
        }
        else
        {
            _source.PlayOneShot(_enemyShot, .5f);
        }

        // 10 - Hold on it for a few seconds, then fade out
        yield return new WaitForSeconds(4f);
        ViewManager.GetView<FinalBossView>().FadePanel();
        yield return new WaitForSeconds(2f);
        if (_playerWon) // If the worm won...
        {
            // 11 - Play the mole defeat anim
            _mole.SetCurrentAnimation("Defeat");
            _source.PlayOneShot(_defeat, .5f);
            // Pull the camera over to the worm
            // -168.914
            yield return new WaitForSeconds(2f);
            Transform camPos4 = GameObject.Find("CameraPos4").transform;
            _camera.moveToGivenPos(camPos4.position, camPos4.rotation.eulerAngles, 2f);

            GameManager.Instance.IsMainMenuLoaded = false; // Set GameManager BeenToMainMenu to false so we don't die when we get back to the town
            // Fade to credits after holding for a little
            yield return new WaitForSeconds(5f);
            ViewManager.GetView<FinalBossView>().LeaveSceneTransition();
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("xx_Credits");
            
        }
        else // If the worm lost...
        {
            // Play worm defeat anim
            _worm.SetCurrentAnimation("Death");
            yield return new WaitForSeconds(1f);
            _source.PlayOneShot(_laugh2, .5f);
            yield return new WaitForSeconds(1f);
            // 12 - Go to game over 
            ViewManager.Show<GameOverView>(false);
        }
        
        yield return null;
    }
    

    // This script
        // Coroutine
        // Timer, keeping track of inputs

    // CutsceneCharacterController
        // SetToPosition
        // TravelToPosition
        // SetToRotation
        // TravelToRotation
        // SetCurrentAnimation

    // FinalBossView
        // ShowPanel
        // FadePanelIn
        // FadePanelOut
        // GameOver

    // We can just add all these camera movements to the ManualCameraController
}
