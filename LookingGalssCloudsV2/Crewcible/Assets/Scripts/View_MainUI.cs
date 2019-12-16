using System.Collections;
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
    public Image jumpIcon;

    public Image leftIconBig;
    public Image rightIconBig;

    public RectTransform leftNeedle;
    public RectTransform rightNeedle;
    public RectTransform centerNeedle;

    public RectTransform chargeSpinner;
    Image cSImage;

    public Image leftHeat;
    public Image rightHeat;
    public Image totalHeat;

    public RectTransform jumpProgressBar;
    Image jPBImage;

    [Range(0, 100)]
    public float warmBreakpoint;

    public Image hpBorder;
    public Image hpFill;

    void Start()
    {
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        InitializeAssets();

        SCG_EventManager.instance.Register<Event_EnemyBulletHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_LifeUpTick>(EventHandler);

        jPBImage = jumpProgressBar.GetComponent<Image>();
        cSImage = chargeSpinner.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIcons();
        UpdateStationHeat();
        UpdateTotalHeat();
        UpdateJumpProgress();
        UpdateColor();
        ChargeSpinner();
        HPUpdate();
    }

    void InitializeAssets()
    {
        gunsIcon = Resources.Load<Sprite>("Icons/" + "Guns");
        shieldIcon = Resources.Load<Sprite>("Icons/" + "Shield");
        rocketsIcon = Resources.Load<Sprite>("Icons/" + "Rockets");
        lanceIcon = Resources.Load<Sprite>("Icons/" + "Lance");
        thrustersIcon = Resources.Load<Sprite>("Icons/" + "Thursters");

        leftIconBig.color = gone;
        rightIconBig.color = gone;
    }

    public void EventHandler(SCG_Event e)
    {
        Event_EnemyBulletHit eBH = e as Event_EnemyBulletHit;
        if (eBH != null)
        {
            jPBImage.color = gameModel.c_Hot;
            hpFill.color = lifeHit;
        }

        Event_LifeUpTick lUT = e as Event_LifeUpTick;
        if (lUT != null)
        {
            hpFill.color = lifeTick;
        }
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

        leftIconBig.sprite = leftIcon.sprite;
        rightIconBig.sprite = rightIcon.sprite;
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
            _amount = Mathf.Clamp01(heatModel.heat_Thrusters / 100);
            _needleAng = 180 - 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Thrusters_Apparent / 100);
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
            _amount = Mathf.Clamp01(heatModel.heat_Thrusters / 100);
            _needleAng = 180 + 270 * _amount;
            _amount = Mathf.Clamp01(heatModel.heat_Thrusters_Apparent / 100);
        }
        _needleRot.z = _needleAng;
        rightNeedle.localEulerAngles = _needleRot;
        rightHeat.fillAmount = .75f * _amount;
    }

    void UpdateTotalHeat()
    {
        _amount = Mathf.Clamp01(heatModel.heat_Total / heatModel.max_HeatTotal);
        _needleAng = 180 - 360 * _amount;
        _needleRot.z = _needleAng;
        centerNeedle.localEulerAngles = _needleRot;

        _amount = Mathf.Clamp01(heatModel.heat_Total_Apparent / heatModel.max_HeatTotal);
        totalHeat.fillAmount = _amount;
    }

    void UpdateJumpProgress()
    {
        jumpProgressBar.localScale = new Vector3(Mathf.Clamp01(playModel.jumpTotal / 100), 1, 1);
    }

    float _lastJumpProgress;
    Color scratchColor;

    Stations leftLast;
    Stations rightLast;
    Color bigSplash = new Color(1, 1, 1, .35f);
    Color gone = new Color(1, 1, 1, 0);

    Color lifeNorm = new Color(1, 1, 1, .4f);
    Color lifeHit = Color.red;
    Color lifeTick = new Color(0, 1, .5f, 1);
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
            scratchColor = Color.Lerp(gameModel.c_Cool, gameModel.c_Warm, Easings.QuarticEaseInOut(heatModel.heat_Total / warmBreakpoint));
        else
            scratchColor = Color.Lerp(gameModel.c_Warm, gameModel.c_Hot, Easings.QuarticEaseInOut((heatModel.heat_Total - warmBreakpoint) / (150 - warmBreakpoint)));

        totalHeat.color = centerNeedle.GetComponent<Image>().color = scratchColor;
        scratchColor.a = .5f * scratchColor.a;
        cSImage.color = scratchColor;
        #endregion

        #region Jump
        if (_lastJumpProgress >= playModel.jumpTotal)
            jumpIcon.color = Color.red;
        else
            jumpIcon.color = new Color(1, 1, 1, .5f);

        jPBImage.color = Color.Lerp(jPBImage.color, gameModel.c_UI_Base, Time.deltaTime);

        _lastJumpProgress = playModel.jumpTotal;
        #endregion

        #region BigIcons

        if (playModel.leftStation != leftLast)
            leftIconBig.color = bigSplash;
        if (playModel.rightStation != rightLast)
            rightIconBig.color = bigSplash;

        leftLast = playModel.leftStation;
        rightLast = playModel.rightStation;

        leftIconBig.color = Color.Lerp(leftIconBig.color, gone, 4*Time.deltaTime);
        rightIconBig.color = Color.Lerp(rightIconBig.color, gone, 4*Time.deltaTime);
        #endregion

        #region Life
        hpFill.color = Color.Lerp(hpFill.color, lifeNorm, Time.deltaTime * 1.6f);
        #endregion
    }

    public float spinSpeedBase;
    Vector3 euler;
    void ChargeSpinner()
    {
        //Debug.Log(spinSpeedBase * heatModel.heatToChargeConversionFactor.Evaluate(heatModel.heat_Total / heatModel.max_HeatTotal) * Time.deltaTime);
        euler.z += spinSpeedBase * heatModel.heatToChargeConversionFactor.Evaluate(heatModel.heat_Total / heatModel.max_HeatTotal) * Time.deltaTime;
        chargeSpinner.localRotation = Quaternion.Euler(euler);
    }

    void HPUpdate()
    {
        hpFill.fillAmount = (playModel.playerHP / 5) * .42f + .29f;
    }
}
