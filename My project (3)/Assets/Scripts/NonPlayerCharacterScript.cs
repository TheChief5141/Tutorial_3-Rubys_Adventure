using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NonPlayerCharacterScript : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject ruby;
    public GameObject dialogBox;
    public Text jambiText;
    float timerDisplay;
    AudioSource audioSource;
    public AudioClip audioClip;


    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.PlayOneShot(audio);
    }

    public void DisplayDialogue()
    {
        RubyController rubyControllerObject = ruby.GetComponent<RubyController>();
        
        if (rubyControllerObject.score >= 6 && SceneManager.GetActiveScene().buildIndex == 0)
        {
            jambiText.text = "You did it! Let's move to Stage 2!";
            timerDisplay = displayTime;
            dialogBox.SetActive(true);
            PlayAudio(audioClip);
            rubyControllerObject.winFunction();
        }
        else if (rubyControllerObject.score >= 6 && SceneManager.GetActiveScene().buildIndex == 1)
        {
            jambiText.text = "You did it! Great job!";
            timerDisplay = displayTime;
            dialogBox.SetActive(true);
            PlayAudio(audioClip);
            rubyControllerObject.winFunction();
        }
        else
        {
            jambiText.text = "Help me fix all those robots! Press X on the Ammo Crates to find more ammo!";
            timerDisplay = displayTime;
            dialogBox.SetActive(true);
            PlayAudio(audioClip);
        }
    }


}
