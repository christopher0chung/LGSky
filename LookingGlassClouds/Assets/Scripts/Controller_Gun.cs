using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Gun : MonoBehaviour {

    public Model_Game gameModel;
    public Model_Input inputModel;
    public Manager_GameAssets assetManager;

    public Transform player;
    private Transform swivel;
    private Transform pitcher;
    private MeshRenderer reticle;
    private Transform gun;
    private MeshRenderer gunPointer;

    private AudioSource myAS;

    private bool leftRightBarrel;

    private float shootTimer;

    void Start () {
        swivel = player.GetChild(0);
        pitcher = swivel.GetChild(0);
        reticle = pitcher.GetChild(0).GetComponent<MeshRenderer>();
        gun = player.GetChild(1);
        gunPointer = gun.GetChild(0).GetComponent<MeshRenderer>();
        myAS = player.GetComponent<AudioSource>();
    }
	
	void Update () {
        if (gameModel.leftStation == Stations.Guns)
        {
            _CalculateAndMoveGunPointer(inputModel.L_Brg, inputModel.L_Mag);
            _FiringController(inputModel.L_Action_Down, inputModel.L_Action_OnUp);
            reticle.enabled = true;
            gunPointer.enabled = true;
        }
        else if (gameModel.rightStation == Stations.Guns)
        {
            _CalculateAndMoveGunPointer(inputModel.R_Brg, inputModel.R_Mag);
            _FiringController(inputModel.R_Action_Down, inputModel.R_Action_OnUp);
            reticle.enabled = true;
            gunPointer.enabled = true;
        }
        else
        {
            reticle.enabled = false;
            gunPointer.enabled = false;
        }
	}

    #region Guns
    private void _CalculateAndMoveGunPointer(float brg, float dec)
    {
        swivel.eulerAngles = new Vector3(0, -brg, 0);
        pitcher.localEulerAngles = new Vector3(dec, 0, 0);

        gun.rotation = Quaternion.RotateTowards(gun.rotation, pitcher.GetChild(0).rotation, 85 * Time.deltaTime);
    }

    private void _FiringController(bool shoot, bool release)
    {
        gameModel.gunOn = shoot;
        if (shoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer - gameModel.t_SpinUpTime >= gameModel.t_TimeBetweenShots)
            {
                shootTimer -= gameModel.t_TimeBetweenShots;
                GameObject bullet;
                if (leftRightBarrel)
                    bullet = assetManager.Make(MyGameAsset.Bullet, player.position + Vector3.left * .1f);
                else
                    bullet = assetManager.Make(MyGameAsset.Bullet, player.position + Vector3.right * .1f);
                bullet.GetComponent<Rigidbody>().AddForce(gun.forward * 30, ForceMode.Impulse);
                leftRightBarrel = !leftRightBarrel;
                myAS.PlayOneShot(gameModel.gunShot);
            }
        }
        if (release)
            shootTimer = 0;
    }
    #endregion
}
