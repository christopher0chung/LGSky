using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Gun : SCG_Controller {

    private Model_Game gameModel;
    private Model_Energy energyModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    private Transform player;
    private Manager_GameAssets assetManager;

    private Transform swivel;
    private Transform pitcher;
    private Transform gun;
    private MeshRenderer gunPointer;

    private AudioSource myAS;

    private bool leftRightBarrel;

    private float shootTimer;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        energyModel = ServiceLocator.instance.Model.GetComponent<Model_Energy>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        player = ServiceLocator.instance.Player;

        assetManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
    }

    void Start () {
        swivel = player.GetChild(0);
        pitcher = swivel.GetChild(0);
        gun = player.GetChild(1);
        gunPointer = gun.GetChild(0).GetComponent<MeshRenderer>();
        myAS = player.GetComponent<AudioSource>();
    }
	
	void Update () {
        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
            if (gameModel.leftStation == Stations.Guns)
            {
                _CalculateAndMoveGunPointer(inputModel.L_Brg, inputModel.L_Mag);
                _FiringController(inputModel.L_Action_Down, inputModel.L_Action_OnUp);
                pitcher.gameObject.SetActive(true);
                gunPointer.enabled = true;
            }
            else if (gameModel.rightStation == Stations.Guns)
            {
                _CalculateAndMoveGunPointer(inputModel.R_Brg, inputModel.R_Mag);
                _FiringController(inputModel.R_Action_Down, inputModel.R_Action_OnUp);
                pitcher.gameObject.SetActive(true);
                gunPointer.enabled = true;
            }
            else
            {
                pitcher.gameObject.SetActive(false);
                gunPointer.enabled = false;
            }
        }
        else
        {
            pitcher.gameObject.SetActive(false);
            gunPointer.enabled = false;
        }
	}

    #region Guns
    private void _CalculateAndMoveGunPointer(float brg, float dec)
    {
        swivel.eulerAngles = new Vector3(0, -brg, 0);
        pitcher.localEulerAngles = new Vector3(dec, 0, 0);

        if (energyModel.gunsOn)
            gun.rotation = Quaternion.RotateTowards(gun.rotation, pitcher.GetChild(0).rotation, gameModel.s_GunMoveFiring * Time.deltaTime);
        else
            gun.rotation = Quaternion.RotateTowards(gun.rotation, pitcher.GetChild(0).rotation, gameModel.s_GunMovePassive * Time.deltaTime);
    }

    private void _FiringController(bool shoot, bool release)
    {
        energyModel.gunsOn = shoot;
        if (shoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer - gameModel.t_SpinUpTime >= gameModel.t_TimeBetweenShots)
            {
                shootTimer -= gameModel.t_TimeBetweenShots;
                GameObject bullet;

                Vector3 rando = Random.insideUnitCircle;
                if (leftRightBarrel)
                    bullet = assetManager.Make(MyGameAsset.Bullet, player.position + Vector3.left * .1f);
                else
                    bullet = assetManager.Make(MyGameAsset.Bullet, player.position + Vector3.right * .1f);
                bullet.GetComponent<Rigidbody>().AddForce(gun.forward * 30 + rando * 4, ForceMode.Impulse);
                leftRightBarrel = !leftRightBarrel;
                myAS.PlayOneShot(gameModel.gunShot);
            }
        }
        if (release)
            shootTimer = 0;
    }
    #endregion
}
