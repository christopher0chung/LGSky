using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_EnergySystems : MonoBehaviour {

    public Model_Game gameModel;
    public Model_Energy energyModel;

    [Header("Temp Tuning Var")]
    public float chargingScalar;

    SCG_FSM<Controller_EnergySystems> _fsm_left;
    SCG_FSM<Controller_EnergySystems> _fsm_right;

    private float _leftLoad;
    private float _rightLoad;

    private Stations _leftStationOld;
    private Stations _rightStationOld;

    void Awake()
    {
        SCG_EventManager.instance.Register<Event_EnemyBulletHit>(PlayerHitEventHandler);
        SCG_EventManager.instance.Register<Event_EnemyBulletBlock>(PlayerBlockEventHandler);
    }

    void Start()
    {
        _fsm_left = new SCG_FSM<Controller_EnergySystems>(this);
        _fsm_right = new SCG_FSM<Controller_EnergySystems>(this);
        _fsm_left.TransitionTo<Gun>();
        _fsm_right.TransitionTo<Shield>();
    }

    void Update()
    {
        StationSwapCheck();
        _fsm_left.Update();
        _fsm_right.Update();

        //Get Values
        OperationalReactorLoad();
        TransientOverTime();

        //Calculate and set
        OverloadCalculation();
        SetModel();

        ChargeJump();
        ApparentToActual();
    }

    #region Handlers
    void PlayerHitEventHandler(SCG_Event e)
    {
        Event_EnemyBulletHit eBH = e as Event_EnemyBulletHit;

        if (eBH != null)
        {
            energyModel.reactor_Transient -= 5;
        }
        transientTimer = 0;
    }

    void PlayerBlockEventHandler(SCG_Event e)
    {
        Event_EnemyBulletBlock eBB = e as Event_EnemyBulletBlock;

        if (eBB != null)
        {
            if (energyModel.shieldOn)
                energyModel.reactor_Transient += gameModel.e_BlockedShotEnergyHit_Boost;
            else
                energyModel.reactor_Transient += gameModel.e_BlockedShotEnergyHit_Base;
        }

        transientTimer = 0;
    }
    #endregion

    private float transientTimer;
    private float transientNormalizedTime;
    private float nTimePos;
    private float nTimeNeg;

    void TransientOverTime()
    {
        // the closer the 
        if (transientTimer == 0) // if timer is reset by an event on this frame, recalc new transient normalized time
        {
            if (energyModel.reactor_Transient <= 0) // positive transient
                transientNormalizedTime = Mathf.Clamp01((100 - energyModel.reactor_Actual) / 100) * 5;
            else                                    // negative transient
                transientNormalizedTime = Mathf.Clamp01(energyModel.reactor_Actual / 100) * 5;
        }

        transientTimer += Time.deltaTime / transientNormalizedTime;
        transientTimer = Mathf.Clamp01(transientTimer);

        energyModel.reactor_Transient = Mathf.Lerp(energyModel.reactor_Transient, 0, Easings.QuadraticEaseInOut(transientTimer));
    }

    float _tempReactorLoad;

    void OperationalReactorLoad()
    {
        _tempReactorLoad = gameModel.e_Reactor_Base;

        if (gameModel.leftStation == Stations.Guns || gameModel.rightStation == Stations.Guns)
            _tempReactorLoad -= energyModel.guns_Actual;
        if (gameModel.leftStation == Stations.Pilot || gameModel.rightStation == Stations.Pilot)
            _tempReactorLoad -= energyModel.pilot_Actual;
        if (gameModel.leftStation == Stations.Rockets || gameModel.rightStation == Stations.Rockets)
            _tempReactorLoad -= energyModel.rockets_Actual;
        if (gameModel.leftStation == Stations.Shield || gameModel.rightStation == Stations.Shield)
            _tempReactorLoad -= energyModel.shield_Actual;
        if (gameModel.leftStation == Stations.Sword || gameModel.rightStation == Stations.Sword)
            _tempReactorLoad -= energyModel.sword_Actual;
    }

    void OverloadCalculation()
    {
        Vector2 transientOverload = _TransientOverload(_tempReactorLoad, energyModel.reactor_Transient);
        energyModel.reactor_Transient = transientOverload.x;
        energyModel.jump_Actual += transientOverload.y;
    }

    void SetModel()
    {
        energyModel.reactor_Actual = _tempReactorLoad + energyModel.reactor_Transient;
    }

    void ChargeJump()
    {
        if (energyModel.reactor_Actual >= gameModel.e_ChargeJumpThreshold)
        {
            energyModel.jump_Actual += (energyModel.reactor_Actual - gameModel.e_ChargeJumpThreshold) * Time.deltaTime * chargingScalar;
        }
    }

    void ApparentToActual()
    {
        energyModel.guns_Apparent = Mathf.Lerp(energyModel.guns_Apparent, energyModel.guns_Actual, .09f);
        energyModel.jump_Apparent = Mathf.Lerp(energyModel.jump_Apparent, energyModel.jump_Actual, .09f);
        energyModel.pilot_Apparent = Mathf.Lerp(energyModel.pilot_Apparent, energyModel.pilot_Actual, .09f);
        energyModel.reactor_Apparent = Mathf.Lerp(energyModel.reactor_Apparent, energyModel.reactor_Actual, .09f);
        energyModel.rockets_Apparent = Mathf.Lerp(energyModel.rockets_Apparent, energyModel.rockets_Actual, .09f);
        energyModel.shield_Apparent = Mathf.Lerp(energyModel.shield_Apparent, energyModel.shield_Actual, .09f);
        energyModel.sword_Apparent = Mathf.Lerp(energyModel.sword_Apparent, energyModel.sword_Actual, .09f);
    }

    void StationSwapCheck()
    {
        if (gameModel.leftStation != _leftStationOld)
        {
            if (gameModel.leftStation == Stations.Guns)
                _fsm_left.TransitionTo<Gun>();
            else if (gameModel.leftStation == Stations.Pilot)
                _fsm_left.TransitionTo<Pilot>();
            else if (gameModel.leftStation == Stations.Rockets)
                _fsm_left.TransitionTo<Rockets>();
            else if (gameModel.leftStation == Stations.Shield)
                _fsm_left.TransitionTo<Shield>();
            else if (gameModel.leftStation == Stations.Sword)
                _fsm_left.TransitionTo<Sword>();
        }

        if (gameModel.rightStation != _rightStationOld)
        {
            if (gameModel.rightStation == Stations.Guns)
                _fsm_right.TransitionTo<Gun>();
            else if (gameModel.rightStation == Stations.Pilot)
                _fsm_right.TransitionTo<Pilot>();
            else if (gameModel.rightStation == Stations.Rockets)
                _fsm_right.TransitionTo<Rockets>();
            else if (gameModel.rightStation == Stations.Shield)
                _fsm_right.TransitionTo<Shield>();
            else if (gameModel.rightStation == Stations.Sword)
                _fsm_right.TransitionTo<Sword>();
        }

        _leftStationOld = gameModel.leftStation;
        _rightStationOld = gameModel.rightStation;
    }

    void GunCooldownWhenInactive()
    {
        if (gameModel.leftStation != Stations.Guns && gameModel.rightStation != Stations.Guns)
        {
            _CooldownGuns();
        }
    }

    private void _CooldownGuns()
    {
        energyModel.gun_OpCost -= Time.deltaTime * gameModel.e_Gun_Active * 4.5f;
        energyModel.gun_OpCost = Mathf.Clamp(energyModel.gun_OpCost, 0, 100 - gameModel.e_Gun_Passive);
    }

    private Vector2 _TransientOverload(float reactor, float transient)
    {
        // return.x is the new transient value that is clamped to not exceed reactor's available power
        // return.y is the amount in excess of reactor
        Vector2 toReturn;
        toReturn.x = transient;
        toReturn.y = 0;

        if (transient < 0 && Mathf.Abs(transient) > reactor)
        {
            // If the transient is negative and dips below the reactor's avaiable power
            toReturn.x = -reactor;                  // the value to set transient cannot exceed the present capacity
            toReturn.y = transient + reactor;       // the sum of the two will be equal to the negative transient in excess of the current capacity
        }
        else if (transient > 0 && transient + reactor > 100)
        {
            // If the transient is positive and would cause the total capacity to exceed the max (100)
            toReturn.x = 100 - reactor;             // the value to set transient so total equals max (100)
            toReturn.y = transient + reactor - 100; // the value above 100
        }

        return toReturn;
    }

    #region FSM

    protected class State_Base : SCG_FSM<Controller_EnergySystems>.State
    {

    }

    protected class Gun : State_Base
    {
        public override void OnEnter()
        {
            Context.energyModel.guns_Actual = Context.gameModel.e_Gun_Passive;
        }

        public override void Update()
        {
            if (Context.energyModel.gunsOn)
            {
                Context.energyModel.gun_OpCost += Time.deltaTime * Context.gameModel.e_Gun_Active;
            }
            else
            {
                Context._CooldownGuns();
            }
            Context.energyModel.guns_Actual = Context.gameModel.e_Gun_Passive + Context.energyModel.gun_OpCost;

        }

        public override void OnExit()
        {
            Context.energyModel.guns_Actual = 0;
        }
    }

    protected class Pilot : State_Base
    {
        private float jumpLoad;
        private float jumpTimer;
        private float jumpLerp;

        public override void OnEnter()
        {
            Context.energyModel.pilot_Actual = Context.gameModel.e_Pilot_Passive;
            jumpTimer = 100;
        }

        public override void Update()
        {
            jumpTimer += Time.deltaTime;
            jumpLerp = Mathf.Clamp01(jumpTimer / Context.gameModel.t_BoostCooldown);
            jumpLoad = Mathf.Lerp(Context.gameModel.e_Pilot_Boost, 0, jumpLerp);

            if (Context.energyModel.pilotOn)
            {
                Context.energyModel.pilot_Actual = Context.gameModel.e_Pilot_Passive + Context.gameModel.e_Flying + jumpLoad;
            }
            else
            {
                Context.energyModel.guns_Actual = Context.gameModel.e_Gun_Passive + jumpLoad;
            }
        }

        public override void OnExit()
        {
            Context.energyModel.pilot_Actual = 0;
        }
    }

    protected class Rockets : State_Base
    {
        private float rocketsLoad;
        private float rocketsTimer;
        private float rocketsLerp;

        public override void OnEnter()
        {
            Context.energyModel.rockets_Actual = Context.gameModel.e_Rockets_Passive;
        }

        public override void Update()
        {
            if (Context.energyModel.rocketsOn)
                rocketsTimer = 0;

            rocketsTimer += Time.deltaTime;
            rocketsLerp = Mathf.Clamp01(rocketsTimer / Context.gameModel.t_RocketCooldown);
            rocketsLoad = Mathf.Lerp(Context.gameModel.e_Rockets_Active, 0, rocketsLerp);

            Context.energyModel.rockets_Actual = Context.gameModel.e_Rockets_Passive + rocketsLoad;
        }

        public override void OnExit()
        {
            Context.energyModel.rockets_Actual = 0;
        }
    }

    private class Shield : State_Base
    {
        public override void OnEnter()
        {
            Context.energyModel.shield_Actual = Context.gameModel.e_Shield_Passive;
        }

        public override void Update()
        {
            if (Context.energyModel.shieldOn)
            {
                Context.energyModel.shield_Actual = Context.gameModel.e_Shield_Passive + Context.gameModel.e_Shield_Active;
            }
            else
            {
                Context.energyModel.shield_Actual = Context.gameModel.e_Shield_Passive;
            }
        }

        public override void OnExit()
        {
            Context.energyModel.shield_Actual = 0;
        }
    }

    private class Sword : State_Base
    {
        public override void OnEnter()
        {
            Context.energyModel.sword_Actual = Context.gameModel.e_SwordEnergyRate_Passive;
        }

        public override void Update()
        {
            if (Context.energyModel.swordOn)
            {
                Context.energyModel.sword_Actual = Context.gameModel.e_SwordEnergyRate_Passive + Context.gameModel.e_SwordEnergyRate_Active;
            }
            else
            {
                Context.energyModel.sword_Actual = Context.gameModel.e_SwordEnergyRate_Passive;
            }
        }

        public override void OnExit()
        {
            Context.energyModel.sword_Actual = 0;
        }
    }

    #endregion
}
