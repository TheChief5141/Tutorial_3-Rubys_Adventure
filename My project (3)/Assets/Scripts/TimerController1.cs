using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController1 : MonoBehaviour
{
    public Text timeCounter;
    public float startingTime;
    private bool timerGoing;
    private float currentTime;

    AudioSource audioSource;
    public AudioClip startTimerSound;
    public AudioClip audioClip;

    private RubyController playerController;

    public float displayTime = 4.0f;
    public GameObject ruby;
    public GameObject dialogBox;
    public Text jambiText;
    float timerDisplay;
    // Start is called before the first frame update
    void Start()
    {
        timeCounter.gameObject.SetActive(false);
        timerGoing = false;
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        playerController = ruby.GetComponent<RubyController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void BeginTimer()
    {
        timerGoing = true;
        timeCounter.gameObject.SetActive(true);
        timeCounter.text = "Time: " + startingTime.ToString();
        currentTime = startingTime;
        audioSource.clip = startTimerSound;
        audioSource.PlayOneShot(startTimerSound);
        Debug.Log("Timer Started");
        //StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        currentTime = 0;
        timerGoing = false;
        Debug.Log("Timer Ended");
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.PlayOneShot(audio);
    }

/*
    public IEnumerator UpdateTimer()
    {
        while (timerGoing && currentTime > 0)
        {
            currentTime -=  1.0f * Time.deltaTime;
            displayTimer(currentTime);
        }
        currentTime = 0;
        displayTimer(currentTime);
        timerGoing = false;
        yield return null;
    }
*/
    void displayTimer(float time)
    {
        Debug.Log("New to this");
        timerDisplay += 1;
        float minutes = Mathf.FloorToInt(time/60);
        float seconds = Mathf.FloorToInt(time%60);
        timeCounter.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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

        if (timerGoing)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                displayTimer(currentTime);
                if (playerController.score >= 6)
                {
                    EndTimer();
                }
            }
            else
            {
                playerController.loseFunction();
            }
        }

    }

    public void DisplayDialogue()
    {
        RubyController rubyControllerObject = ruby.GetComponent<RubyController>();

        Debug.Log("Saying Something");
        
        if (rubyControllerObject.score >= 6 && SceneManager.GetActiveScene().buildIndex == 0)
        {
            jambiText.text = "You did in the time limit! Great job! Here's your reward!";
            rubyControllerObject.updateCog(5);
            timerDisplay = displayTime;
            EndTimer();
            dialogBox.SetActive(true);
            PlayAudio(audioClip);
            rubyControllerObject.winFunction();
        }
        else if (rubyControllerObject.score >= 6 && SceneManager.GetActiveScene().buildIndex == 1)
        {
            jambiText.text = "You did it! Great job!";
            timerDisplay = displayTime;
            rubyControllerObject.updateCog(5);
            EndTimer();
            dialogBox.SetActive(true);
            PlayAudio(audioClip);
            rubyControllerObject.winFunction();
        }
        else
        {
            jambiText.text = "Help me fix all those robots in the limited time! Press X on the Ammo Crates to find more ammo! GO!";
            timerDisplay = displayTime;
            dialogBox.SetActive(true);
            PlayAudio(audioClip);
            BeginTimer();
        }
    }

}
