using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Controller_Pilot : SCG_Controller {

    //Pilot is the only station controller that can write to the energy model

    private AudioSource _myAS;
    private AudioSource _myEngineAS;

    private Model_Game gameModel;
    private Model_Energy energyModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    private Transform player;

    public GameObject jumpReticleParent;
    public Behavior_PilotJumpParticles jumpParts;

    public float xBoundClose;
    public float xBoundFar;
    public float zBoundClose;
    public float zBoundFar;

    private Vector3 leftBoundVector;
    private Vector3 rightBoundVector;
    private Vector3 leftInVector;
    private Vector3 rightInVector;

    private float thrusterVolume;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        energyModel = ServiceLocator.instance.Model.GetComponent<Model_Energy>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        player = ServiceLocator.instance.Player;
    }

    void Start() {

        _myAS = GetComponent<AudioSource>();
        _myEngineAS = MakeEngineAS();
        leftBoundVector = Vector3.Normalize(new Vector3(-xBoundFar - -xBoundClose, 0, zBoundFar - zBoundClose));
        leftInVector = Vector3.Cross(leftBoundVector, Vector3.up);

        rightBoundVector = Vector3.Normalize(new Vector3(xBoundFar - xBoundClose, 0, zBoundFar - zBoundClose));
        rightInVector = Vector3.Cross(rightBoundVector, Vector3.down);

        jumpReticleParent.SetActive(false);
    }

    void Update() {
        if (playModel.currentPlayerState == PlayerState.Alive)
        {
            if (gameModel.leftStation == Stations.Pilot)
            {
                _Pilot(inputModel.L_X, inputModel.L_Y, inputModel.L_Action_OnDown);
            }
            else if (gameModel.rightStation == Stations.Pilot)
            {
                _Pilot(inputModel.R_X, inputModel.R_Y, inputModel.R_Action_OnDown);
            }
            else
            {
                jumpReticleParent.SetActive(false);
                thrusterVolume = 0;
            }
        }
        else
        {
            jumpReticleParent.SetActive(false);
            thrusterVolume = 0;
            _myEngineAS.volume = 0;
        }


        _myEngineAS.volume = Mathf.Lerp(_myEngineAS.volume, thrusterVolume, .1f);

    }

    #region Pilot
    Vector3 inputDirRaw;
    Vector3 inputDirNorm;
    Vector3 moveDir;
    float xBoundCalc;
    Vector3 limitPos;

    private void _Pilot(float inputX, float inputY, bool jump)
    {
        xBoundCalc = Mathf.Lerp(xBoundClose, xBoundFar, (player.position.z - zBoundClose) / (zBoundFar - zBoundClose));

        inputDirRaw.x = inputX;
        inputDirRaw.z = inputY;

        inputDirNorm = Vector3.Normalize(inputDirRaw);

        if (Vector3.Magnitude(inputDirNorm) >= .05f)
            energyModel.pilotOn = true;
        else
            energyModel.pilotOn = false;

        if (player.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
            moveDir = inputDirRaw;
        else if (player.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
            moveDir = inputDirRaw;

        else if (player.position.z <= zBoundClose && Vector3.Dot(inputDirNorm, Vector3.forward) > 0)
            moveDir = inputDirRaw;
        else if (player.position.z >= zBoundFar && Vector3.Dot(inputDirNorm, Vector3.back) > 0)
            moveDir = inputDirRaw;

        else moveDir = inputDirRaw;

        float moveMag = Vector3.Magnitude(moveDir);
        playModel.pilot_flyMag = moveMag;
        thrusterVolume = moveMag;

        player.position += moveDir * Time.deltaTime * gameModel.flySpeed;



        JumpReticleManage();
        JumpTimerManage();
        JumpCheck(jump);

        BoundarySet();

        player.position = limitPos;
    }

    void JumpReticleManage()
    {
        if (energyModel.pilot_JumpCooldownTimeRemaining == 0)
            jumpReticleParent.SetActive(true);
        else
            jumpReticleParent.SetActive(false);

        jumpReticleParent.transform.position = player.position + moveDir * gameModel.boostDist;
    }

    void JumpTimerManage()
    {
        energyModel.pilot_JumpCooldownTimeRemaining -= Time.deltaTime;
        energyModel.pilot_JumpCooldownTimeRemaining = Mathf.Clamp(energyModel.pilot_JumpCooldownTimeRemaining, 0, gameModel.t_BoostCooldown);
    }

    void JumpCheck(bool jump)
    {
        if (jump && energyModel.pilot_JumpCooldownTimeRemaining == 0)
        {
            jumpParts.Play(player.position, moveDir);

            player.position += moveDir * gameModel.boostDist;
            energyModel.pilot_JumpCooldownTimeRemaining = gameModel.t_BoostCooldown;
            energyModel.pilot_JumpOpCost += gameModel.e_Pilot_Boost;
            _myAS.PlayOneShot(gameModel.pilotBoost);
        }
    }

    void BoundarySet()
    {
        limitPos = player.position;
        if (player.position.x < -xBoundCalc)
            limitPos.x = -xBoundCalc;
        if (player.position.x > xBoundCalc)
            limitPos.x = xBoundCalc;
        if (player.position.z < zBoundClose)
            limitPos.z = zBoundClose;
        if (player.position.z > zBoundFar)
            limitPos.z = zBoundFar;
    }
    #endregion

    private AudioSource MakeEngineAS()
    {
        GameObject audioChild = new GameObject("AudioChild");
        audioChild.transform.SetParent(transform);
        AudioSource aSource = audioChild.AddComponent<AudioSource>();
        aSource.clip = gameModel.pilotThruster;
        aSource.volume = 0;
        aSource.Play();
        aSource.loop = true;
        return aSource;
    }
}
