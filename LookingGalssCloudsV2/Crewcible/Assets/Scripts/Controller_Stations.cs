using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Controller_Stations : SCG_Controller {

    private Model_Input inputModel;
    private Model_Play playModel;
    private Model_Heat heatModel;

    private void Awake()
    {
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
    }

    private void Start()
    {
        priority = 7;
        Schedule(this);
    }

    public override void ScheduledUpdate () {
        //_MLD first - Need info from heat model
        //_MS second
        //_POS last - Modifies list based on _MLD in _MS

        _MaintainLockDown();

        _MaintainStations(inputModel.L_SwapUp_OnDown, inputModel.R_SwapUp_OnDown, inputModel.L_SwapDown_OnDown, inputModel.R_SwapDown_OnDown);
        
        _PopulateOtherStations();
	}

    #region Switchers
    private void _MaintainLockDown()
    {
        if (playModel.leftStation == Stations.Guns && heatModel.overheated_Guns)
            playModel.leftStationLocked = true;
        else if (playModel.leftStation == Stations.Lance && heatModel.overheated_Lance)
            playModel.leftStationLocked = true;
        else if (playModel.leftStation == Stations.Rockets && heatModel.overheated_Rockets)
            playModel.leftStationLocked = true;
        else if (playModel.leftStation == Stations.Shield && heatModel.overheated_Shield)
            playModel.leftStationLocked = true;
        else if (playModel.leftStation == Stations.Thrusters && heatModel.overheated_Thrusters)
            playModel.leftStationLocked = true;
        else
            playModel.leftStationLocked = false;

        if (playModel.rightStation == Stations.Guns && heatModel.overheated_Guns)
            playModel.rightStationLocked = true;
        else if (playModel.rightStation == Stations.Lance && heatModel.overheated_Lance)
            playModel.rightStationLocked = true;
        else if (playModel.rightStation == Stations.Rockets && heatModel.overheated_Rockets)
            playModel.rightStationLocked = true;
        else if (playModel.rightStation == Stations.Shield && heatModel.overheated_Shield)
            playModel.rightStationLocked = true;
        else if (playModel.rightStation == Stations.Thrusters && heatModel.overheated_Thrusters)
            playModel.rightStationLocked = true;
        else
            playModel.rightStationLocked = false;
    }

    private void _MaintainStations(bool left, bool right, bool altLeft, bool altRight)
    {
        if (left || right || altLeft || altRight)
        {
            if (left && !playModel.leftStationLocked)
            {
                int next = (int)playModel.leftStation + 1;
                if (next >= 5)
                    next = 0;

                if (playModel.unaccessedStations.Contains((Stations)next))
                    playModel.leftStation = (Stations)next;
                else
                {
                    next++;
                    if (next >= 5)
                        next = 0;
                    if (playModel.unaccessedStations.Contains((Stations)next))
                        playModel.leftStation = (Stations)next;
                }
            }
            else if (right && !playModel.rightStationLocked)
            {
                int next = (int)playModel.rightStation + 1;
                if (next >= 5)
                    next = 0;

                if (playModel.unaccessedStations.Contains((Stations)next))
                    playModel.rightStation = (Stations)next;
                else
                {
                    next++;
                    if (next >= 5)
                        next = 0;
                    if (playModel.unaccessedStations.Contains((Stations)next))
                        playModel.rightStation = (Stations)next;
                }
            }
            else if (altLeft && !playModel.leftStationLocked)
            {
                int prev = (int)playModel.leftStation - 1;
                if (prev < 0)
                    prev = 4;

                if (playModel.unaccessedStations.Contains((Stations)prev))
                    playModel.leftStation = (Stations)prev;
                else
                {
                    prev--;
                    if (prev < 0)
                        prev = 4;
                    if (playModel.unaccessedStations.Contains((Stations)prev))
                        playModel.leftStation = (Stations)prev;
                }
            }
            else if (altRight && !playModel.rightStationLocked)
            {
                int prev = (int)playModel.rightStation - 1;
                if (prev < 0)
                    prev = 4;

                if (playModel.unaccessedStations.Contains((Stations)prev))
                    playModel.rightStation = (Stations)prev;
                else
                {
                    prev--;
                    if (prev < 0)
                        prev = 4;
                    if (playModel.unaccessedStations.Contains((Stations)prev))
                        playModel.rightStation = (Stations)prev;
                }
            }
        }
    }

    private void _PopulateOtherStations()
    {
        playModel.unaccessedStations.Clear();
        playModel.unaccessedStations = Enum.GetValues(typeof(Stations)).OfType<Stations>().ToList<Stations>();

        playModel.unaccessedStations.Remove(playModel.leftStation);
        playModel.unaccessedStations.Remove(playModel.rightStation);
    }
    #endregion
}
