using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_DeathExplosionTimer : MonoBehaviour {

    private ParticleSystem _ps;
    //private ParticleSystem _ps_Debris;

    private float _t;
    private float timer
    {
        get
        {
            return _t;
        }
        set
        {
            if (value != _t)
            {
                _t = value;
                if (value == 1)
                    _ps.Stop();
            }
        }
    }
    private float intermediateTimer;
    public float maxTime;

    public void Awake()
    {
        if (_ps == null)
        {
            _ps = GetComponent<ParticleSystem>();
            //_ps_Debris = GetComponentInChildren<ParticleSystem>();
        }

        timer = 0;
        _ps.Play();
        //_ps_Debris.Emit(20);
    }

    public void RestartTimer()
    {
        Awake();
    }

    void Update () {
        intermediateTimer += Time.deltaTime / maxTime;
        timer = Mathf.Clamp01(intermediateTimer);
	}
}
