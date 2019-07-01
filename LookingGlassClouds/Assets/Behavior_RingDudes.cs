using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_RingDudes : MonoBehaviour {

    SCG_FSM<Behavior_RingDudes> _fsm;
    Transform player;

    public int myIndex;
    public int ringMax;

    public float velocity;

    public float radius;
    public float height;
    public float rotationRate;

    void Awake()
    {
        player = ServiceLocator.instance.Player;
    }

    void Start () {
        if (_fsm == null)
            _fsm = new SCG_FSM<Behavior_RingDudes>(this);
        _fsm.TransitionTo<MoveToPos>();
	}
	
    public void Init(int index, int max, float height)
    {
        myIndex = index;
        ringMax = max;
        this.height = height;

        if (_fsm == null)
            _fsm = new SCG_FSM<Behavior_RingDudes>(this);
        _fsm.TransitionTo<MoveToPos>();
    }

	// Update is called once per frame
	void Update () {
        _fsm.Update();
        transform.rotation = Quaternion.Euler(0, Time.time * -100, 0);
    }

    public class State_Base : SCG_FSM<Behavior_RingDudes>.State
    {
        public Vector3 getRingPos()
        {
            return Context.player.position + Vector3.up * Context.height + Quaternion.Euler(0, (360 * (float)Context.myIndex + Time.time * Context.rotationRate) / (((float)Context.ringMax)), 0) * (Vector3.forward * Context.radius);
        }

        public Vector3 getSquishyRingPos()
        {
            return Context.player.position + Vector3.up * Context.height + Quaternion.Euler(0, (360 * (float)Context.myIndex + Time.time * Context.rotationRate) / (((float)Context.ringMax)), 0) * (Vector3.forward * (Context.radius + Mathf.Sin(Time.time * 2)));
        }
    }

    public class MoveToPos : State_Base
    {
        public override void Update()
        {
            Vector3 pos = getRingPos();

            Context.transform.position = Vector3.MoveTowards(Context.transform.position, pos, Context.velocity * Time.deltaTime);

            if (Vector3.Distance(Context.transform.position, pos) < 1)
                TransitionTo<Attack>();
        }
    }

    public class Attack : State_Base
    {
        public override void Update()
        {
            Context.transform.position = Vector3.MoveTowards(Context.transform.position,
                getSquishyRingPos(),
                Context.velocity * Time.deltaTime);
        }
    }
}
