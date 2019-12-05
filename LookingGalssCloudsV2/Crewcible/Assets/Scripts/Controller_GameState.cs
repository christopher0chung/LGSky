using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_GameState : SCG_Controller
{
    Model_Play playModel;
    Model_Input inputModel;
    Model_Game gameModel;
    Transform player;


    SCG_FSM<Controller_GameState> _fsm;

    public ParticleSystem preExplosion;
    public ParticleSystem explosion;

    //MeshRenderer[] renderers;

    public GameObject gameOver;
    public Material gameOverTextMat;

    public ParticleSystem charge1;
    public ParticleSystem charge2;
    public ParticleSystem jump1;
    public ParticleSystem jump2;
    void Start()
    {

        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        player = ServiceLocator.instance.Player;

        priority = 10;
        Schedule(this);

        playModel.lives = 3;
        gameOver.SetActive(false);

        _fsm = new SCG_FSM<Controller_GameState>(this);
        _fsm.TransitionTo<Alive>();

        charge1.Stop();
        charge2.Stop();
        jump1.Stop();
        jump2.Stop();

        Invoke("StartAudio", .5f);
    }

    private void StartAudio()
    {
        SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.Welcome));
    }

    // Update is called once per frame
    public override void ScheduledUpdate()
    {
        _fsm.Update();
    }

    public class State_Base : SCG_FSM<Controller_GameState>.State
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

    public class Starting : State_Base
    {
        public override void OnEnter()
        {
            Context.playModel.lives = 3;
            Context.gameOver.SetActive(false);
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.Welcome));
        }
        public override void Update()
        {
            TransitionTo<Respawning>();
        }
    }

    public class Alive : State_Base
    {
        public override void OnEnter()
        {
            Context.playModel.currentPlayerState = PlayerState.Alive;

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
            else if (Context.playModel.jumpTotal == 100)
            {
                TransitionTo<LevelVictory>();
            }
        }
    }

    public class Dead : State_Base
    {
        protected float timer;
        protected float dyingDuration = 3.9f;
        public override void OnEnter()
        {
            timer = 0;
            Context.playModel.currentPlayerState = PlayerState.Dead;

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
            Context.playModel.deathExplosionTrigger = true;
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.CriticalError));
        }
    }

    public class Respawning : State_Base
    {
        private float timer;
        private float respawnDuration = 4;

        public override void OnEnter()
        {
            timer = 0;
            Context.playModel.currentPlayerState = PlayerState.Respawning;
        }
        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= respawnDuration)
                TransitionTo<Alive>();
        }
    }

    public class GameOver : Dead
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context.playModel.currentPlayerState = PlayerState.GameOver;
            Context.gameOverTextMat.SetColor("_Emissive", Color.red * intensity);
            Context.gameOver.SetActive(true);
            intensity = 0;
        }

        private float intensity;

        private float restartTimer;

        public override void Update()
        {
            base.Update();
            intensity = Mathf.Lerp(intensity, 1.8f, .5f * Time.deltaTime);
            Context.gameOverTextMat.SetColor("_Emissive", Color.red * intensity);

            restartTimer += Time.unscaledDeltaTime;
            if (restartTimer >= 3)
            {
                if (Context.inputModel.startPause)
                    TransitionTo<Starting>();
            }
        }

        public override void GoToRespawn()
        {
            //if (timer >= dyingDuration)
            //    TransitionTo<State_Base>();   
        }

        public override void OnExit()
        {
            SCG_EventManager.instance.Fire(new Event_Restart());
            Context.preExplosion.Stop();
            Context.explosion.transform.position = Context.player.position;
            Context.explosion.Emit(250);
            Context.playModel.deathExplosionTrigger = true;
        }
    }

    public class LevelVictory : State_Base
    {
        //LevelVictory

        float chargeMax = 600;
        float chargeDuration = 6.3f;

        float currentChargeRate;
        float timer;
        public override void OnEnter()
        {
            timer = 0;
            currentChargeRate = 0;
            Context.charge1.Play();
            Context.charge2.gameObject.SetActive(true);
            Context.charge2.Play();
            Context.playModel.currentPlayerState = PlayerState.LevelVictory;
            Context.charge1.transform.position = Context.player.position;
            Context.charge2.transform.position = Context.player.position;

            SCG_EventManager.instance.Fire(new Event_LevelClear());
        }

        public override void Update()
        {
            timer += Time.unscaledDeltaTime / chargeDuration;

            var rate = Context.charge1.emission;
            rate.rateOverTime = chargeMax * timer;

            if (timer >= 1)
                TransitionTo<Dash>();
        }

        public override void OnExit()
        {
            var rate = Context.charge1.emission;
            rate.rateOverTime = 0;

            Context.charge2.Stop();
            Context.charge2.Clear();
            Context.charge2.gameObject.SetActive(false);
        }
    }

    public class Dash : State_Base
    {
        public override void OnEnter()
        {
            Context.jump1.transform.position = Context.player.position - Vector3.forward * 25;
            Context.jump2.transform.position = Context.player.position - Vector3.forward * 80;
            Context.jump1.Emit(550);
            Context.jump2.Emit(1000);
            timer = 0;
            //Renderer[] rs = ServiceLocator.instance.Player.GetComponentsInChildren<MeshRenderer>();
            //foreach (Renderer r in rs)
            //{
            //    r.enabled = false;
            //}
            Context.playModel.currentPlayerState = PlayerState.Dash;
        }

        float timer;

        public override void Update()
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= 10)
                TransitionTo<FastForward>();
        }
    }

    public class FastForward : State_Base
    {
        float timer;
        float duration = 10;
        public override void OnEnter()
        {
            timer = 0;
            Context.playModel.currentPlayerState = PlayerState.Flyby;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            Context.playModel.worldSpeed_Current = Context.gameModel.worldSpeed_min + Context.gameModel.worldSpeed_max * timer / duration;
            if (timer >= duration)
                TransitionTo<Alive>();
        }

        public override void OnExit()
        {
            Context.playModel.worldSpeed_Current = Context.gameModel.worldSpeed_min;
            ServiceLocator.instance.Model.GetComponent<Model_ScoreAndDifficulty>().level++;
            ServiceLocator.instance.View.GetComponent<View_ColorConfigurator>().SetColors(AestheticMode.Shift);
        }
    }
}
