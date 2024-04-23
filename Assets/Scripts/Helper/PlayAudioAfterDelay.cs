using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To use, attach to object and then call the public function of this script with parameters.
/// PRevents StopAllCoroutine() calls in animation scripts from interfering with audio playing properly.
/// </summary>
public class PlayAudioAfterDelay : MonoBehaviour
{
    public void DoDelayedAudio(AudioSource source, AudioClip clip, float volume, float secDelay)
    {
        StartCoroutine(DoAudio(source, clip, volume, secDelay));
    }

    private IEnumerator DoAudio(AudioSource source, AudioClip clip, float volume, float secDelay) // Unique sequence for firing the gun
    {
        yield return new WaitForSeconds(secDelay); // make sure click finishes right when you can fire again

        source.PlayOneShot(clip, volume);
    }
}
