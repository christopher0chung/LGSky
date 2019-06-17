using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Gun : MonoBehaviour {

    public Model_Game gameModel;
    public Model_Input inputModel;

    public GameObject bullet;
    public List<GameObject> _bullets;
    public List<float> _bulletTimes;

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
        _bullets = new List<GameObject>();
        _bulletTimes = new List<float>();
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

        _BulletsUpdate();
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
                if (leftRightBarrel)
                    _bullets.Add(Instantiate(bullet, player.position + Vector3.left * .1f, Quaternion.identity, null));
                else
                    _bullets.Add(Instantiate(bullet, player.position + Vector3.right * .1f, Quaternion.identity, null));
                leftRightBarrel = !leftRightBarrel;
                _bulletTimes.Add(0);
                _bullets[_bullets.Count - 1].GetComponent<Rigidbody>().AddForce(gun.forward * 30, ForceMode.Impulse);
                myAS.PlayOneShot(gameModel.gunShot);
            }
        }
        if (release)
            shootTimer = 0;
    }
    private void _BulletsUpdate()
    {
        for (int i = 0; i < _bulletTimes.Count; i++)
        {
            _bulletTimes[i] += Time.deltaTime;
            if (_bulletTimes[i] >= 2.2f)
            {
                _bulletTimes.RemoveAt(i);
                GameObject bulletToDestroy = _bullets[i];
                _bullets.RemoveAt(i);
                Destroy(bulletToDestroy);
            }
        }
    }
    #endregion
}
