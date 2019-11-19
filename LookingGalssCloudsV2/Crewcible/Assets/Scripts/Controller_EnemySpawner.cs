using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_EnemySpawner : MonoBehaviour
{
    public GameObject StraferPrefab;
    public GameObject SwarmPrefab;
    public GameObject PeakyPrefab;
    public GameObject SwooperPrefab;

    SCG_FSM<Controller_EnemySpawner> _fsm;

    Model_ScoreAndDifficulty sadModel;

    GameObject baddieParent;

    void Start()
    {
        sadModel = ServiceLocator.instance.Model.GetComponent<Model_ScoreAndDifficulty>();
        sadModel.score = 0;
        sadModel.level = 1;

        _fsm = new SCG_FSM<Controller_EnemySpawner>(this);
        TransitionToRandomState();
        baddieParent = new GameObject("BaddieParent");
        baddieParent.transform.SetParent(ServiceLocator.instance.Controller);

        SCG_EventManager.instance.Register<Event_Restart>(EventHandler);
    }

    void Update()
    {
        sadModel.difficulty = sadModel.level + (float)sadModel.score / 22000;
        sadModel.difficulty_Log = Mathf.Log(sadModel.difficulty) * 2;
        _fsm.Update();
    }

    #region Internal
    void Strafer(int num)
    {
        float angle = Random.Range(-20, 20);
        Quaternion rot = Quaternion.Euler(0, angle, 0);
        Vector3 moveDir = Vector3.Normalize(rot * Vector3.forward * 37 - Vector3.forward * 30);
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(StraferPrefab, 
                ServiceLocator.instance.Player.position - 
                    (70 + 15 * i) * moveDir + 
                    Vector3.up * Random.Range(20, 30)
                    + Vector3.right * Random.Range(-5, 5), 
                rot,
                baddieParent.transform);
            g.GetComponent<Enemy_Base>().SetHitPoint(101);
        }
    }

    void Swarm(int num)
    {
        GameObject g = Instantiate(SwarmPrefab, new Vector3(Random.Range(-150, 150), 20, 260), Quaternion.identity, baddieParent.transform);
        g.GetComponent<Test_Swarm>().Initialize(num);
    }

    void Peaky(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(PeakyPrefab, new Vector3(0, 0, 260), Quaternion.identity, baddieParent.transform);
            g.GetComponent<Enemy_Base>().SetHitPoint(10);
        }
    }

    void Swooper(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(SwooperPrefab, new Vector3(Random.Range(-50, 50), 0, 260), Quaternion.identity, baddieParent.transform);
            g.GetComponent<Enemy_Base>().SetHitPoint(1);
        }
    }
    #endregion

    void TransitionToRandomState()
    {
        int i = Random.Range(0, 4);

        if (i == 0)
            _fsm.TransitionTo<SwooperSpawn>();
        else if (i == 1)
            _fsm.TransitionTo<SwarmSpawn>();
        else if (i == 2)
            _fsm.TransitionTo<PeakySpawn>();
        else if (i == 3)
            _fsm.TransitionTo<StraferSpawn>();
    }

    public void EventHandler(SCG_Event e)
    {
        Event_Restart r = e as Event_Restart;
        if (r != null)
        {
            SCG_EventManager.instance.Fire(new Event_DumpReg());
            Destroy(baddieParent);
            baddieParent = new GameObject("BaddieParent");
            baddieParent.transform.SetParent(ServiceLocator.instance.Controller);
            sadModel.score = 0;
            sadModel.level = 1;
            _fsm.TransitionTo<Wait>();
        }
    }

    #region States
    public class State_Base : SCG_FSM<Controller_EnemySpawner>.State
    {

    }

    public class Wait : State_Base
    {
        float timer;
        float delay;

        float standard = 38;
        float subtractor = 1;

        public override void OnEnter()
        {
            timer = 0;
            delay = standard - 5 * Context.sadModel.difficulty;
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            //Debug.Log(timer);

            if (timer >= delay)
                Context.TransitionToRandomState();
        }
    }

    public class StraferSpawn : State_Base
    {
        float timer;
        public override void OnEnter()
        {
            timer = 0;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                Context.Strafer(Mathf.FloorToInt(Context.sadModel.difficulty_Log) + 2);
                TransitionTo<Wait>();
            }
        }

        public override void OnExit()
        {
            
        }
    }

    public class SwarmSpawn : State_Base
    {
        float timer;
        float delay;

        float min = .25f;
        float max = 3;

        public override void OnEnter()
        {
            // Announce SFX
            delay = Mathf.Lerp(min, max, 1 / Context.sadModel.difficulty);
            timer = 0;
        }
        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                // number of swarms goes up for every other point of difficulty
                // swarm count goes up by 5 for every point of difficulty
                for (int i = 0; i < Mathf.FloorToInt(Context.sadModel.difficulty_Log / 3) + 1; i++)
                {
                    Context.Swarm(Mathf.FloorToInt(30 + Context.sadModel.difficulty * 5));
                    //Debug.Log("New swarm of " + Mathf.FloorToInt(30 / (1 + Context.sadModel.difficulty_Log) + Context.sadModel.difficulty * 10));
                }

                TransitionTo<Wait>();
            }
        }
    }

    public class PeakySpawn : State_Base
    {
        float delayTimer;

        float spawnIntervalTimer;

        int counter;

        int max;

        float spawnInterval = .8f;

        public override void OnEnter()
        {
            delayTimer = 0;
            spawnIntervalTimer = 0;
            counter = 0;
            max = 2 + 1 * Mathf.FloorToInt(Context.sadModel.difficulty);
        }

        public override void Update()
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= 5)
            {
                spawnIntervalTimer += Time.deltaTime;

                if (spawnIntervalTimer > spawnInterval)
                {
                    spawnIntervalTimer -= spawnInterval;
                    Context.Peaky(1);
                    counter++;
                }
            }

            if (counter >= max)
                TransitionTo<Wait>();
        }
    }

    public class SwooperSpawn : State_Base
    {
        float timer;
        float duration = 3;

        int counter;
        int max;

        public override void OnEnter()
        {
            timer = 0;
            counter = 0;
            max = 8 + Mathf.FloorToInt(Context.sadModel.difficulty) * 2;
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= duration / max)
            {
                timer -= duration / max;
                counter++;
                Context.Swooper(1);
            }

            if (counter >= max)
                TransitionTo<Wait>();
        }
    }
    #endregion
}
