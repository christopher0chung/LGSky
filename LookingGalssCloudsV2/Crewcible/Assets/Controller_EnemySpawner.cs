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

    void Start()
    {
        sadModel = ServiceLocator.instance.Model.GetComponent<Model_ScoreAndDifficulty>();
        _fsm = new SCG_FSM<Controller_EnemySpawner>(this);
        _fsm.TransitionTo<PeakySpawn>();
    }

    void Update()
    {
        sadModel.difficulty = 1 + (float)sadModel.score / 22000;
        _fsm.Update();
    }

    #region Internal
    void Strafer(int num)
    {

    }

    void Swarm(int num)
    {
        GameObject g = Instantiate(SwarmPrefab, new Vector3(Random.Range(-150, 150), 20, 260), Quaternion.identity, transform);
        g.GetComponent<Test_Swarm>().Initialize(num);
    }

    void Peaky(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(PeakyPrefab, new Vector3(0, 0, 260), Quaternion.identity, transform);
            g.GetComponent<Enemy_Base>().SetHitPoint(15);
        }
    }

    void Swooper(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(SwooperPrefab, new Vector3(Random.Range(-50, 50), 0, 260), Quaternion.identity, transform);
            g.GetComponent<Enemy_Base>().SetHitPoint(1);
        }
    }
    #endregion

    #region States
    public class State_Base : SCG_FSM<Controller_EnemySpawner>.State
    {

    }

    public class Wait : State_Base
    {
        float timer;
        float delay;

        float standard = 45;
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
                TransitionTo<PeakySpawn>();
        }
    }

    public class StraferSpawn : State_Base
    {

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
                for (int i = 0; i <= Mathf.FloorToInt(Context.sadModel.difficulty / 2); i++)
                {
                    Context.Swarm(Mathf.FloorToInt(30 + Context.sadModel.difficulty * 5));
                    Debug.Log("New swarm of " + Mathf.FloorToInt(30 + Context.sadModel.difficulty * 10));
                }

                TransitionTo<Wait>();
            }
        }
    }

    public class PeakySpawn : State_Base
    {
        float timer;

        public override void OnEnter()
        {
            timer = 0;
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= 5)
            {
                Context.Peaky(2 + 1 * Mathf.FloorToInt(Context.sadModel.difficulty));
                TransitionTo<Wait>();
            }
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
            max = 8 + Mathf.FloorToInt(Context.sadModel.difficulty) * 1;
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= duration / max)
            {
                timer -= duration / max;
                counter++;
                Context.Swooper(Mathf.FloorToInt(Context.sadModel.difficulty));
            }

            if (counter >= max)
                TransitionTo<Wait>();
        }
    }
    #endregion
}
