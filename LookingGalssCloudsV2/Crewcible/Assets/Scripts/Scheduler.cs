using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : MonoBehaviour
{
    public List<SCG_Controller> scheduledControllers;
    Model_Play playModel;

    private void Awake()
    {
        scheduledControllers = new List<SCG_Controller>();
    }

    private void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    public void Register(SCG_Controller controller)
    {
        //Debug.Log("my priority is " + controller.priority);

        if (scheduledControllers.Count == 0)
            scheduledControllers.Add(controller);
        else if (scheduledControllers.Count >= 1)
        {
            //Debug.Log("attempting to schedule priority " + controller.priority + " controller");
            for (int i = 0; i < scheduledControllers.Count; i++)
            {
                if (controller.priority <= scheduledControllers[i].priority)
                {
                    scheduledControllers.Insert(i, controller);
                    //Debug.Log(controller.priority + " is inserted at " + i);
                    return;
                }

            }
            // Fall back
            // If there isn't a higher priority item already present, add it to the end.
            scheduledControllers.Add(controller);
        }
    }

    void Update()
    {
        for (int i = 0; i < scheduledControllers.Count; i++)
        {
            if (!playModel.isPaused || i == 0)
                scheduledControllers[i].ScheduledUpdate();
        }
    }

    void FixedUpdate()
    {
        if (!playModel.isPaused)
        {
            for (int i = 0; i < scheduledControllers.Count; i++)
            {
                scheduledControllers[i].ScheduledFixedUpdate();
            }
        }
    }
}

//Controller priority map
//0 - Input                 Reads inputs which is the basis of all actions
//1 - Guns                  Respond to inputs in alphabetical order
//2 - Lance
//3 - Rockets
//4 - Shield
//5 - Thrusters
//6 - Heat                  Accounting based on input based actions
//7 - Stations              Requires heat based lockout to prevent swaps
//8 - Jump
//9 - Asset Manager         Cleans up at the end
