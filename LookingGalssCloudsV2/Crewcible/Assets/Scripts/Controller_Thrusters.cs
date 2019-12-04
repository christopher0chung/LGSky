using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class Controller_Thrusters : SCG_Controller
{

    //Pilot is the only station controller that can write to the energy model

    //private AudioSource _myEngineAS;

    private Model_Game gameModel;
    //private Model_Energy energyModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    private Model_Heat heatModel;
    private Transform player;

    //public GameObject jumpReticleParent;
    private ParticleSystem dashParts;
    private MeshRenderer flyRet;
    private MeshRenderer dashRet;

    //public float xBoundClose;
    //public float xBoundFar;
    //public float zBoundClose;
    //public float zBoundFar;

    private Vector3 leftBoundVector;
    private Vector3 rightBoundVector;
    private Vector3 leftInVector;
    private Vector3 rightInVector;

    private float thrusterVolume;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        //energyModel = ServiceLocator.instance.Model.GetComponent<Model_Energy>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        player = ServiceLocator.instance.Player;

        dashParts = player.Find("Reticles").Find("ThrusterReticle_Swivel").Find("Dash Particle System").GetComponent<ParticleSystem>();
        dashRet = player.Find("Reticles").Find("ThrusterReticle_Swivel").Find("ThrusterReticles").Find("DashReticle").GetComponent<MeshRenderer>();
        flyRet = player.Find("Reticles").Find("ThrusterReticle_Swivel").Find("ThrusterReticles").Find("FlyReticle").GetComponent<MeshRenderer>();
    }

    void Start()
    {

        //_myEngineAS = MakeEngineAS();
        leftBoundVector = Vector3.Normalize(new Vector3(-gameModel.f_xBoundFar - -gameModel.f_xBoundClose, 0, gameModel.f_zBoundFar - gameModel.f_zBoundClose));
        leftInVector = Vector3.Cross(leftBoundVector, Vector3.up);

        rightBoundVector = Vector3.Normalize(new Vector3(gameModel.f_xBoundFar - gameModel.f_xBoundClose, 0, gameModel.f_zBoundFar - gameModel.f_zBoundClose));
        rightInVector = Vector3.Cross(rightBoundVector, Vector3.down);

        //jumpReticleParent.SetActive(false);
        flyRet.enabled = false;
        dashRet.enabled = false;

        priority = 5;
        Schedule(this);
    }

    public override void ScheduledUpdate()
    {
        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
            if (playModel.leftStation == Stations.Thrusters)
            {
                _Pilot(inputModel.L_X, inputModel.L_Y, inputModel.L_Action_OnDown);
            }
            else if (playModel.rightStation == Stations.Thrusters)
            {
                _Pilot(inputModel.R_X, inputModel.R_Y, inputModel.R_Action_OnDown);
            }
            else
            {
                //jumpReticleParent.SetActive(false);
                flyRet.enabled = false;
                dashRet.enabled = false;
                thrusterVolume = 0;
            }
        }
        else
        {
            //jumpReticleParent.SetActive(false);
            flyRet.enabled = false;
            dashRet.enabled = false;
            thrusterVolume = 0;
            //_myEngineAS.volume = 0;
        }
       // _myEngineAS.volume = Mathf.Lerp(_myEngineAS.volume, thrusterVolume, .1f);
    }

    #region Pilot
    Vector3 inputDirRaw;
    Vector3 inputDirNorm;
    Vector3 moveDir;
    float xBoundCalc;
    Vector3 limitPos;

    private void _Pilot(float inputX, float inputY, bool jump)
    {
        xBoundCalc = Mathf.Lerp(gameModel.f_xBoundClose, gameModel.f_xBoundFar, (player.position.z - gameModel.f_zBoundClose) / (gameModel.f_zBoundFar - gameModel.f_zBoundClose));

        inputDirRaw.x = inputX;
        inputDirRaw.z = inputY;

        inputDirNorm = Vector3.Normalize(inputDirRaw);

        if (Vector3.Magnitude(inputDirNorm) >= .05f && !heatModel.overheated_Thrusters)
        {
            heatModel.active_Thrusters = true;
            flyRet.enabled = true;
            //Debug.Log("Thruster input.");
        }
        else
        {
            heatModel.active_Thrusters = false;
            flyRet.enabled = false;
            //Debug.Log("No thruster input.");
        }

        if (player.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
            moveDir = inputDirRaw;
        else if (player.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, rightInVector) > 0)
            moveDir = inputDirRaw;

        else if (player.position.z <= gameModel.f_zBoundClose && Vector3.Dot(inputDirNorm, Vector3.forward) > 0)
            moveDir = inputDirRaw;
        else if (player.position.z >= gameModel.f_zBoundFar && Vector3.Dot(inputDirNorm, Vector3.back) > 0)
            moveDir = inputDirRaw;

        else moveDir = inputDirRaw;

        float moveMag = Vector3.Magnitude(moveDir);

        if (!heatModel.overheated_Thrusters)
        {
            player.position += moveDir * Time.deltaTime * gameModel.s_Thrusters_Speed;
        }

        float rot = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        flyRet.transform.rotation = Quaternion.Euler(-90, rot, 0);
        flyRet.transform.localScale = Vector3.one + Vector3.down + Vector3.up * moveMag;
        dashParts.transform.rotation = Quaternion.Euler(0, rot, 0);

        JumpReticleManage();
        JumpTimerManage();
        JumpCheck(jump);

        BoundarySet();

        player.position = limitPos;
    }

    void JumpReticleManage()
    {
        if (playModel.dashReloadProgress == 0)
            dashRet.enabled = true;
        else
            dashRet.enabled = false;

        dashRet.transform.position = player.position + moveDir * gameModel.f_Thrusters_DashDistance;
    }

    void JumpTimerManage()
    {
        playModel.dashReloadProgress -= Time.deltaTime;
        playModel.dashReloadProgress = Mathf.Clamp(playModel.dashReloadProgress, 0, gameModel.t_Thrusters_DashCooldown);
    }

    void JumpCheck(bool jump)
    {
        if (jump && playModel.dashReloadProgress == 0)
        {
            dashParts.Emit(100);

            player.position += moveDir * gameModel.f_Thrusters_DashDistance;
            playModel.dashReloadProgress = gameModel.t_Thrusters_DashCooldown;
            ServiceLocator.instance.SFX.PlayOneShot(gameModel.sfx_Thrusters_Dash);

            heatModel.active_Dash = true;
        }
        else
            heatModel.active_Dash = false;
    }

    void BoundarySet()
    {
        limitPos = player.position;
        if (player.position.x < -xBoundCalc)
            limitPos.x = -xBoundCalc;
        if (player.position.x > xBoundCalc)
            limitPos.x = xBoundCalc;
        if (player.position.z < gameModel.f_zBoundClose)
            limitPos.z = gameModel.f_zBoundClose;
        if (player.position.z > gameModel.f_zBoundFar)
            limitPos.z = gameModel.f_zBoundFar;
    }
    #endregion

    private AudioSource MakeEngineAS()
    {
        GameObject audioChild = new GameObject("AudioChild");
        audioChild.transform.SetParent(transform);
        AudioSource aSource = audioChild.AddComponent<AudioSource>();
        //aSource.clip = gameModel.pilotThruster;
        aSource.volume = 0;
        aSource.Play();
        aSource.loop = true;
        return aSource;
    }
}

