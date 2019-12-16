using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Heat : SCG_Controller
{
    Model_Heat heatModel;
    Model_Play playModel;
    Model_Game gameModel;

    //private List<Stations> _otherStations;
    private bool _last_Guns, _last_Lance, _last_Shield, _last_Thrusters, _last_Rockets;

    private void Awake()
    {
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }

    private void Start()
    {
        //_otherStations = new List<Stations>();
        _LastStatesUpdate();

        SCG_EventManager.instance.Register<Event_PlayerShieldBlock>(EventHandler);
        SCG_EventManager.instance.Register<Event_Restart>(EventHandler);
        SCG_EventManager.instance.Register<Event_Respawn>(EventHandler);
        
        priority = 6;
        Schedule(this);
    }

    public override void ScheduledUpdate()
    {
        //_PopulateOtherStations();
        _HeatCalcAndLockDown();
    }

    public override void ScheduledFixedUpdate()
    {
        _ApparentToActual();
    }

    #region Helper


    private void _HeatCalcAndLockDown()
    {
        heatModel.heat_Total = 0;

        _HeatCalc_Guns();
        _HeatCalc_Lance();
        _HeatCalc_Rockets();
        _HeatCalc_Shield();
        _HeatCalc_Thrusters();
        _HeatCalc_Total();

        _LastStatesUpdate();

        _AudioPrompts();
    }

    private void _ApparentToActual()
    {
        heatModel.heat_Guns_Apparent = Mathf.Lerp(heatModel.heat_Guns_Apparent, heatModel.heat_Guns, .08f);
        heatModel.heat_Lance_Apparent = Mathf.Lerp(heatModel.heat_Lance_Apparent, heatModel.heat_Lance, .08f);
        heatModel.heat_Shield_Apparent = Mathf.Lerp(heatModel.heat_Shield_Apparent, heatModel.heat_Shield, .08f);
        heatModel.heat_Thrusters_Apparent = Mathf.Lerp(heatModel.heat_Thrusters_Apparent, heatModel.heat_Thrusters, .08f);
        heatModel.heat_Rockets_Apparent = Mathf.Lerp(heatModel.heat_Rockets_Apparent, heatModel.heat_Rockets, .08f);

        heatModel.heat_Total_Apparent = Mathf.Lerp(heatModel.heat_Total_Apparent, heatModel.heat_Total, .08f);
    }
    #endregion

    #region Internal
    private void _HeatCalc_Guns()
    {
        // if not locked down, calculate how much to charge total
        if (!heatModel.overheated_Guns)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (playModel.unaccessedStations.Contains(Stations.Guns))
            {
                heatModel.heat_Guns -= heatModel.cooldownRate_Guns * Time.deltaTime;
                heatModel.heat_Guns = Mathf.Clamp(heatModel.heat_Guns, 0, 200);
            }
            // if accessed -
            //  when activated - heat goes up by activation cost + sustained
            //  when inactivated - heat goes down by cooldown
            else
            {
                if (heatModel.active_Guns)
                {
                    heatModel.heat_Guns += heatModel.sustainedHeatRate_Guns * Time.deltaTime;

                    if (heatModel.active_Guns && !_last_Guns)
                        heatModel.heat_Guns += heatModel.activationHeat_Guns;
                }
                else
                    heatModel.heat_Guns -= heatModel.cooldownRate_Guns * Time.deltaTime;

                //Clamping
                if (heatModel.heat_Guns >= 100)
                {
                    heatModel.heat_Guns = 100;
                    heatModel.overheated_Guns = true;
                }
                else if (heatModel.heat_Guns <= 0)
                    heatModel.heat_Guns = 0;
            }

            heatModel.heat_Total += heatModel.heat_Guns;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Guns -= heatModel.cooldownRate_Guns * Time.deltaTime;
            if (heatModel.heat_Guns <= heatModel.rebootThreshold_Guns)
            {
                heatModel.overheated_Guns = false;
                heatModel.heat_Total += heatModel.heat_Guns;
            }
            else
            {
                heatModel.heat_Total += 100;
            }
        }
    }

    private void _HeatCalc_Shield()
    {
        // if not locked down, calculate how much to charge total
        if (!heatModel.overheated_Shield)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (playModel.unaccessedStations.Contains(Stations.Shield))
            {
                heatModel.heat_Shield -= heatModel.cooldownRate_Shield * Time.deltaTime;
                heatModel.heat_Shield = Mathf.Clamp(heatModel.heat_Shield, 0, 200);
            }
            // if accessed -
            //  when activated - heat goes up by activation cost + sustained
            //  when inactivated - heat goes down by cooldown
            else
            {
                if (heatModel.active_Shield)
                {
                    heatModel.heat_Shield += heatModel.sustainedHeatRate_Shield * Time.deltaTime;

                    if (heatModel.active_Shield && !_last_Shield)
                        heatModel.heat_Shield += heatModel.activationHeat_Shield;
                }
                else
                    heatModel.heat_Shield -= heatModel.cooldownRate_Shield * Time.deltaTime;

                //Clamping
                if (heatModel.heat_Shield >= 100)
                {
                    heatModel.heat_Shield = 100;
                    heatModel.overheated_Shield = true;
                }
                else if (heatModel.heat_Shield <= 0)
                    heatModel.heat_Shield = 0;
            }

            heatModel.heat_Total += heatModel.heat_Shield;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Shield -= heatModel.cooldownRate_Shield * Time.deltaTime;
            if (heatModel.heat_Shield <= heatModel.rebootThreshold_Shield)
            {
                heatModel.overheated_Shield = false;
                heatModel.heat_Total += heatModel.heat_Shield;
            }
            else
            {
                heatModel.heat_Total += 100;
            }
        }
    }

    private void _HeatCalc_Lance()
    {
        // if not locked down, calculate how much to charge total
        if (!heatModel.overheated_Lance)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (playModel.unaccessedStations.Contains(Stations.Lance))
            {
                heatModel.heat_Lance -= heatModel.cooldownRate_Lance * Time.deltaTime;
                heatModel.heat_Lance = Mathf.Clamp(heatModel.heat_Lance, 0, 200);
            }
            // if accessed -
            //  when activated - heat goes up by activation cost + sustained
            //  when inactivated - heat goes down by cooldown
            else
            {
                if (heatModel.active_Lance)
                {
                    heatModel.heat_Lance += heatModel.sustainedHeatRate_Lance * Time.deltaTime;

                    if (heatModel.active_Lance && !_last_Lance)
                        heatModel.heat_Lance += heatModel.activationHeat_Lance;
                }
                else
                    heatModel.heat_Lance -= heatModel.cooldownRate_Lance * Time.deltaTime;

                //Clamping
                if (heatModel.heat_Lance >= 100)
                {
                    heatModel.heat_Lance = 100;
                    heatModel.overheated_Lance = true;
                }
                else if (heatModel.heat_Lance <= 0)
                    heatModel.heat_Lance = 0;
            }

            heatModel.heat_Total += heatModel.heat_Lance;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Lance -= heatModel.cooldownRate_Lance * Time.deltaTime;
            if (heatModel.heat_Lance <= heatModel.rebootThreshold_Lance)
            {
                heatModel.overheated_Lance = false;
                heatModel.heat_Total += heatModel.heat_Lance;
            }
            else
            {
                heatModel.heat_Total += 100;
            }
        }
    }

    private void _HeatCalc_Rockets()
    {
        // if not locked down, calculate how much to charge total
        if (!heatModel.overheated_Rockets)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (playModel.unaccessedStations.Contains(Stations.Rockets))
            {
                heatModel.heat_Rockets -= heatModel.cooldownRate_Rockets * Time.deltaTime;
                heatModel.heat_Rockets = Mathf.Clamp(heatModel.heat_Rockets, 0, 200);
            }
            // if accessed -
            //  when activated - heat goes up by activation cost + sustained
            //  when inactivated - heat goes down by cooldown
            else
            {
                if (heatModel.active_Rockets)
                {
                    heatModel.heat_Rockets += heatModel.sustainedHeatRate_Rockets * Time.deltaTime;

                    if (heatModel.active_Rockets && !_last_Rockets)
                        heatModel.heat_Rockets += heatModel.activationHeat_Rockets;

                    _last_Rockets = heatModel.active_Rockets;
                }
                else
                    heatModel.heat_Rockets -= heatModel.cooldownRate_Rockets * Time.deltaTime;

                //Clamping
                if (heatModel.heat_Rockets >= 100)
                {
                    heatModel.heat_Rockets = 100;
                    heatModel.overheated_Rockets = true;
                }
                else if (heatModel.heat_Rockets <= 0)
                    heatModel.heat_Rockets = 0;
            }

            heatModel.heat_Total += heatModel.heat_Rockets;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Rockets -= heatModel.cooldownRate_Rockets * Time.deltaTime;
            if (heatModel.heat_Rockets <= heatModel.rebootThreshold_Rockets)
            {
                heatModel.overheated_Rockets = false;
                heatModel.heat_Total += heatModel.heat_Rockets;
            }
            else
            {
                heatModel.heat_Total += 100;
            }
        }
    }

    private void _HeatCalc_Thrusters()
    {
        // if not locked down, calculate how much to charge total
        if (!heatModel.overheated_Thrusters)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (playModel.unaccessedStations.Contains(Stations.Thrusters))
            {
                heatModel.heat_Thrusters -= heatModel.cooldownRate_Thrusters * Time.deltaTime;
                heatModel.heat_Thrusters = Mathf.Clamp(heatModel.heat_Thrusters, 0, 200);
            }
            // if accessed -
            //  when activated - heat goes up by activation cost + sustained
            //  when inactivated - heat goes down by cooldown
            else
            {
                if (heatModel.active_Thrusters)
                {
                    heatModel.heat_Thrusters += heatModel.sustainedHeatRate_Thrusters * Time.deltaTime * playModel.thrusterVelPerc;

                    //if (heatModel.active_Thrusters && !_last_Thrusters)
                    //    heatModel.heat_Thrusters += heatModel.activationHeat_Thrusters;
                }
                else
                    heatModel.heat_Thrusters -= heatModel.cooldownRate_Thrusters * Time.deltaTime;

                if (heatModel.active_Dash)
                    heatModel.heat_Thrusters += heatModel.activationHeat_Thrusters;

                //Clamping
                if (heatModel.heat_Thrusters >= 100)
                {
                    heatModel.heat_Thrusters = 100;
                    heatModel.overheated_Thrusters = true;
                }
                else if (heatModel.heat_Thrusters <= 0)
                    heatModel.heat_Thrusters = 0;
            }

            heatModel.heat_Total += heatModel.heat_Thrusters;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Thrusters -= heatModel.cooldownRate_Thrusters * Time.deltaTime;
            if (heatModel.heat_Thrusters <= heatModel.rebootThreshold_Thrusters)
            {
                heatModel.overheated_Thrusters = false;
                heatModel.heat_Total += heatModel.heat_Thrusters;
            }
            else
            {
                heatModel.heat_Total += 100;
            }
        }
    }

    private void _HeatCalc_Total()
    {
        heatModel.heat_Total = Mathf.Clamp(heatModel.heat_Total, 0, heatModel.max_HeatTotal);
    }

    private void _LastStatesUpdate()
    {
        _last_Guns = heatModel.active_Guns;
        _last_Lance = heatModel.active_Lance;
        _last_Rockets = heatModel.active_Rockets;
        _last_Shield = heatModel.active_Shield;
        _last_Thrusters = heatModel.active_Shield;
    }

    public void EventHandler(SCG_Event e)
    {
        Event_PlayerShieldBlock psb = e as Event_PlayerShieldBlock;
        
        if (psb != null)
        {
            if (heatModel.active_Shield)
                heatModel.heat_Shield += gameModel.f_Shield_ActiveBlockPenalty;
        }

        Event_Restart restart = e as Event_Restart;
        Event_Respawn respawn = e as Event_Respawn;

        if (restart != null || respawn != null)
        {
            heatModel.heat_Guns = 0;
            heatModel.heat_Lance = 0;
            heatModel.heat_Rockets = 0;
            heatModel.heat_Shield = 0;
            heatModel.heat_Thrusters = 0;
        }
    }


    bool OH_GunsLast;
    bool OH_LanceLast;
    bool OH_RocketsLast;
    bool OH_ShieldLast;
    bool OH_ThrusterLast;

    bool LD_Port;
    bool LD_Stbd;

    float GunsLast;
    float LanceLast;
    float RocketsLast;
    float ShieldLast;
    float ThrusterLast;
    private void _AudioPrompts()
    {
        if (!heatModel.overheated_Guns && OH_GunsLast)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.O_Guns));
        if (!heatModel.overheated_Lance && OH_LanceLast)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.O_Lance));
        if (!heatModel.overheated_Rockets && OH_RocketsLast)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.O_Rockets));
        if (!heatModel.overheated_Shield && OH_ShieldLast)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.O_Shield));
        if (!heatModel.overheated_Thrusters && OH_ThrusterLast)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.O_Thrusters));

        if (playModel.leftStationLocked && !LD_Port)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.LD_Port));
        if (playModel.rightStationLocked && !LD_Stbd)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.LD_Stbd));

        if (heatModel.heat_Guns > 70 && GunsLast <= 70)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.W_Guns));
        if (heatModel.heat_Lance > 70 && LanceLast <= 70)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.W_Lance));
        if (heatModel.heat_Rockets > 70 && RocketsLast <= 70)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.W_Rockets));
        if (heatModel.heat_Shield > 70 && ShieldLast <= 70)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.W_Shield));
        if (heatModel.heat_Thrusters > 70 && ThrusterLast <= 70)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.W_Thrusters));

        OH_GunsLast = heatModel.overheated_Guns;
        OH_LanceLast = heatModel.overheated_Lance;
        OH_RocketsLast = heatModel.overheated_Rockets;
        OH_ShieldLast = heatModel.overheated_Shield;
        OH_ThrusterLast = heatModel.overheated_Thrusters;

        LD_Port = playModel.leftStationLocked;
        LD_Stbd = playModel.rightStationLocked;

        GunsLast = heatModel.heat_Guns;
        LanceLast = heatModel.heat_Lance;
        RocketsLast = heatModel.heat_Rockets;
        ShieldLast = heatModel.heat_Shield;
        ThrusterLast = heatModel.heat_Thrusters;

    }
    #endregion
}
