using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_RocketsAlt : MonoBehaviour
{
    private Manager_GameAssets assetManager;
    private Model_Game gameModel;
    private Model_Play playModel;
    private Transform rocketChild;

    public GameObject[] targetChoices;
    public Transform target;

    private void Awake()
    {
        //Restart();
    }

    public void Restart(Vector3 starting, Vector3 pointing)
    {
        if (assetManager == null)
            assetManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
        if (gameModel == null)
            gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        if (playModel == null)
            playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        if (rocketChild == null)
            rocketChild = transform.GetChild(0);

        transform.position = starting;
        transform.rotation = Quaternion.LookRotation(pointing);
        rocketChild.localPosition = Vector3.zero;
        rocketChild.localRotation = Quaternion.identity;

        AcquireTarget();
    }

    private void AcquireTarget()
    {
        targetChoices = GameObject.FindGameObjectsWithTag("Enemy");

        if (targetChoices != null)
            Debug.Log(targetChoices.Length);

        if (targetChoices != null && targetChoices.Length > 0)
            target = targetChoices[Random.Range(0, targetChoices.Length)].transform;
    }

    void Update()
    {
        if (target == null)
        {
            AcquireTarget();
        }

        if (target != null)
        {
            rocketChild.transform.rotation = Quaternion.RotateTowards(rocketChild.rotation, Quaternion.LookRotation(target.position - transform.position), gameModel.s_Rockets_TurnRate * Time.deltaTime);
            //rocketChild.transform.rotation = Quaternion.LookRotation(target.position - transform.position);
        }

        Debug.Log(gameModel.s_Rockets_TurnRate * Time.deltaTime);

        transform.position += rocketChild.forward * Time.deltaTime * gameModel.s_Rockets_FlySpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy_Base e = other.gameObject.GetComponent<Enemy_Base>();
        if (e != null)
        {
            //Debug.Log("Rocket trigger entered");
            SCG_EventManager.instance.Fire(new Event_BonusPoints(103));
            SCG_EventManager.instance.Fire(new Event_PlayerRocketHit(e, gameModel.d_Rockets_Damage, transform.position, this));
        }
    }
}