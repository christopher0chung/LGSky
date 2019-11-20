using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_PlayerShip : MonoBehaviour
{
    public GameObject shipModel;
    Model_Play playModel;
    Model_Game gameModel;

    PlayerState lastState;

    Renderer[] rends;

    private float timer;
    private float intervalCycle = .15f;
    private float goneTime = 2;

    public ParticleSystem worldParticles;

    void Start()
    {
        rends = shipModel.GetComponentsInChildren<Renderer>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }

    void Update()
    {
// ---------------------------Discrete
        if (playModel.currentPlayerState == PlayerState.Alive && lastState != PlayerState.Alive)
        {
            foreach(Renderer r in rends)
            {
                r.enabled = true;
            }
        }
        if (playModel.currentPlayerState != PlayerState.Dead && lastState == PlayerState.Dead)
        {
            foreach (Renderer r in rends)
            {
                r.enabled = false;
            }
        }
        if (playModel.currentPlayerState == PlayerState.Respawning && lastState != PlayerState.Respawning)
        {
            timer = 0;
        }
        if (playModel.currentPlayerState != PlayerState.GameOver && lastState == PlayerState.GameOver)
        {
            foreach (Renderer r in rends)
            {
                r.enabled = false;
            }
        }
        if (playModel.currentPlayerState == PlayerState.LevelVictory && lastState != PlayerState.LevelVictory)
        {
            foreach (Renderer r in rends)
            {
                r.enabled = true;
            }
        }
        if (playModel.currentPlayerState == PlayerState.Dash && lastState != PlayerState.Dash)
        {
            foreach (Renderer r in rends)
            {
                r.enabled = false;
            }
        }
        if (playModel.currentPlayerState == PlayerState.Flyby && lastState != PlayerState.Flyby)
        {
            foreach (Renderer r in rends)
            {
                r.enabled = false;
            }
        }
        else if (playModel.currentPlayerState != PlayerState.Flyby && lastState == PlayerState.Flyby)
        {
            var vel = worldParticles.velocityOverLifetime;
            vel.z = 0;
        }

        // ---------------------------Continuous
        if (playModel.currentPlayerState == PlayerState.Respawning)
        {
            timer += Time.deltaTime;

            if (timer <= goneTime)
                foreach (Renderer r in rends)
                    r.enabled = false;
            else
            {
                if (timer % intervalCycle < intervalCycle / 2)
                    //Context.player.gameObject.SetActive(false);
                    foreach (Renderer r in rends)
                        r.enabled = false;
                else
                    //Context.player.gameObject.SetActive(true);
                    foreach (Renderer r in rends)
                        r.enabled = true;
            }
        }
        else if (playModel.currentPlayerState == PlayerState.Flyby)
        {
            var vel = worldParticles.velocityOverLifetime;
            vel.z = playModel.worldSpeed_Current - gameModel.worldSpeed_min;
        }

        lastState = playModel.currentPlayerState;
    }
}
