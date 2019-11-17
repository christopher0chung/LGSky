using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Respawn : SCG_Controller
{
    Model_Play playModel;
    Transform player;

    SCG_FSM<Controller_Respawn> _fsm;

    public ParticleSystem preExplosion;
    public ParticleSystem explosion;

    MeshRenderer[] renderers;

    public GameObject gameOver;
    public Material gameOverTextMat;
    void Start()
    {
        _fsm = new SCG_FSM<Controller_Respawn>(this);
        _fsm.TransitionTo<Alive>();

        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        player = ServiceLocator.instance.Player;

        priority = 10;
        Schedule(this);

        renderers = player.GetComponentsInChildren<MeshRenderer>();

        playModel.lives = 1;
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    public override void ScheduledUpdate()
    {
        _fsm.Update();
    }

    public class State_Base : SCG_FSM<Controller_Respawn>.State
    {
        public override void OnEnter()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }

    public class Alive : State_Base
    {
        public override void OnEnter()
        {
            Context.playModel.currentPlayerState = PlayerState.Alive;
            //Context.player.gameObject.SetActive(true);
            foreach (MeshRenderer m in Context.renderers)
                m.enabled = true;
            Context.playModel.jumpTotal = 15;
        }

        public override void Update()
        {
            if (Context.playModel.jumpTotal <= 0.5f)
            {
                Context.playModel.lives--;
                if (Context.playModel.lives > 0)
                    TransitionTo<Dead>();
                else
                    TransitionTo<GameOver>();
            }
        }
    }

    public class Dead : State_Base
    {
        protected float timer;
        protected float dyingDuration = 2.9f;
        public override void OnEnter()
        {
            timer = 0;
            Context.playModel.currentPlayerState = PlayerState.Dead;
            //Context.player.gameObject.SetActive(false);
            foreach (MeshRenderer m in Context.renderers)
                m.enabled = false;
            Context.preExplosion.transform.position = Context.player.position;
            Context.preExplosion.Play();
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer <= dyingDuration)
            {
                Context.preExplosion.transform.position = Context.player.position;
            }

            GoToRespawn();
        }

        public virtual void GoToRespawn()
        {
            if (timer >= dyingDuration)
                TransitionTo<Respawning>();
        }

        public override void OnExit()
        {
            Context.preExplosion.Stop();
            Context.explosion.transform.position = Context.player.position;
            Context.explosion.Emit(150);
        }
    }

    public class Respawning : State_Base
    {
        private float timer;
        private float intervalCycle = .15f;
        private float respawnDuration = 5;
        private float goneTime = 2;

        public override void OnEnter()
        {
            timer = 0;
            Context.playModel.currentPlayerState = PlayerState.Respawning;
        }
        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer <= goneTime)
                //Context.player.gameObject.SetActive(false);
                foreach (MeshRenderer m in Context.renderers)
                    m.enabled = false;
            else
            {
                if (timer % intervalCycle < intervalCycle / 2)
                    //Context.player.gameObject.SetActive(false);
                    foreach (MeshRenderer m in Context.renderers)
                        m.enabled = false;
                else
                    //Context.player.gameObject.SetActive(true);
                    foreach (MeshRenderer m in Context.renderers)
                        m.enabled = true;
            }

            if (timer >= respawnDuration)
                TransitionTo<Alive>();
        }
    }

    public class GameOver : Dead
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context.gameOverTextMat.SetColor("_Emissive", Color.red * intensity);
            Context.gameOver.SetActive(true);
            intensity = 0;
        }

        private float intensity;

        public override void Update()
        {
            base.Update();
            Time.timeScale = Mathf.Lerp(Time.timeScale, .09f, 6 * Time.deltaTime);
            intensity = Mathf.Lerp(intensity, 1.8f, .5f * Time.deltaTime);
            Context.gameOverTextMat.SetColor("_Emissive", Color.red * intensity);
            Debug.Log(Time.timeScale);
        }

        public override void GoToRespawn()
        {
            if (timer >= dyingDuration)
                TransitionTo<State_Base>();   
        }
    }

    public class LevelVictory : State_Base
    {

    }
}
