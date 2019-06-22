using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_UI: MonoBehaviour {

    public Model_Game gameModel;
    public Model_Energy energyModel;

    public SpriteRenderer left;
    public SpriteRenderer right;

    public Transform barLeft;
    public Transform barRight;
    public Transform barTop;
    public Transform barBottom;

    public Color color_Charged;
    public Color color_Charge;
    public Color color_Hold;
    public Color color_Discharge;
    public Color color_Danger;

    public float flashingTimeOn;
    public float flashingTImeOff;

    private Vector3 vertBarMinSize = new Vector3(.2f, .1f, .2f);
    private Vector3 vertBarMaxSize = new Vector3(.2f, 2.95f, .2f);
    private Vector3 horzBarMinSize = new Vector3(.2f, .1f, .2f);
    private Vector3 horzBarMaxSize = new Vector3(.2f, 5.95f, .2f);

    private Vector3 leftMinPos = new Vector3(-7.31f, 0.378f, -1.046f);
    private Vector3 leftMaxPos = new Vector3(-7.31f, 3.125f, -0.287f);
    private Vector3 rightMinPos = new Vector3(7.31f, 0.378f, -1.046f);
    private Vector3 rightMaxPos = new Vector3(7.31f, 3.125f, -0.287f);

    private Vector3 topMinPos = new Vector3(-5.29f, 6.53f, 0.65f);
    private Vector3 topMaxPos = new Vector3(.56f, 6.53f, 0.65f);
    private Vector3 botMinPos = new Vector3(-5.29f, -1.92f, -1.68f);
    private Vector3 botMaxPos = new Vector3(.56f, -1.92f, -1.68f);

    private float scratchFloatLeft;
    private float scratchFloatRight;
    private float scratchFloatBottom;
    private float scratchFloatTop;

    private float _gunsOld;
    private float _pilotOld;
    private float _rocketsOld;
    private float _shieldsOld;
    private float _swordOld;
    private float _reactorOld;
    private float _jumpOld;

    #region Materials
    Material barTopMat;
    Material barBotMat;
    Material barLeftMat;
    Material barRightMat;
    #endregion

    #region MeshRenderers
    MeshRenderer topRenderer;
    MeshRenderer bottomRenderer;
    #endregion

    #region Internal Variables
    float _flashingTimer;
    bool _flashingOnOff;
    #endregion

    #region Icons
    private Stations _l;
    private Stations _left
    {
        get { return _l; }
        set
        {
            if (_l != value)
            {
                _l = value;
                left.sprite = Resources.Load<Sprite>("UI/" + _l.ToString());
            }
        }
    }

    private Stations _r;
    private Stations _right
    {
        get { return _r; }
        set
        {
            if (_r != value)
            {
                _r = value;
                right.sprite = Resources.Load<Sprite>("UI/" + _r.ToString());
            }
        }
    }
    #endregion

    void Start()
    {
        left.sprite = Resources.Load<Sprite>("UI/" + _l.ToString());
        right.sprite = Resources.Load<Sprite>("UI/" + _r.ToString());

        topRenderer = barTop.GetComponent<MeshRenderer>();
        bottomRenderer = barBottom.GetComponent<MeshRenderer>();

        barTopMat = barTop.GetComponent<MeshRenderer>().material;
        barBotMat = barBottom.GetComponent<MeshRenderer>().material;
        barLeftMat = barLeft.GetComponent<MeshRenderer>().material;
        barRightMat = barRight.GetComponent<MeshRenderer>().material;
    }

    void Update () {
        _left = gameModel.leftStation;
        _right = gameModel.rightStation;

        // Left
        if (_left == Stations.Guns)
            scratchFloatLeft = energyModel.guns_Apparent / 100;
        else if (_left == Stations.Pilot)
            scratchFloatLeft = energyModel.pilot_Apparent / 100;
        else if (_left == Stations.Rockets)
            scratchFloatLeft = energyModel.rockets_Apparent / 100;
        else if (_left == Stations.Shield)
            scratchFloatLeft = energyModel.shield_Apparent / 100;
        else if (_left == Stations.Sword)
            scratchFloatLeft = energyModel.sword_Apparent / 100;

        // Right
        if (_right == Stations.Guns)
            scratchFloatRight = energyModel.guns_Apparent / 100;
        else if (_right == Stations.Pilot)
            scratchFloatRight = energyModel.pilot_Apparent / 100;
        else if (_right == Stations.Rockets)
            scratchFloatRight = energyModel.rockets_Apparent / 100;
        else if (_right == Stations.Shield)
            scratchFloatRight = energyModel.shield_Apparent / 100;
        else if (_right == Stations.Sword)
            scratchFloatRight = energyModel.sword_Apparent / 100;

        // Bottom
        scratchFloatBottom = energyModel.reactor_Apparent / 100;

        // Top
        scratchFloatTop = energyModel.jump_Apparent / 100;

        // Clamp
        scratchFloatLeft = Mathf.Clamp01(scratchFloatLeft);
        scratchFloatRight = Mathf.Clamp01(scratchFloatRight);
        scratchFloatBottom = Mathf.Clamp01(scratchFloatBottom);
        scratchFloatTop = Mathf.Clamp01(scratchFloatTop);

        // Display
        barLeft.localPosition = Vector3.Lerp(leftMinPos, leftMaxPos, scratchFloatLeft);
        barLeft.localScale = Vector3.Lerp(vertBarMinSize, vertBarMaxSize, scratchFloatLeft);

        barRight.localPosition = Vector3.Lerp(rightMinPos, rightMaxPos, scratchFloatRight);
        barRight.localScale = Vector3.Lerp(vertBarMinSize, vertBarMaxSize, scratchFloatRight);

        barBottom.localPosition = Vector3.Lerp(botMinPos, botMaxPos, scratchFloatBottom);
        barBottom.localScale = Vector3.Lerp(horzBarMinSize, horzBarMaxSize, scratchFloatBottom);

        barTop.localPosition = Vector3.Lerp(topMinPos, topMaxPos, scratchFloatTop);
        barTop.localScale = Vector3.Lerp(horzBarMinSize, horzBarMaxSize, scratchFloatTop);

        BarColors();

        _UpdateFlashingParams();
        _CheckAndApplyFlashing(scratchFloatTop, .1f, .99f, topRenderer);
        _CheckAndApplyFlashing(scratchFloatBottom, .25f, 1, bottomRenderer);
    }

    void BarColors()
    {
        _CheckAndApplyColorStd(_reactorOld, energyModel.reactor_Apparent, barBotMat);
        _CheckAndApplyColorStd(_jumpOld, energyModel.jump_Apparent, barTopMat);
        
        // Left
        if (_left == Stations.Guns)
            _CheckAndApplyColorInv(_gunsOld, energyModel.guns_Apparent, barLeftMat);
        else if (_left == Stations.Pilot)
            _CheckAndApplyColorInv(_pilotOld, energyModel.pilot_Apparent, barLeftMat);
        else if (_left == Stations.Rockets)
            _CheckAndApplyColorInv(_rocketsOld, energyModel.rockets_Apparent, barLeftMat);
        else if (_left == Stations.Shield)
            _CheckAndApplyColorInv(_shieldsOld, energyModel.shield_Apparent, barLeftMat);
        else if (_left == Stations.Sword)
            _CheckAndApplyColorInv(_swordOld, energyModel.sword_Apparent, barLeftMat);
        // Right
        if (_right == Stations.Guns)
            _CheckAndApplyColorInv(_gunsOld, energyModel.guns_Apparent, barRightMat);
        else if (_right == Stations.Pilot)
            _CheckAndApplyColorInv(_pilotOld, energyModel.pilot_Apparent, barRightMat);
        else if (_right == Stations.Rockets)
            _CheckAndApplyColorInv(_rocketsOld, energyModel.rockets_Apparent, barRightMat);
        else if (_right == Stations.Shield)
            _CheckAndApplyColorInv(_shieldsOld, energyModel.shield_Apparent, barRightMat);
        else if (_right == Stations.Sword)
            _CheckAndApplyColorInv(_swordOld, energyModel.sword_Apparent, barRightMat);

        _gunsOld = energyModel.guns_Apparent;
        _pilotOld = energyModel.pilot_Apparent;
        _rocketsOld = energyModel.rockets_Apparent;
        _shieldsOld = energyModel.shield_Apparent;
        _swordOld = energyModel.sword_Apparent;
        _reactorOld = energyModel.reactor_Apparent;
        _jumpOld = energyModel.jump_Apparent;
    }

    private void _CheckAndApplyColorStd(float oldVal, float newVal, Material bar)
    {
        float del = newVal - oldVal;

        if (Mathf.Abs(del) <= .01f)
        {
            bar.SetColor("_Color0", color_Hold);
            bar.SetColor("_Color1", color_Hold);
        }
        else if (del <= 0)
        {
            bar.SetColor("_Color0", color_Discharge);
            bar.SetColor("_Color1", color_Discharge);
        }
        else
        {
            bar.SetColor("_Color0", color_Charge);
            bar.SetColor("_Color1", color_Charge);
        }
    }
    private void _CheckAndApplyColorInv(float oldVal, float newVal, Material bar)
    {
        float del = newVal - oldVal;

        if (Mathf.Abs(del) <= .01f)
        {
            bar.SetColor("_Color0", color_Hold);
            bar.SetColor("_Color1", color_Hold);
        }
        else if (del <= 0)
        {
            bar.SetColor("_Color0", color_Charge);
            bar.SetColor("_Color1", color_Charge);
        }
        else
        {
            bar.SetColor("_Color0", color_Discharge);
            bar.SetColor("_Color1", color_Discharge);
        }
    }

    private void _UpdateFlashingParams()
    {
        _flashingTimer += Time.deltaTime;
        if (_flashingOnOff && _flashingTimer >= flashingTimeOn)
        {
            _flashingOnOff = false;
            _flashingTimer -= flashingTimeOn;
        }
        else if (!_flashingOnOff && _flashingTimer >= flashingTImeOff)
        {
            _flashingOnOff = true;
            _flashingTimer -= flashingTImeOff;
        }
    }
    private void _CheckAndApplyFlashing(float currentVal, float lowThreshold, float highThreshold, MeshRenderer renderer)
    {
        if (currentVal <= lowThreshold || currentVal >= highThreshold)
            renderer.enabled = _flashingOnOff;
        else
            renderer.enabled = true;
    }
}
