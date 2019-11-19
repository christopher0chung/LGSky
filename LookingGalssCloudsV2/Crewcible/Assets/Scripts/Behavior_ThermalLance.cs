using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_ThermalLance : MonoBehaviour
{
    Model_Play playModel;
    Model_Game gameModel;

    private void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playModel.unaccessedStations.Contains(Stations.Lance) && (playModel.currentPlayerState == PlayerState.Alive || playModel.currentPlayerState == PlayerState.Respawning))
        {
            if (other.gameObject.tag == "Enemy")
            {
                Enemy_Base e = other.GetComponent<Enemy_Base>();

                SCG_EventManager.instance.Fire(new Event_BonusPoints(7));
                SCG_EventManager.instance.Fire(new Event_PlayerSwordHit(e, gameModel.d_Lance_Damage, other.transform.position));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playModel.unaccessedStations.Contains(Stations.Lance) && (playModel.currentPlayerState == PlayerState.Alive || playModel.currentPlayerState == PlayerState.Respawning))
        {
            if (other.gameObject.tag == "Enemy")
            {
                Enemy_Base e = other.GetComponent<Enemy_Base>();

                SCG_EventManager.instance.Fire(new Event_PlayerSwordHit(e, gameModel.d_Lance_Damage_Sustained * Time.deltaTime, other.transform.position));
            }
        }
    }
}
