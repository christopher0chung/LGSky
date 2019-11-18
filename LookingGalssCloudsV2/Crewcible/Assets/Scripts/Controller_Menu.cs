using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Menu : SCG_Controller
{
    private void Start()
    {
        priority = 2;
        Schedule(this);
    }


}
