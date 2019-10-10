﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : MonoBehaviour
{
    public List<SCG_Controller> scheduledControllers;

    private void Awake()
    {
        scheduledControllers = new List<SCG_Controller>();
    }

    public void Register(SCG_Controller controller)
    {
        Debug.Log("my priority is " + controller.priority);

        if (scheduledControllers.Count == 0)
            scheduledControllers.Add(controller);
        else if (scheduledControllers.Count >= 1)
        {
            Debug.Log("attempting to schedule priority " + controller.priority + " controller");
            for (int i = 0; i < scheduledControllers.Count; i++)
            {
                if (controller.priority <= scheduledControllers[i].priority)
                {
                    scheduledControllers.Insert(i, controller);
                    Debug.Log(controller.priority + " is inserted at " + i);
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
            scheduledControllers[i].ScheduledUpdate();
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < scheduledControllers.Count; i++)
        {
            scheduledControllers[i].ScheduledFixedUpdate();
        }
    }
}
