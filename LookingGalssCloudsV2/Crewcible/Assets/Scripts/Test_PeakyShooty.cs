using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PeakyShooty : Behavior_BaddyBase
{
    Transform target;

    Vector3 oldPos;
    Vector3 newPos;

    public float interval;
    public float bobRate;

    private float _lastTime;
    private float _tOffset;

    Transform shield;

    [Range(0, 1)] public float shieldFraction;

    public GameObject bullet;
    public GameObject flameout;

    Enemy_Base myE;

    ParticleSystem damageInd;

    SCG_FSM<Test_PeakyShooty> _fsm;

    float effectsTimer;

    // Start is called before the first frame update
    void Start()
    {
        GrabStdRefs();

        myE = GetComponent<Enemy_Base>();
        target = ServiceLocator.instance.Player;

        oldPos = transform.position;
        newPos = target.position + 
            Vector3.forward * Random.Range(7, 40) + 
            Vector3.right * Random.Range(-33, 33) + 
            Vector3.up * Random.Range(5, 33);

        _tOffset = Random.Range(0.00f, 10.00f);

        shield = transform.Find("PeakyShield");

        SCG_EventManager.instance.Register<Event_EnemyDeath>(EnemyDeathHandler);
        SCG_EventManager.instance.Register<Event_DumpReg>(EnemyDeathHandler);
        
        damageInd = GetComponentInChildren<ParticleSystem>();

        _fsm = new SCG_FSM<Test_PeakyShooty>(this);
        _fsm.TransitionTo<Approach>();
    }

    // Update is called once per frame
    void Update()
    {
        effectsTimer += Time.deltaTime;

        _fsm.Update();

        var rate = damageInd.emission;
        rate.rateOverTime = 50 * ((myE.hitpoints_Max - myE.hitpoints_Current) / myE.hitpoints_Max);
    }

    public void EnemyDeathHandler(SCG_Event e)
    {
        Event_EnemyDeath ed = e as Event_EnemyDeath;
        if (ed != null)
        {

            if (ed.enemyToBeDestroyed == myE)
            {
                SCG_EventManager.instance.Fire(new Event_BonusPoints(161));
                SCG_EventManager.instance.Unregister<Event_DumpReg>(EnemyDeathHandler);
                SCG_EventManager.instance.Unregister<Event_EnemyDeath>(EnemyDeathHandler);
                Instantiate(flameout, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }

        Event_DumpReg d = e as Event_DumpReg;
        if (d != null)
        {
            SCG_EventManager.instance.Unregister<Event_EnemyDeath>(EnemyDeathHandler);
            SCG_EventManager.instance.Unregister<Event_DumpReg>(EnemyDeathHandler);
        }
    }

    public class State_Base : SCG_FSM<Test_PeakyShooty>.State
    {

    }

    public class Approach : State_Base
    {
        float timer;
        Vector3 oldPos;
        Vector3 newPos;
        public override void OnEnter()
        {
            timer = 0;
            oldPos = Context.transform.position;
            newPos = Context.target.position +
                Vector3.forward * Random.Range(7, 40) +
                Vector3.right * Random.Range(-33, 33) +
                Vector3.up * Random.Range(5, 33);
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            oldPos += Context.WorldEffectOffset();
            newPos += Context.WorldEffectOffset();

            Context.transform.position = Vector3.Lerp(oldPos, newPos, Easings.QuinticEaseIn(timer / 7)) + 
                Vector3.up * 1.3f * Mathf.Sin(Context.effectsTimer * Context.bobRate + Context._tOffset);
            Context.transform.LookAt(Context.target.position);
            Context.shield.localPosition = new Vector3(0, 0, 1.16f);
            Context.shield.localRotation = Quaternion.identity;

            if (timer >= 7 && Context.playModel.currentPlayerState != PlayerState.LevelVictory)
                TransitionTo<OnStation>();
            if (Context.transform.position.z <= -100)
            {
                SCG_EventManager.instance.Unregister<Event_DumpReg>(Context.EnemyDeathHandler);
                SCG_EventManager.instance.Unregister<Event_EnemyDeath>(Context.EnemyDeathHandler);
                Destroy(Context.gameObject);
            }
        }

        public override void OnExit()
        {
            
        }
    }

    public class OnStation : State_Base
    {
        public Vector3 oldPos;
        Vector3 newPos;

        float timer;

        int counter;

        float oldT;
        public override void OnEnter()
        {
            if (counter == 0)
                oldPos = Context.transform.position;
            else
                oldPos = newPos;
            newPos = Context.target.position + Vector3.forward * Random.Range(7, 40) + Vector3.right * Random.Range(-33, 33) + Vector3.up * Random.Range(5, 33);

            timer = 0;

            counter++;
        }

        public override void Update()
        {
            timer += Time.deltaTime / Context.interval;

            oldPos += Context.WorldEffectOffset();
            newPos += Context.WorldEffectOffset();

            Context.transform.position = 
                Vector3.Lerp(oldPos, newPos, Easings.QuinticEaseInOut(timer)) + 
                Vector3.up * 1.3f * Mathf.Sin(Context.effectsTimer * Context.bobRate + Context._tOffset);

            if (timer <= Context.shieldFraction)
            {
                float shieldT = timer  / Context.shieldFraction;

                Context.shield.localPosition = Vector3.right * 4 * Mathf.Sin(shieldT * Mathf.PI) + Vector3.forward * 1.16f;
                Context.shield.localRotation = Quaternion.identity;

                for (int i = 0; i < 2; i++)
                {
                    if (shieldT >= .3f + .07f * i && oldT < .3f + .07f * i)
                        if (Context.playModel.currentPlayerState == PlayerState.Alive || Context.playModel.currentPlayerState == PlayerState.LevelVictory)
                            Instantiate(Context.bullet, Context.transform.position + Context.transform.right * -2.67f + Context.transform.forward * 3.45f, Quaternion.LookRotation(Context.transform.forward));
                }

                oldT = shieldT;   
            }

            Context.transform.LookAt(Context.target.position);

            if (timer >= 1 && Context.playModel.currentPlayerState != PlayerState.LevelVictory)
                TransitionTo<Rebound>();
            if (Context.transform.position.z <= -100)
            {
                SCG_EventManager.instance.Unregister<Event_DumpReg>(Context.EnemyDeathHandler);
                SCG_EventManager.instance.Unregister<Event_EnemyDeath>(Context.EnemyDeathHandler);
                Destroy(Context.gameObject);
            }
        }

        public override void OnExit()
        {
            
        }
    }

    class Rebound : State_Base
    {
        public override void Update()
        {
            TransitionTo<OnStation>();
        }
    }
}
