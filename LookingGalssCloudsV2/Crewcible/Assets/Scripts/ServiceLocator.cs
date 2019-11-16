using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private static ServiceLocator _i;
    public static ServiceLocator instance
    {
        get
        {
            if (_i == null)
                _i = new ServiceLocator();
            return _i;
        }
    }

    private Transform _app;
    public Transform Application
    {
        get
        {
            if (_app == null)
                _app = GameObject.Find("Application").transform;
            return _app;
        }
    }

    private Transform _m;
    private Transform _v;
    private Transform _c;

    public Transform Model
    {
        get
        {
            if (_m == null)
                _m = Application.Find("Model");
            return _m;
        }
    }

    public Transform View
    {
        get
        {
            if (_v == null)
                _v = Application.Find("View");
            return _v;
        }
    }

    public Transform Controller
    {
        get
        {
            if (_c == null)
                _c = Application.Find("Controller");
            return _c;
        }
    }

    private Transform _p;

    public Transform Player
    {
        get
        {
            if (_p == null)
                _p = View.Find("Player");
            return _p;
        }
    }

    private AudioSource _sfx;

    public AudioSource SFX
    {
        get
        {
            if (_sfx == null)
                _sfx = Application.gameObject.GetComponent<AudioSource>();
            return _sfx;
        }
    }
}

public class SCG_Model : MonoBehaviour { }

public class SCG_View : MonoBehaviour { }
public class SCG_Controller: MonoBehaviour
{
    [HideInInspector] public int priority = 1000;

    public virtual void Schedule(SCG_Controller c)
    {
        ServiceLocator.instance.Controller.GetComponent<Scheduler>().Register(c);
    }

    public virtual void ScheduledUpdate()
    {

    }

    public virtual void ScheduledFixedUpdate()
    {

    }
}
