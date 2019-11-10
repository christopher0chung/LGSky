﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class View_MainUI : MonoBehaviour
{
    Model_Heat heatModel;
    Model_Play playModel;
    Model_Game gameModel;

    Sprite gunsIcon;
    Sprite shieldIcon;
    Sprite rocketsIcon;
    Sprite lanceIcon;
    Sprite thrustersIcon;

    public Image leftIcon;
    public Image rightIcon;

    public RectTransform leftNeedle;
    public RectTransform rightNeedle;
    public RectTransform centerNeedle;

    public Image leftHeat;
    public Image rightHeat;
    public Image totalHeat;

    [Range(0, 100)]
    public float warmBreakpoint;

    void Start()
    {
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        InitializeAssets();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIcons();
        UpdateStationHeat();
        UpdateTotalHeat();
        UpdateJumpProgress();
        UpdateColor();
    }

    void InitializeAssets()
    {
        gunsIcon = Resources.Load<Sprite>("Icons/" + "Guns");
        shieldIcon = Resources.Load<Sprite>("Icons/" + "Shield");
        rocketsIcon = Resources.Load<Sprite>("Icons/" + "Rockets");
        lanceIcon = Resources.Load<Sprite>("Icons/" + "Lance");
        thrustersIcon = Resources.Load<Sprite>("Icons/" + "Thursters");
    }

    void UpdateIcons()
    {
        Image toAssign = leftIcon;
        if (playModel.leftStation == Stations.Guns)
            toAssign.sprite = gunsIcon;
        else if (playModel.leftStation == Stations.Shield)
            toAssign.sprite = shieldIcon;
        else if (playModel.leftStation == Stations.Rockets)
            toAssign.sprite = rocketsIcon;
        else if (playModel.leftStation == Stations.Lance)
            toAssign.sprite = lanceIcon;
        else if (playModel.leftStation == Stations.Thrusters)
            toAssign.sprite = thrustersIcon;

        toAssign = rightIcon;
        if (playModel.rightStation == Stations.Guns)
            toAssign.sprite = gunsIcon;
        else if (playModel.rightStation == Stations.Shield)
            toAssign.sprite = shieldIcon;
        else if (playModel.rightStation == Stations.Rockets)
            toAssign.sprite = rocketsIcon;
        else if (playModel.rightStation == Stations.Lance)
            toAssign.sprite = lanceIcon;
        else if (playModel.rightStation == Stations.Thrusters)
            toAssign.sprite = thrustersIcon;
    }

    private float _needleAng;
    private Vector3 _needleRot;
    private float _amount;
    void UpdateStationHeat()
    {
        if (playModel.leftStation == Stations.Guns)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Guns / 100);
            _needleAng = 180 - 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Guns_Apparent / 100);
        }
        else if (playModel.leftStation == Stations.Shield)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Shield / 100);
            _needleAng = 180 - 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Shield_Apparent / 100);
        }
        else if (playModel.leftStation == Stations.Rockets)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Rockets / 100);
            _needleAng = 180 - 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Rockets_Apparent / 100);
        }
        else if (playModel.leftStation == Stations.Lance)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Lance / 100);
            _needleAng = 180 - 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Lance_Apparent / 100);
        }
        else if (playModel.leftStation == Stations.Thrusters)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Lance / 100);
            _needleAng = 180 - 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Lance_Apparent / 100);
        }
        _needleRot.z = _needleAng;
        leftNeedle.localEulerAngles = _needleRot;
        leftHeat.fillAmount = .75f * _amount;


        if (playModel.rightStation == Stations.Guns)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Guns / 100);
            _needleAng = 180 + 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Guns_Apparent / 100);
        }
        else if (playModel.rightStation == Stations.Shield)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Shield / 100);
            _needleAng = 180 + 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Shield_Apparent / 100);
        }
        else if (playModel.rightStation == Stations.Rockets)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Rockets / 100);
            _needleAng = 180 + 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Rockets_Apparent / 100);
        }
        else if (playModel.rightStation == Stations.Lance)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Lance / 100);
            _needleAng = 180 + 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Lance_Apparent / 100);
        }
        else if (playModel.rightStation == Stations.Thrusters)
        {
            _amount = Mathf.Clamp01(heatModel.heat_Lance / 100);
            _needleAng = 180 + 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Lance_Apparent / 100);
        }
        _needleRot.z = _needleAng;
        rightNeedle.localEulerAngles = _needleRot;
        rightHeat.fillAmount = .75f * _amount;
    }

    void UpdateTotalHeat()
    {
        _amount = Mathf.Clamp01(heatModel.heat_Total / 150);
        _needleAng = 180 - 360 * _amount;
        _needleRot.z = _needleAng;
        centerNeedle.localEulerAngles = _needleRot;

        _amount = Mathf.Clamp01(heatModel.heat_Total_Apparent / 150);
        totalHeat.fillAmount = _amount;
    }

    void UpdateJumpProgress()
    {

    }

    void UpdateColor()
    {
        #region Left
        if (playModel.leftStation == Stations.Guns)
        {
            if (heatModel.heat_Guns < warmBreakpoint)
                leftHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Guns / warmBreakpoint));
            else
                leftHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Guns - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Guns)
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        else if (playModel.leftStation == Stations.Shield)
        {
            if (heatModel.heat_Shield < warmBreakpoint)
                leftHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Shield / warmBreakpoint));
            else
                leftHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Shield - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Shield)
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        else if (playModel.leftStation == Stations.Rockets)
        {
            if (heatModel.heat_Rockets < warmBreakpoint)
                leftHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Rockets / warmBreakpoint));
            else
                leftHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Rockets - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Rockets)
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        else if (playModel.leftStation == Stations.Lance)
        {
            if (heatModel.heat_Lance < warmBreakpoint)
                leftHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Lance / warmBreakpoint));
            else
                leftHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Lance - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Lance)
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        else if (playModel.leftStation == Stations.Thrusters)
        {
            if (heatModel.heat_Thrusters < warmBreakpoint)
                leftHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Thrusters / warmBreakpoint));
            else
                leftHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Thrusters - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Thrusters)
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                leftIcon.color = leftNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        #endregion

        #region Right
        if (playModel.rightStation == Stations.Guns)
        {
            if (heatModel.heat_Guns < warmBreakpoint)
                rightHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Guns / warmBreakpoint));
            else
                rightHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Guns - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Guns)
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        else if (playModel.rightStation == Stations.Shield)
        {
            if (heatModel.heat_Shield < warmBreakpoint)
                rightHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Shield / warmBreakpoint));
            else
                rightHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Shield - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Shield)
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        else if (playModel.rightStation == Stations.Rockets)
        {
            if (heatModel.heat_Rockets < warmBreakpoint)
                rightHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Rockets / warmBreakpoint));
            else
                rightHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Rockets - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Rockets)
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        else if (playModel.rightStation == Stations.Lance)
        {
            if (heatModel.heat_Lance < warmBreakpoint)
                rightHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Lance / warmBreakpoint));
            else
                rightHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Lance - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Lance)
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        else if (playModel.rightStation == Stations.Thrusters)
        {
            if (heatModel.heat_Thrusters < warmBreakpoint)
                rightHeat.color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Thrusters / warmBreakpoint));
            else
                rightHeat.color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Thrusters - warmBreakpoint) / (100 - warmBreakpoint)));

            if (heatModel.overheated_Thrusters)
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Hot;
            else
                rightIcon.color = rightNeedle.GetComponent<Image>().color = gameModel.c_Cool;
        }
        #endregion

        #region Total
        if (heatModel.heat_Total < warmBreakpoint)
            totalHeat.color = centerNeedle.GetComponent<Image>().color = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Total / warmBreakpoint));
        else
            totalHeat.color = centerNeedle.GetComponent<Image>().color = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Total - warmBreakpoint) / (150 - warmBreakpoint)));
        #endregion
    }
}
