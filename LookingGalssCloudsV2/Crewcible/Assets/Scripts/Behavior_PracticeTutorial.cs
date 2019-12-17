using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_PracticeTutorial : MonoBehaviour
{
    Model_Input inputModel;
    Model_Play playModel;

    SCG_FSM<Behavior_PracticeTutorial> _fsm;

    public GameObject[] sequences;
    public GameObject inputPrompt;
    public GameObject exitPrompt;

    public AudioClip boop;

    AudioSource myAS;

    int counter;
    void Start()
    {
        myAS = gameObject.AddComponent<AudioSource>();
        myAS.playOnAwake = false;
        myAS.loop = false;
        myAS.volume = .5f;
        myAS.clip = boop;

        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();

        _fsm = new SCG_FSM<Behavior_PracticeTutorial>(this);
        _fsm.TransitionTo<Begin>();
    }

    void Update()
    {
        if (playModel.isPaused)
        {
            inputPrompt.SetActive(false);
            exitPrompt.SetActive(false);
            foreach (GameObject g in sequences)
                g.SetActive(false);
        }
        else
            _fsm.Update();
    }

    public class State_Base : SCG_FSM<Behavior_PracticeTutorial>.State { }

    public class Begin : State_Base
    {
        float timer;

        public override void OnEnter()
        {
            Context.inputPrompt.SetActive(false);
            Context.exitPrompt.SetActive(false);
            
            foreach(GameObject g in Context.sequences)
            {
                g.SetActive(false);
            }

            Context.counter = 0;
        }

        public override void Update()
        {
            base.Update();

            timer += Time.deltaTime;
            if (timer >= 3)
                TransitionTo<Loading>();
        }
    }

    public class Stall : State_Base
    {
        float timer;
        public override void OnEnter()
        {
            timer = 0;

            Context.inputPrompt.SetActive(false);

            foreach (GameObject g in Context.sequences)
                g.SetActive(false);
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= .5f)
                TransitionTo<Loading>();
        }
    }

    public class Loading : State_Base
    {
        float timer;
        public override void OnEnter()
        {
            timer = 0;
            Context.myAS.Play();
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            Context.inputPrompt.SetActive(false);

            foreach (GameObject g in Context.sequences)
                g.SetActive(false);
            Context.sequences[Context.counter].SetActive(true);


            if (timer > 1.3f)
            {
                TransitionTo<StandbyForInput>();
            }
        }
    }

    public class StandbyForInput : State_Base
    {
        public override void OnEnter()
        {
            Context.inputPrompt.SetActive(true);
        }

        public override void Update()
        {
            Context.inputPrompt.SetActive(true);

            foreach (GameObject g in Context.sequences)
                g.SetActive(false);
            Context.sequences[Context.counter].SetActive(true);

            if (Context.inputModel.acknowledge)
            {
                Context.counter++;
                ServiceLocator.instance.SFX.PlayOneShot(ServiceLocator.instance.Model.GetComponent<Model_Game>().sfx_Shield_Block);
                if (Context.counter < Context.sequences.Length)
                    TransitionTo<Stall>();
                else
                    TransitionTo<Complete>();
            }
        }
    }

    public class Complete : State_Base
    {
        public override void OnEnter()
        {
            foreach (GameObject g in Context.sequences)
                g.SetActive(false);

            Context.inputPrompt.SetActive(false);

            Context.exitPrompt.SetActive(true);
        }
    }
}
