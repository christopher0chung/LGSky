using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class CustomPost : MonoBehaviour
{
    public Material Post_Kuwahara;

    public PostStates currentState;

    public Text readout;

    private int countOfEnums;

    List<string> names;
    private void Start()
    {
        countOfEnums = Enum.GetValues(typeof(PostStates)).Length;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            int intOfCurrentState = (int)currentState;

            Debug.Log(countOfEnums);

            int nextState = (intOfCurrentState + 1) % countOfEnums;
            PostStates next = (PostStates)nextState;
            currentState = next;
        }

        readout.text = currentState.ToString();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (currentState == PostStates.None)
            return;
        else if (currentState == PostStates.Kuwahara)
            Graphics.Blit(source, destination, Post_Kuwahara);
    }
}

public enum PostStates { None, Kuwahara}