using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Rockets : MonoBehaviour
{
    private Manager_GameAssets assetManager;
    public Model_Game gameModel;
    private Model_Play playModel;
    private Model_Input inputModel;
    private Transform rocketReticle;
    private Transform rocketChild;

    public float xAngTgt;
    public float yAngTgt;

    private float xAng;
    private float yAng;

    Quaternion nextRot;

    float timer;
    float rollover;

    float inputError;

    private void Awake()
    {
        Restart();
    }

    public void Restart()
    {
        if (assetManager == null)
            assetManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
        if (gameModel == null)
            gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        if (playModel == null)
            playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        if (rocketReticle == null)
            rocketReticle = ServiceLocator.instance.Player.Find("Reticles").Find("RocketReticle_Swivel").Find("RocketReticle_Pitcher");
        if (inputModel == null)
            inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        if (rocketChild == null)
            rocketChild = transform.GetChild(0);

        transform.position = ServiceLocator.instance.Player.position + Vector3.forward;
        transform.rotation = Quaternion.identity;
        rocketChild.localPosition = Vector3.zero;
        rocketChild.localRotation = Quaternion.identity;

        nextRot = Quaternion.Euler(Random.Range(-gameModel.f_Rockets_Spread, gameModel.f_Rockets_Spread), Random.Range(-gameModel.f_Rockets_Spread, gameModel.f_Rockets_Spread), Random.Range(-gameModel.f_Rockets_Spread, gameModel.f_Rockets_Spread));
        rollover = Random.Range(.3f, .5f);
        timer = Random.Range(0, rollover);
        inputError = Random.Range(.9f, 1.1f);

        xAngTgt = 0;
        xAng = 0;
        yAngTgt = 0;
        yAng = 0;
    }

    void Update()
    {

        if(playModel.leftStation == Stations.Rockets || playModel.rightStation == Stations.Rockets)
        {
            if (playModel.leftStation == Stations.Rockets)
            {
                xAngTgt = 60 * inputModel.L_Y * inputError;
                yAngTgt += gameModel.s_Rockets_TurnRate * inputModel.L_X * inputError * Time.deltaTime;
            }
            else
            {
                xAngTgt = 60 * inputModel.R_Y * inputError;
                yAngTgt += gameModel.s_Rockets_TurnRate * inputModel.R_X * inputError * Time.deltaTime;
            }
        }

        timer += Time.deltaTime;
        if (timer >= rollover)
        {
            timer -= rollover;
            rollover = Random.Range(.3f, .5f);
            nextRot = Quaternion.Euler(Random.Range(-gameModel.f_Rockets_Spread, gameModel.f_Rockets_Spread), Random.Range(-gameModel.f_Rockets_Spread, gameModel.f_Rockets_Spread), Random.Range(-gameModel.f_Rockets_Spread, gameModel.f_Rockets_Spread));
        }

        xAng = Mathf.Lerp(xAng, xAngTgt, .05f);
        yAng = Mathf.Lerp(yAng, yAngTgt, .05f);




        transform.rotation = Quaternion.Slerp(transform.rotation, nextRot, .05f);

        rocketChild.localRotation = Quaternion.Euler(xAng, yAng, 0);

        transform.position += rocketChild.forward * Time.deltaTime * gameModel.s_Rockets_FlySpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy_Base e = other.gameObject.GetComponent<Enemy_Base>();
        if (e != null)
        {
            Debug.Log("Rocket trigger entered");
            //SCG_EventManager.instance.Fire(new Event_PlayerRocketHit(e, gameModel.d_Rockets_Damage, transform.position, this));
        }
    }
}