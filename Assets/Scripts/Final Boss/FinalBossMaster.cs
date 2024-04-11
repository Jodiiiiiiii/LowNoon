using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMaster : MonoBehaviour
{
    private bool _playerWon;            // Whether the player has won or lost
    private bool _timerActive;          // If we want our timer running
    private float _timerValue = 24.75f;    // How long the timer is
    public float _finalTimer;          // The value representing the timer

    [SerializeField] private CutsceneCharacterController _worm;
    [SerializeField] private CutsceneCharacterController _mole;
    [SerializeField] private ManualCameraController _camera;

    private AudioSource _source;
    [SerializeField] private AudioClip _finalTrack;
    [SerializeField] private AudioClip _playerShot;
    [SerializeField] private AudioClip _enemyShot;
    void Start()
    {
        _finalTimer = _timerValue;
        _timerActive = false;
        _source = GetComponent<AudioSource>();

        StartCoroutine(DoFinalBoss());
    }

    // TODO
    // 3 - Need mole model and animations
    // 6 - Add correct GameManager volume
    // 7 - Turn mole around
    // 8 - All
    // 9 - Add correct enemy gunshot, add correct volumes
    // 10 - All
    // 11 - All
    // 12 - All

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

        // 3 - TODO
        // ...who does a dramatic turnaround and laughs.
        // (To do this, play the step animation while rotating the mole with another animation)

        // 4 - Move the worm model over to a position near the mole (use SetPositionAndRotation because otherwise the worm's light makes things very obvious)
        Transform wormPos1 = GameObject.Find("WormPos1").transform;
        _worm.TravelToPositionAndRotation(wormPos1.position, wormPos1.rotation.eulerAngles, 3f);

        yield return new WaitForSeconds(4f); // Wait for the mole to turn around & laugh and the worm to arrive

        // 5 - Pull the camera out so that it's centered between them (and give it a little time to do so)
        Transform camPos2 = GameObject.Find("CameraPos2").transform;
        _camera.moveToGivenPos(camPos2.position, camPos2.rotation.eulerAngles, 1.5f);
        yield return new WaitForSeconds(5f);

        // 6 - Tell FinalBossView to show the text, "Use the mouse to fire at the stroke of low noon!"
        ViewManager.GetView<FinalBossView>().ShowText();
        // Start the music and the timer at the same time
        _source.PlayOneShot(_finalTrack, .5f); //  GameManager.Instance.GetMusicVolume()
        _timerActive = true;

        // 7 - TODO: Between the beginning of the music and the first stroke of the bell, turn the characters around so they face away from each other
        _worm.TravelToPositionAndRotation(wormPos1.position, new Vector3(0, 90, 0), 2f);

        // Wait so that we don't start the movement until the right time in the song
        yield return new WaitForSeconds(4f);

        // 8 - Start the camera moving and the duelers walking
        // TODO - Camera
        // Set both of them moving and playing their walk animations at an appropriate speed
        //Transform wormPos2 = GameObject.Find("WormPos2").transform;
        //_worm.TravelToPositionAndRotation(wormPos2.position, wormPos2.rotation.eulerAngles, 12f);
        //_worm.SetCurrentAnimation("Move");

        //TODO - Mole version of that
        // Slowly fade out the instruction text
        ViewManager.GetView<FinalBossView>().FadeText();

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

        // 10 - Hold on it for a few seconds, during which time re rotate the characters to face each other, then fade out
        if (_playerWon) // If the worm won...
        {
            // 11 - Play the mole defeat anim
            // Pull the camera over to the worm
            // Set GameManager BeenToMainMenu to false so we don't die when we get back to the town
            // Fade to credits after holding for a little
        }
        else // If the worm lost...
        {
            // Play worm defeat anim
            // 12 - Go to game over 
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
