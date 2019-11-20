using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller_Menu : SCG_Controller
{
    Model_ControllerRefs input;
    Model_Input inputModel;

    public Text p1;
    public Text p2;
    public Text c;

    private float oldL;
    private float oldR;

    public Image up;
    public Image down;
    public Image aButton;

    int choices;
    float threshold = .25f;

    Color normalArrowColor = new Color(1, 1, 1, .25f);

    Color buttonColor = Color.white;

    private void Start()
    {
        priority = 2;
        Schedule(this);
        input = ServiceLocator.instance.controllerRefs;
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        choices = 0;
    }

    public override void ScheduledUpdate()
    {
        buttonColor.a = .5f * Mathf.Sin(7 * Time.time) + .5f;
        aButton.color = buttonColor;

        if (input.device0 != null)
            p1.text = "p1";
        else
            p1.text = "";

        if (input.device1 != null) 
            p2.text = "p2";
        else
            p2.text = "";

        if ((inputModel.L_Y >= threshold && oldL < threshold) || (inputModel.R_Y >= threshold && oldR < threshold))
        {
            choices--;
            if (choices < 0)
                choices = 2;
            up.color = Color.white;
        }
        if ((inputModel.L_Y <= -threshold && oldL > -threshold) || (inputModel.R_Y <= -threshold && oldR > -threshold))
        {
            choices++;
            if (choices > 2)
                choices = 0;
            down.color = Color.white;
        }

        up.color = Color.Lerp(up.color, normalArrowColor, 5 * Time.deltaTime);
        down.color = Color.Lerp(down.color, normalArrowColor, 5 * Time.deltaTime);

        oldL = inputModel.L_Y;
        oldR = inputModel.R_Y;

        if (choices == 0)
            c.text = "play";
        else if (choices == 1)
            c.text = "practice";
        else if (choices == 2)
            c.text = "controls";

        if ((inputModel.L_Action_OnDown || inputModel.R_Action_OnDown) && (ServiceLocator.instance.controllerRefs.device0 != null || ServiceLocator.instance.controllerRefs.device1 != null))
            SceneManager.LoadScene(1);
    }
}
