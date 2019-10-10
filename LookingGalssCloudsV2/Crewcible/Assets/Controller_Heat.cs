﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Controller_Heat : SCG_Controller
{
    public Model_Heat heatModel;
    public Model_Play playModel;

    private List<Stations> _otherStations;
    private bool _last_Guns, _last_Lance, _last_Shield, _last_Thrusters, _last_Rockets;

    private void Awake()
    {
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    private void Start()
    {
        _otherStations = new List<Stations>();
        _lastStatesUpdate();

        priority = 6;
        Schedule(this);
    }

    public override void ScheduledUpdate()
    {
        _PopulateOtherStations();
        _HeatCalcAndLockDown();
    }

    public override void ScheduledFixedUpdate()
    {
        _ApparentToActual();
    }

    #region Helper
    private void _PopulateOtherStations()
    {
        _otherStations.Clear();
        _otherStations = Enum.GetValues(typeof(Stations)).OfType<Stations>().ToList<Stations>();

        _otherStations.Remove(playModel.leftStation);
        _otherStations.Remove(playModel.rightStation);
    }

    private void _HeatCalcAndLockDown()
    {
        heatModel.heat_Total = 0;

        _HeatCalc_Guns();
        _HeatCalc_Lance();
        _HeatCalc_Rockets();
        _HeatCalc_Shield();
        _HeatCalc_Thrusters();

        _lastStatesUpdate();
    }

    private void _ApparentToActual()
    {
        heatModel.heat_Guns_Apparent = Mathf.Lerp(heatModel.heat_Guns_Apparent, heatModel.heat_Guns, .08f);
        heatModel.heat_Lance_Apparent = Mathf.Lerp(heatModel.heat_Lance_Apparent, heatModel.heat_Lance, .08f);
        heatModel.heat_Shield_Apparent = Mathf.Lerp(heatModel.heat_Shield_Apparent, heatModel.heat_Shield, .08f);
        heatModel.heat_Thrusters_Apparent = Mathf.Lerp(heatModel.heat_Thrusters_Apparent, heatModel.heat_Thrusters, .08f);
        heatModel.heat_Rockets_Apparent = Mathf.Lerp(heatModel.heat_Rockets_Apparent, heatModel.heat_Rockets, .08f);

    }
    #endregion

    #region Internal
    private void _HeatCalc_Guns()
    {
        // if not locked down, calculate how much to charge total
        if (!heatModel.lockedDown_Guns)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (_otherStations.Contains(Stations.Guns))
                heatModel.heat_Guns -= heatModel.cooldownRate_Guns;
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
                    heatModel.heat_Guns -= heatModel.cooldownRate_Guns;

                //Clamping
                if (heatModel.heat_Guns >= 100)
                {
                    heatModel.heat_Guns = 100;
                    heatModel.lockedDown_Guns = true;
                }
                else if (heatModel.heat_Guns <= 0)
                    heatModel.heat_Guns = 0;
            }

            heatModel.heat_Total += heatModel.heat_Guns;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Guns -= heatModel.cooldownRate_Guns;
            if (heatModel.heat_Guns <= heatModel.rebootThreshold_Guns)
            {
                heatModel.lockedDown_Guns = false;
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
        if (!heatModel.lockedDown_Shield)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (_otherStations.Contains(Stations.Shield))
                heatModel.heat_Shield -= heatModel.cooldownRate_Shield;
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
                    heatModel.heat_Shield -= heatModel.cooldownRate_Shield;

                //Clamping
                if (heatModel.heat_Shield >= 100)
                {
                    heatModel.heat_Shield = 100;
                    heatModel.lockedDown_Shield = true;
                }
                else if (heatModel.heat_Shield <= 0)
                    heatModel.heat_Shield = 0;
            }

            heatModel.heat_Total += heatModel.heat_Shield;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Shield -= heatModel.cooldownRate_Shield;
            if (heatModel.heat_Shield <= heatModel.rebootThreshold_Shield)
            {
                heatModel.lockedDown_Shield = false;
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
        if (!heatModel.lockedDown_Lance)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (_otherStations.Contains(Stations.Lance))
                heatModel.heat_Lance -= heatModel.cooldownRate_Lance;
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
                    heatModel.heat_Lance -= heatModel.cooldownRate_Lance;

                //Clamping
                if (heatModel.heat_Lance >= 100)
                {
                    heatModel.heat_Lance = 100;
                    heatModel.lockedDown_Lance = true;
                }
                else if (heatModel.heat_Lance <= 0)
                    heatModel.heat_Lance = 0;
            }

            heatModel.heat_Total += heatModel.heat_Lance;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Lance -= heatModel.cooldownRate_Lance;
            if (heatModel.heat_Lance <= heatModel.rebootThreshold_Lance)
            {
                heatModel.lockedDown_Lance = false;
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
        if (!heatModel.lockedDown_Rockets)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (_otherStations.Contains(Stations.Rockets))
                heatModel.heat_Rockets -= heatModel.cooldownRate_Rockets;
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
                }
                else
                    heatModel.heat_Rockets -= heatModel.cooldownRate_Rockets;

                //Clamping
                if (heatModel.heat_Rockets >= 100)
                {
                    heatModel.heat_Rockets = 100;
                    heatModel.lockedDown_Rockets = true;
                }
                else if (heatModel.heat_Rockets <= 0)
                    heatModel.heat_Rockets = 0;
            }

            heatModel.heat_Total += heatModel.heat_Rockets;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Rockets -= heatModel.cooldownRate_Rockets;
            if (heatModel.heat_Rockets <= heatModel.rebootThreshold_Rockets)
            {
                heatModel.lockedDown_Rockets = false;
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
        if (!heatModel.lockedDown_Thrusters)
        {
            // _otherStations means that they're not being accessed, so only behavior is cooldown
            if (_otherStations.Contains(Stations.Thrusters))
                heatModel.heat_Thrusters -= heatModel.cooldownRate_Thrusters;
            // if accessed -
            //  when activated - heat goes up by activation cost + sustained
            //  when inactivated - heat goes down by cooldown
            else
            {
                if (heatModel.active_Thrusters)
                {
                    heatModel.heat_Thrusters += heatModel.sustainedHeatRate_Thrusters * Time.deltaTime;

                    if (heatModel.active_Thrusters && !_last_Thrusters)
                        heatModel.heat_Thrusters += heatModel.activationHeat_Thrusters;
                }
                else
                    heatModel.heat_Thrusters -= heatModel.cooldownRate_Thrusters;

                //Clamping
                if (heatModel.heat_Thrusters >= 100)
                {
                    heatModel.heat_Thrusters = 100;
                    heatModel.lockedDown_Thrusters = true;
                }
                else if (heatModel.heat_Thrusters <= 0)
                    heatModel.heat_Thrusters = 0;
            }

            heatModel.heat_Total += heatModel.heat_Thrusters;
        }
        // if locked down, charge 100 until less than reboot threshold
        else
        {
            heatModel.heat_Thrusters -= heatModel.cooldownRate_Thrusters;
            if (heatModel.heat_Thrusters <= heatModel.rebootThreshold_Thrusters)
            {
                heatModel.lockedDown_Thrusters = false;
                heatModel.heat_Total += heatModel.heat_Thrusters;
            }
            else
            {
                heatModel.heat_Total += 100;
            }
        }
    }

    private void _lastStatesUpdate()
    {
        _last_Guns = heatModel.active_Guns;
        _last_Lance = heatModel.active_Lance;
        _last_Rockets = heatModel.active_Rockets;
        _last_Shield = heatModel.active_Shield;
        _last_Thrusters = heatModel.active_Shield;
    }
    #endregion
}
