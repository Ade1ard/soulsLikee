using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepsSound : MonoBehaviour
{
    public List<AudioClip> stepSpinds;

    AudioSource playerAudio;

    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }
    public void StepSound()
    {
        playerAudio.PlayOneShot(stepSpinds[Random.Range(0, stepSpinds.Count)]);
    }
}
