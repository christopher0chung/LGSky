using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Gun : SCG_Controller {

    private Model_Game gameModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    private Model_Heat heatModel;
    private Transform player;
    private Manager_GameAssets assetManager;

    public Transform swivel;
    public Transform pitcher;
    public Transform inputReticle;

    public Transform turret;
    public Transform guns;
    public Transform gunPointer;

    private AudioSource myAS;

    private bool leftRightBarrel;

    private float shootTimer;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        player = ServiceLocator.instance.Player;

        assetManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
    }

    void Start () {
        swivel = player.Find("Reticles").Find("GunReticles_Swivel");
        pitcher = swivel.Find("GunReticles_Pitcher");
        inputReticle = pitcher.Find("GunReticles_MR");

        turret = player.Find("ShipParent").Find("PlayerShip").Find("Turret");
        guns = turret.Find("Guns");
        gunPointer = guns.Find("GunPointer");

        myAS = player.GetComponent<AudioSource>();

        priority = 1;
        Schedule(this);
    }
	
	public override void ScheduledUpdate () {
        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
            if (playModel.leftStation == Stations.Guns)
            {
                _CalculateAndMoveGunPointer(inputModel.L_Brg, inputModel.L_Mag);
                _FiringController(inputModel.L_Action_Down, inputModel.L_Action_OnUp);
                pitcher.gameObject.SetActive(true);
                gunPointer.gameObject.SetActive(true);
                inputReticle.gameObject.SetActive(true);
            }
            else if (playModel.rightStation == Stations.Guns)
            {
                _CalculateAndMoveGunPointer(inputModel.R_Brg, inputModel.R_Mag);
                _FiringController(inputModel.R_Action_Down, inputModel.R_Action_OnUp);
                pitcher.gameObject.SetActive(true);
                gunPointer.gameObject.SetActive(true);
                inputReticle.gameObject.SetActive(true);
            }
            else
            {
                _ResetGunPos();
                pitcher.gameObject.SetActive(false);
                gunPointer.gameObject.SetActive(false);
                inputReticle.gameObject.SetActive(false);
            }
        }
        else
        {
            _ResetGunPos();
            pitcher.gameObject.SetActive(false);
            gunPointer.gameObject.SetActive(false);
            inputReticle.gameObject.SetActive(false);
        }
    }

    #region Guns
    private void _CalculateAndMoveGunPointer(float brg, float dec)
    {
        swivel.eulerAngles = new Vector3(0, -brg, 0);
        pitcher.localEulerAngles = new Vector3(dec, 0, 0);

        if (heatModel.active_Guns)
        {
            turret.rotation = Quaternion.RotateTowards(turret.rotation, swivel.rotation, gameModel.s_Guns_TurnSpeed_Activated * Time.deltaTime);
            guns.localRotation = Quaternion.RotateTowards(guns.localRotation, pitcher.localRotation, gameModel.s_Guns_TurnSpeed_Activated * Time.deltaTime);
        }
        else
        {
            turret.rotation = Quaternion.RotateTowards(turret.rotation, swivel.rotation, gameModel.s_Guns_TurnSpeed_Inactivated * Time.deltaTime);
            guns.localRotation = Quaternion.RotateTowards(guns.localRotation, pitcher.localRotation, gameModel.s_Guns_TurnSpeed_Inactivated * Time.deltaTime);
        }
    }

    private void _ResetGunPos()
    {
        //Debug.Log("In here");
        turret.rotation = Quaternion.Slerp(turret.rotation, Quaternion.identity, .75f * Time.deltaTime);
        guns.localRotation = Quaternion.Slerp(guns.localRotation, Quaternion.Euler(90, 0, 0), .75f * Time.deltaTime);
    }

    private void _FiringController(bool shoot, bool release)
    {
        heatModel.active_Guns = shoot;
        if (shoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer - gameModel.t_Guns_SpinUpTime >= gameModel.t_Guns_TimeBetweenShots)
            {
                shootTimer -= gameModel.t_Guns_TimeBetweenShots;
                GameObject bullet;

                Vector3 rando = Random.insideUnitCircle * gameModel.f_Guns_BulletDispersion;


                if (leftRightBarrel)
                    bullet = assetManager.Make(MyGameAsset.Bullet, guns.position - guns.right * .1f + guns.up * .7f);
                else
                    bullet = assetManager.Make(MyGameAsset.Bullet, guns.position + guns.right * .1f + guns.up * .7f);

                bullet.GetComponent<Rigidbody>().AddForce(guns.up * 30 + rando * 4, ForceMode.Impulse);
                leftRightBarrel = !leftRightBarrel;
                //myAS.PlayOneShot(gameModel.sfx_Gun_Shot);
            }
        }
        if (release)
            shootTimer = 0;
    }
    #endregion
}
