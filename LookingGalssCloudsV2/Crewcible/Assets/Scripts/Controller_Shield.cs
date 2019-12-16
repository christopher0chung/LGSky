using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Shield : SCG_Controller {

    private Model_Game gameModel;
    private Model_Heat heatModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    private Manager_GameAssets assetManager;

    //public Material shieldMat;

    private MeshRenderer shieldLinesRenderer;

    private float bouncePerc = .6f;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();

        assetManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();

        shieldLinesRenderer = ServiceLocator.instance.Player.Find("ShipParent").Find("PlayerShip").Find("ShieldEdges").GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        priority = 4;
        Schedule(this);

        SCG_EventManager.instance.Register<Event_PlayerShieldBlock>(EventHandler);
    }

    public override void ScheduledUpdate () {
        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
            if (playModel.leftStation == Stations.Shield)
            {
                _ShieldCalculateAndMove(inputModel.L_X, inputModel.L_Y);
                _ShieldSize(inputModel.L_Action_Down);

                shieldLinesRenderer.enabled = true;
            }
            else if (playModel.rightStation == Stations.Shield)
            {
                _ShieldCalculateAndMove(inputModel.R_X, inputModel.R_Y);
                _ShieldSize(inputModel.R_Action_Down);

                shieldLinesRenderer.enabled = true;
            }
            else
            {
                playModel.shieldSize = 1.1f;
                shieldLinesRenderer.enabled = false;
            }
        }
        else
        {
            playModel.shieldSize = 1.1f;

            shieldLinesRenderer.enabled = false;
            heatModel.active_Shield = false;
        }
    }

    public void EventHandler(SCG_Event e)
    {
        if (playModel.shieldSize == gameModel.f_Shield_Cutoff_Min)
        {
            Event_PlayerShieldBlock psb = e as Event_PlayerShieldBlock;
            if (psb != null)
            {
                float outCome = Random.Range(0.000f, 1.0000f);
                if (outCome <= bouncePerc)
                {
                    GameObject bullet = assetManager.Make(MyGameAsset.Bullet, ServiceLocator.instance.Player.position + (Vector3)playModel.shieldDirection);
                    bullet.transform.localScale = Vector3.one * 5;
                    bullet.GetComponent<Behavior_Bullet>().bounced = true;
                    bullet.GetComponent<Rigidbody>().AddForce((Vector3)playModel.shieldDirection * gameModel.s_Guns_BulletSpeed * 2.8f, ForceMode.Impulse);
                }
            }
        }
    }

    #region Shield
    private void _ShieldCalculateAndMove(float inputX, float inputY)
    {
        playModel.shieldDirection.x = inputX;
        playModel.shieldDirection.z = inputY;
        playModel.shieldDirection.y = 1 - (Mathf.Sqrt(inputX * inputX + inputY * inputY));
    }

    private void _ShieldSize(bool on)
    {
        if (!heatModel.overheated_Shield)
        {
            heatModel.active_Shield = on;

            if (on)
                playModel.shieldSize = gameModel.f_Shield_Cutoff_Max;
            else
                playModel.shieldSize = gameModel.f_Shield_Cutoff_Min;
        }
        else
        {
            playModel.shieldSize = gameModel.f_Shield_Cutoff_Min;
        }
    }
    #endregion
}
