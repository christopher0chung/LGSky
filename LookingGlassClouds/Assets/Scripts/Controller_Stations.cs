using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Stations : MonoBehaviour {

    private Model_Game gameModel;
    private Model_Input inputModel;
    private Model_Energy energyModel;
    private List<Stations> unassignedStations = new List<Stations>();

    private void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        energyModel = ServiceLocator.instance.Model.GetComponent<Model_Energy>();
    }

    void Update () {
        _MaintainStations(inputModel.L_SwapUp_OnDown, inputModel.R_SwapUp_OnDown, inputModel.L_SwapDown_OnDown, inputModel.R_SwapDown_OnDown);
	}
    #region Switchers
    private void _MaintainStations(bool left, bool right, bool altLeft, bool altRight)
    {
        if (left || right || altLeft || altRight)
        {
            // populate list
            unassignedStations = new List<Stations>();
            unassignedStations.Add(Stations.Guns);
            unassignedStations.Add(Stations.Pilot);
            unassignedStations.Add(Stations.Rockets);
            unassignedStations.Add(Stations.Shield);
            unassignedStations.Add(Stations.Sword);

            unassignedStations.Remove(gameModel.leftStation);
            unassignedStations.Remove(gameModel.rightStation);

            if (left && !energyModel.leftLocked)
            {
                int next = (int)gameModel.leftStation + 1;
                if (next >= 5)
                    next = 0;

                if (unassignedStations.Contains((Stations)next))
                    gameModel.leftStation = (Stations)next;
                else
                {
                    next++;
                    if (next >= 5)
                        next = 0;
                    if (unassignedStations.Contains((Stations)next))
                        gameModel.leftStation = (Stations)next;
                }
            }
            else if (right && !energyModel.rightLocked)
            {
                int next = (int)gameModel.rightStation + 1;
                if (next >= 5)
                    next = 0;

                if (unassignedStations.Contains((Stations)next))
                    gameModel.rightStation = (Stations)next;
                else
                {
                    next++;
                    if (next >= 5)
                        next = 0;
                    if (unassignedStations.Contains((Stations)next))
                        gameModel.rightStation = (Stations)next;
                }
            }
            else if (altLeft && !energyModel.leftLocked)
            {
                int prev = (int)gameModel.leftStation - 1;
                if (prev < 0)
                    prev = 4;

                if (unassignedStations.Contains((Stations)prev))
                    gameModel.leftStation = (Stations)prev;
                else
                {
                    prev--;
                    if (prev < 0)
                        prev = 4;
                    if (unassignedStations.Contains((Stations)prev))
                        gameModel.leftStation = (Stations)prev;
                }
            }
            else if (altRight && !energyModel.rightLocked)
            {
                int prev = (int)gameModel.rightStation - 1;
                if (prev < 0)
                    prev = 4;

                if (unassignedStations.Contains((Stations)prev))
                    gameModel.rightStation = (Stations)prev;
                else
                {
                    prev--;
                    if (prev < 0)
                        prev = 4;
                    if (unassignedStations.Contains((Stations)prev))
                        gameModel.rightStation = (Stations)prev;
                }
            }
        }
    }

    #endregion
}
