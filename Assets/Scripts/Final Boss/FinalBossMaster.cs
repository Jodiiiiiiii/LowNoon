using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMaster : MonoBehaviour
{
    private bool _playerWon;            // Whether the player has won or lost
    private bool _timerActive;          // If we want our timer running
    private float _timerValue = 30f;    // How long the timer is
    private float _finalTimer;          // The value representing the timer

    [SerializeField] private CutsceneCharacterController _worm;
    [SerializeField] private CutsceneCharacterController _mole;
    [SerializeField] private ManualCameraController _camera;
    void Start()
    {
        _finalTimer = _timerValue;
        _timerActive = false;

        StartCoroutine(DoFinalBoss());
    }

    // Update is called once per frame
    void Update()
    {
        if(_timerActive && _finalTimer > 0)
        {
            _finalTimer -= Time.deltaTime;
        }
    }

    private IEnumerator DoFinalBoss()
    {
        // Play the worm entrance animation & wait for it to finish before swapping to idle. The camera is positioned behind the worm.
        _worm.SetCurrentAnimation("RoomEnter");
        yield return new WaitForSeconds(1.2083f);
        _worm.SetCurrentAnimation("Idle");

        yield return new WaitForSeconds(2f); // Buffer for player to view what they're being shown

        // Move the camera over to the mole and wait for it to get to the mole...
        Transform camPos1 = GameObject.Find("CameraPos1").transform;
        _camera.moveToGivenPos(camPos1.position, camPos1.rotation.eulerAngles, 2f);
        yield return new WaitForSeconds(4f);

        // ...who does a dramatic turnaround and laughs.
        // (To do this, play the step animation while rotating the mole with another animation)
        // Insta-Move the worm model over to a position near the mole.

        // Pull the camera out so that it's centered between them.
        // (Wait for the camera to pull out)

        // Tell FinalBossView to show the text,
        // "Use the mouse to fire at the stroke of low noon!"
        // Start a timer to count down "X" amount of time (so that it corresponds to the audio track)
        // Slowly fade out the instruction text
        // Play the music

        // Once there's a half second left/the final, whip the models around
        // The player has a half-second window to fire; if an input is received during that time, we whip the models around and cut to black early
        // Once the timer hits zero, we whip the models around, cut to black and play a gunshot anyway

        // Hold on it for a few seconds, during which time:
        // We set whoever is dead to defeated
        // Then fade out from black
        if (_playerWon) // If the worm won...
        {
            // Pull the camera over to the worm
            // Set GameManager BeenToMainMenu to false so we don't die when we get back to the town
            // Fade to credits after holding for a little
        }
        else // If the worm lost...
        {
            // Go to game over 
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
