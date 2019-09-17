using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class View_UI_Canvas : MonoBehaviour
{
    public Model_Game gameModel;
    public Model_Energy energyModel;

    public Image left;
    public Image right;

    public RectTransform jumpBar;
    public RectTransform jumpIcon;
    public RectTransform energyBar;
    public Image meterLeft;
    public Image meterRight;
    public RectTransform needleLeft;
    public RectTransform needleRight;

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

    private AudioSource myAS;

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

        myAS = GetComponent<AudioSource>();
    }

    void Update()
    {
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
        meterLeft.fillAmount = Mathf.Lerp(0.00f, 0.75f, scratchFloatLeft);
        meterRight.fillAmount = Mathf.Lerp(0.00f, 0.75f, scratchFloatRight);

        needleLeft.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(180, -90, scratchFloatLeft));
        needleRight.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-180, 90, scratchFloatRight));

        jumpBar.sizeDelta = new Vector2(Mathf.Lerp(0, 1630, scratchFloatTop), jumpBar.sizeDelta.y);
        jumpIcon.localPosition = Vector3.Lerp(new Vector3(-863, 556.3f, 0), new Vector3(770, 556.3f, 0), scratchFloatTop);

        energyBar.sizeDelta = new Vector2(Mathf.Lerp(5, 725, scratchFloatBottom), energyBar.sizeDelta.y);
        energyBar.localPosition = Vector3.Lerp(
            Vector3.left * 175 + Vector3.up * -547.1f,
            Vector3.right * 175 + Vector3.up * -547.1f,
            .5f);

        //jumpBar.position = Vector3.Lerp(Vec)


        //BarColors();

        //_UpdateFlashingParams();
        //_CheckAndApplyFlashing(scratchFloatTop, .1f, .99f, topRenderer);
        //_CheckAndApplyFlashing(scratchFloatBottom, .25f, 1, bottomRenderer);

        //_CheckAndPlayAlarm(energyModel.jump_Actual);
    }
}
