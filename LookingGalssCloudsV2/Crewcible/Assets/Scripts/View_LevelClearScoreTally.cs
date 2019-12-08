using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_LevelClearScoreTally : MonoBehaviour
{
    SCG_FSM<View_LevelClearScoreTally> _fsm;
    Model_ScoreAndDifficulty _sadModel;

    public Text label;
    public Text values;

    int deltaScore;
    int displayScore;

    // Start is called before the first frame update
    void Start()
    {
        _sadModel = ServiceLocator.instance.Model.GetComponent<Model_ScoreAndDifficulty>();
        _fsm = new SCG_FSM<View_LevelClearScoreTally>(this);
        _fsm.TransitionTo<Standby>();

        SCG_EventManager.instance.Register<Event_LevelClear>(EventHandler);
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Update();
    }

    public void EventHandler(SCG_Event e)
    {
        Event_LevelClear lc = e as Event_LevelClear;

        if (lc != null)
            _fsm.TransitionTo<Delay>();
    }

    public class State_Base : SCG_FSM<View_LevelClearScoreTally>.State
    {
    }

    public class Delay : State_Base
    {
        float timer;
        public override void OnEnter()
        {
            timer = 0;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer > .1f)
                TransitionTo<WaitToTally>();
        }
    }

    public class WaitToTally : State_Base
    {
        float timer;
        float waitTime = 1;
        public override void OnEnter()
        {
            base.OnEnter();
            timer = 0;

            Context.deltaScore = Context._sadModel.endScore - Context._sadModel.lastScore;
            Context.displayScore = Context._sadModel.lastScore;

            Context.label.enabled = true;
            Context.values.enabled = true;
            Context.label.color = Context.values.color = new Color(1, 1, 1, 1);
            Context.label.rectTransform.localScale = Context.values.rectTransform.localScale = Vector3.one;

            Context.values.text = Context._sadModel.lastScore.ToString() + "\n\n" + Context.deltaScore.ToString() + "\n\n" + Context.displayScore.ToString();
        }

        public override void Update()
        {
            base.Update();
            timer += Time.deltaTime;

            if (timer >= waitTime)
                TransitionTo<Tally>();
        }
    }

    public class Tally : State_Base
    {
        float tallyTimer;
        float tallyTime = 2.5f;
        float totalTime = 7;

        int interval;

        public override void OnEnter()
        {
            base.OnEnter();
            tallyTimer = 0;

            interval = Mathf.FloorToInt((float)Context.deltaScore * Time.deltaTime / tallyTime) + 1;
        }

        public override void Update()
        {
            tallyTimer += Time.deltaTime;
            Context.deltaScore -= interval;
            Context.deltaScore = Mathf.Clamp(Context.deltaScore, 0, Context._sadModel.score);
            Context.displayScore += interval;
            Context.displayScore = Mathf.Clamp(Context.displayScore, 0, Context._sadModel.score);

            Context.values.text = Context._sadModel.lastScore.ToString() + "\n\n" + Context.deltaScore.ToString() + "\n\n" + Context.displayScore.ToString();

            if (tallyTimer >= totalTime)
                TransitionTo<BlowOut>();
        }
    }

    public class BlowOut : State_Base
    {
        float alpha;
        float scale;
        float timer;
        public override void OnEnter()
        {
            base.OnEnter();
            timer = 0;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp01(timer);

            alpha = 1 - timer;
            alpha = Mathf.Clamp01(alpha);

            scale = Mathf.Lerp(1, 5, Easings.QuarticEaseIn(timer));

            Context.label.color = Context.values.color = new Color(1, 1, 1, alpha);
            Context.label.rectTransform.localScale = Context.values.rectTransform.localScale = Vector3.one * scale;

            if (timer >= 1)
                TransitionTo<Standby>();
        }
    }

    public class Standby : State_Base
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context.label.enabled = false;
            Context.values.enabled = false;
        }
    }
}
