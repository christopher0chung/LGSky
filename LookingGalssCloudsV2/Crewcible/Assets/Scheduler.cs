using System.Collections;
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
            for (int i = 0; i < scheduledControllers.Count; i++)
            {
                if (controller.priority <= scheduledControllers[i].priority)
                {
                    scheduledControllers.Insert(i, controller);
                    return;
                }
            }
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
