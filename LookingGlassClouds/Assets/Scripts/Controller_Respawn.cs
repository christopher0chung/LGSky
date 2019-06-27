using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Controller_Respawn : MonoBehaviour {

    private Model_Play playModel;
    private Model_Energy energyModel;
    private Model_Game gameModel;

    private List<MeshRenderer> playerModelRenderers;

    private SCG_FSM<Controller_Respawn> _fsm;

    public ParticleSystem FlameOut;
    public ParticleSystem Explosion;

    private AudioSource _myAS;

    void Awake()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        energyModel = ServiceLocator.instance.Model.GetComponent<Model_Energy>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }

	void Start () {
        Transform player = ServiceLocator.instance.Player.Find("ShipModel3");

        _myAS = GetComponent<AudioSource>();

        playerModelRenderers = new List<MeshRenderer>();
        for (int i = 0; i < player.childCount; i++)
            playerModelRenderers.Add(player.GetChild(i).GetComponent<MeshRenderer>());

        _fsm = new SCG_FSM<Controller_Respawn>(this);
        _fsm.TransitionTo<Play>();
    }

	void Update () {
        _fsm.Update();
    }

    private class State_Base : SCG_FSM<Controller_Respawn>.State
    {
        public void DeadEnergy()
        {
            Context.energyModel.jump_Actual = 0;
            Context.energyModel.reactor_Actual = 0;
            Context.energyModel.guns_Actual = 0;
            Context.energyModel.rockets_Actual = 0;
            Context.energyModel.sword_Apparent = 0;
            Context.energyModel.shield_Actual = 0;
            Context.energyModel.pilot_Actual = 0;          
        }
    }

    private class Die : State_Base
    {
        private float deathTimer;
        public override void OnEnter()
        {
            Context.playModel.currentPlayerState = PlayerState.Dead;
            deathTimer = 0;
            Context.FlameOut.Play();
        }

        public override void Update()
        {
            DeadEnergy();

            deathTimer += Time.deltaTime;
            if (deathTimer >= 2)
                TransitionTo<Wait>();
        }

        public override void OnExit()
        {
            foreach (MeshRenderer m in Context.playerModelRenderers)
                m.enabled = false;

            Context.FlameOut.Stop();
            Context.Explosion.Emit(100);
            Context._myAS.PlayOneShot(Context.gameModel.shipExplodeSound);
        }
    }

    private class Wait : State_Base
    {
        private float waitTimer;

        public override void OnEnter()
        {
            Context.playModel.currentPlayerState = PlayerState.Dead;
            waitTimer = 0;
        }

        public override void Update()
        {
            DeadEnergy();

            waitTimer += Time.deltaTime;
            if (waitTimer >= 1 /* Add condition to assess how many player lives are left */)
                TransitionTo<Respawn>();
        }
    }

    private class Respawn : State_Base
    {
        private float respawnTimer;

        public override void OnEnter()
        {
            Context.playModel.currentPlayerState = PlayerState.Respawning;
            respawnTimer = 0;
            Context.energyModel.jump_Actual = 10;
            Context.energyModel.reactor_Transient = 0;
        }

        public override void Update()
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= 1)
                TransitionTo<RespawnInvincible>();
        }

        public override void OnExit()
        {
            foreach (MeshRenderer m in Context.playerModelRenderers)
                m.enabled = true;
        }
    }

    private class RespawnInvincible : State_Base
    {
        private float respawnInvincibleTimer;
        private float flickerTimer;

        private bool flicker;

        public override void OnEnter()
        {
            Context.playModel.currentPlayerState = PlayerState.Respawning;
            respawnInvincibleTimer = 0;
            flickerTimer = 0;
        }

        public override void Update()
        {
            respawnInvincibleTimer += Time.deltaTime;
            flickerTimer += Time.deltaTime;
            if (flickerTimer >= .05f)
            {
                flickerTimer -= .05f;

                flicker = !flicker;

                if (flicker)
                    foreach (MeshRenderer m in Context.playerModelRenderers)
                        m.enabled = true;
                else
                    foreach (MeshRenderer m in Context.playerModelRenderers)
                        m.enabled = false;
            }

            if (respawnInvincibleTimer >= .5f)
                TransitionTo<Play>();
        }

        public override void OnExit()
        {
            Context._myAS.PlayOneShot(Context.gameModel.respawnSound);

            foreach (MeshRenderer m in Context.playerModelRenderers)
                m.enabled = true;
        }
    }

    private class Play : State_Base
    {
        public override void OnEnter()
        {
            Context.playModel.currentPlayerState = PlayerState.Alive;
        }

        public override void Update()
        {
            if (Context.energyModel.jump_Actual <= 0)
            {
                TransitionTo<Die>();
            }
        }
    }
}
