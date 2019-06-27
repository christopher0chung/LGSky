using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_PlayerTrail : SCG_View {

    private SCG_FSM<View_PlayerTrail> _fsm;

    public Model_Game gameModel;
    public Model_Play playModel;

    private LineRenderer line;
    private Transform tr;
    private Vector3[] positions;
    private Vector3[] directions;

    private int currentNumberOfPoints;
    private bool allPointsAdded;

    private Vector3 tempVec;

    private int numberOfPoints = 10;
    public float spread = .2f;

    public float spreadOverFramesFactor;

    void Start () {

        tr = transform;
        line = GetComponent<LineRenderer>();

        positions = new Vector3[numberOfPoints];
        directions = new Vector3[numberOfPoints];

        line.positionCount = currentNumberOfPoints;

        _fsm = new SCG_FSM<View_PlayerTrail>(this);
        _fsm.TransitionTo<Growing>();
	}

	void Update () {
        _fsm.Update();
	}

    Vector3 diffusionAndWorldTrail()
    {
        Vector3 smokeVec = Random.insideUnitSphere;
        smokeVec.z = 0;
        smokeVec *= spread;
        smokeVec.z = -gameModel.worldSpeed_fwd;
        return smokeVec;
    }

    #region States
    public class State_Base : SCG_FSM<View_PlayerTrail>.State
    {
        public void Grow()
        {
            // Add points until the target number is reached.

            Context.currentNumberOfPoints++;
            Context.line.positionCount = Context.currentNumberOfPoints;
            Context.tempVec = Context.diffusionAndWorldTrail();
            Context.directions[0] = Context.tempVec;
            Context.positions[0] = Context.tr.position;
            Context.line.SetPosition(0, Context.positions[0]);
        }

        public void Shrink()
        {
            Context.currentNumberOfPoints--;
            Context.line.positionCount = Context.currentNumberOfPoints;
        }

        public void PassDownChain()
        {
            // Make each point in the line take the position and direction of the one before it (effectively removing the last point from the line and adding a new one at transform position).
            for (int i = Context.currentNumberOfPoints - 1; i > 0; i--)
            {
                Context.tempVec = Context.positions[i - 1];
                Context.positions[i] = Context.tempVec;
                Context.tempVec = Context.directions[i - 1];
                Context.directions[i] = Context.tempVec;
            }
            Context.tempVec = Context.diffusionAndWorldTrail();
            Context.directions[0] = Context.tempVec; // Remember and give 0th point a direction for when it gets pulled up the chain in the next line update.
        }

        public void TrailPointsBehavior()
        {
            // Update the line...
            for (int i = 0; i < Context.currentNumberOfPoints; i++)
            {
                Context.tempVec = Context.positions[i];
                Context.directions[i].x *= Context.spreadOverFramesFactor;
                Context.directions[i].y *= Context.spreadOverFramesFactor;
                Context.tempVec += Context.directions[i] * Time.deltaTime;
                Context.positions[i] = Context.tempVec;

                Context.line.SetPosition(i, Context.positions[i]);
            }
        }

        public void AnchorZero()
        {
            Context.positions[0] = Context.tr.position; // 0th point is a special case, always follows the transform directly.
            Context.line.SetPosition(0, Context.tr.position);
        }
    }

    public class Growing : State_Base
    {
        public override void Update()
        {
            Grow();
            PassDownChain();
            TrailPointsBehavior();
            AnchorZero();
            if (Context.currentNumberOfPoints >= Context.numberOfPoints)
                TransitionTo<Working>();

            if (Context.playModel.currentPlayerState != PlayerState.Alive)
                TransitionTo<Shrinking>();
        }
    }

    public class Working : State_Base
    {
        public override void Update()
        {
            PassDownChain();
            TrailPointsBehavior();
            AnchorZero();

            if (Context.playModel.currentPlayerState != PlayerState.Alive)
                TransitionTo<Shrinking>();
        }
    }

    public class Shrinking : State_Base
    {
        public override void Update()
        {
            TrailPointsBehavior();
            Shrink();
            if (Context.currentNumberOfPoints <= 0)
                TransitionTo<Clear>();
        }
    }

    public class Clear : State_Base
    {
        public override void Update()
        {
            if (Context.playModel.currentPlayerState == PlayerState.Alive)
                TransitionTo<Growing>();
        }
    }
    #endregion
}
