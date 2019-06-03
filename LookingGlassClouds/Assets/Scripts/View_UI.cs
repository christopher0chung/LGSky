using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_UI: MonoBehaviour {

    public Model_Game gameModel;

    public SpriteRenderer left;
    public SpriteRenderer right;

    public Transform barLeft;
    public Transform barRight;
    public Transform barTop;
    public Transform barBottom;

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
    }

    void Update () {
        _left = gameModel.leftStation;
        _right = gameModel.rightStation;

        // Left
        if (_left == Stations.Guns)
            scratchFloatLeft = gameModel.e_GunEnergy_Actual / 100;
        else if (_left == Stations.Pilot)
            scratchFloatLeft = gameModel.e_PilotEnergy_Actual / 100;
        else if (_left == Stations.Rockets)
            scratchFloatLeft = gameModel.e_RocketEnergy_Actual / 100;
        else if (_left == Stations.Shield)
            scratchFloatLeft = gameModel.e_ShieldEnergy_Actual / 100;
        else if (_left == Stations.Sword)
            scratchFloatLeft = gameModel.e_SwordEnergy_Actual / 100;

        // Right
        if (_right == Stations.Guns)
            scratchFloatRight = gameModel.e_GunEnergy_Actual / 100;
        else if (_right == Stations.Pilot)
            scratchFloatRight = gameModel.e_PilotEnergy_Actual / 100;
        else if (_right == Stations.Rockets)
            scratchFloatRight = gameModel.e_RocketEnergy_Actual / 100;
        else if (_right == Stations.Shield)
            scratchFloatRight = gameModel.e_ShieldEnergy_Actual / 100;
        else if (_right == Stations.Sword)
            scratchFloatRight = gameModel.e_SwordEnergy_Actual / 100;

        // Bottom
        scratchFloatBottom = gameModel.e_ReactorEnergy_Actual / 100;

        // Top
        scratchFloatTop = gameModel.e_JumpEnergy_Actual / 100;

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
    }
}
