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

        ReactorLoad();

        ChargeJump();
        ApparentToActual();
    }

    void ReactorLoad()
    {
        float reactorLoad = gameModel.e_Reactor_Base;

        if (gameModel.leftStation == Stations.Guns || gameModel.rightStation == Stations.Guns)
            reactorLoad -= energyModel.guns_Actual;
        if (gameModel.leftStation == Stations.Pilot || gameModel.rightStation == Stations.Pilot)
            reactorLoad -= energyModel.pilot_Actual;
        if (gameModel.leftStation == Stations.Rockets || gameModel.rightStation == Stations.Rockets)
            reactorLoad -= energyModel.rockets_Actual;
        if (gameModel.leftStation == Stations.Shield || gameModel.rightStation == Stations.Shield)
            reactorLoad -= energyModel.shield_Actual;
        if (gameModel.leftStation == Stations.Sword || gameModel.rightStation == Stations.Sword)
            reactorLoad -= energyModel.sword_Actual;

        energyModel.reactor_Actual = reactorLoad;
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
        energyModel.gun_OpCost -= Time.deltaTime * gameModel.e_Gun_Active * 1.5f;
        energyModel.gun_OpCost = Mathf.Clamp(energyModel.gun_OpCost, 0, 100 - gameModel.e_Gun_Passive);
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
            if (Context.gameModel.gunOn)
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

            if (Context.gameModel.pilotOn)
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
            if (Context.gameModel.rocketsOn)
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
            if (Context.gameModel.shieldOn)
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
            if (Context.gameModel.swordOn)
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
