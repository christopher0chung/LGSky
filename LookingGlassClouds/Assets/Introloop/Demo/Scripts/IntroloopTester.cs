﻿/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using E7.Introloop;

public class IntroloopTester : MonoBehaviour
{
    public IntroloopAudio[] allIntroloopAudio;

    public void PlayIntroloop()
    {
        IntroloopPlayer.Instance.Play(allIntroloopAudio[0]);
    }

    public IntroloopTestSwitches testSwitches;

    /// <summary>
    /// Press "Remember and stop" button to make the next play use the time when it stopped.
    /// </summary>
    private float stopTime;

    public void PlayIndex(int arrayMember)
    {
        IntroloopPlayer.Instance.Play(allIntroloopAudio[arrayMember], testSwitches.useFade ? testSwitches.fadeTime : 0, stopTime);
        UpdateSongInformation(arrayMember);
    }

    public void Preload(int arrayMember)
    {
        IntroloopPlayer.Instance.Preload(allIntroloopAudio[arrayMember]);
    }

    public Text songNameText;
    public Text genreText;
    private void UpdateSongInformation(int arrayMember)
    {
        float length = allIntroloopAudio[arrayMember].ClipLength;
        string songName = "", genre = "";
        switch (arrayMember)
        {
            case 0:
                {
                    songName = "5argon - Assault";
                    genre = "Psy Techno";
                    break;
                }
            case 1:
                {
                    songName = "5argon - Assault (End)";
                    genre = "Psy Techno";
                    break;
                }
            case 2:
                {
                    songName = "5argon - Compete";
                    genre = "Latin Beats";
                    break;
                }
            case 3:
                {
                    songName = "5argon - Compete (End)";
                    genre = "Latin Beats";
                    break;
                }
            case 4:
                {
                    songName = "5argon - Otter's Celebration (Prepare)";
                    genre = "Funk";
                    break;
                }
            case 5:
                {
                    songName = "5argon - Otter's Celebration";
                    genre = "Funk";
                    break;
                }
            case 6:
                {
                    songName = "5argon - Maid Battle (RPG Arrange)";
                    genre = "Jazz Fusion";
                    break;
                }
            case 7:
                {
                    songName = "5argon - Assault (Pitch 0.4)";
                    genre = "Psy Techno";
                    break;
                }
        }
        TimeSpan lengthTime = TimeSpan.FromSeconds(length);
        songName += " [" + string.Format("{0:D1}:{1:D2}", lengthTime.Minutes, lengthTime.Seconds) + "]";
        songNameText.text = songName;
        genreText.text = genre;
    }

    public void DSPPause()
    {
        AudioListener.pause = true;
    }

    public void DSPResume()
    {
        AudioListener.pause = false;
    }

    public void Pause()
    {
        IntroloopPlayer.Instance.Pause(testSwitches.useFade ? testSwitches.fadeTime : 0);
    }

    public void Resume()
    {
        IntroloopPlayer.Instance.Resume(testSwitches.useFade ? testSwitches.fadeTime : 0);
    }

    public void Stop(bool rememberStopTime)
    {
        stopTime = rememberStopTime ? IntroloopPlayer.Instance.GetPlayheadTime() : 0;
        IntroloopPlayer.Instance.Stop(testSwitches.useFade ? testSwitches.fadeTime : 0);
        songNameText.text = "";
        genreText.text = "";
    }

    IEnumerator Start()
    {
        songNameText.text = "";
        genreText.text = "";
        yield break;
    }

    public Button[] playButtons;
    public Button pauseButton;
    public Button resumeButton;
    public Button stopButton;
    public Button rememberAndStopButton;
    public Toggle useFadeToggle;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            DSPPause();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            DSPResume();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            playButtons[0].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            playButtons[1].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            playButtons[2].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            playButtons[3].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            playButtons[4].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            playButtons[5].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            playButtons[6].onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            playButtons[7].onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            pauseButton.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            resumeButton.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            stopButton.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            rememberAndStopButton.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            useFadeToggle.isOn = !useFadeToggle.isOn;
            useFadeToggle.onValueChanged.Invoke(useFadeToggle.isOn);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            IntroloopPlayer.Instance.Preload(allIntroloopAudio[0]);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntroloopDemo");
        }
    }

}
