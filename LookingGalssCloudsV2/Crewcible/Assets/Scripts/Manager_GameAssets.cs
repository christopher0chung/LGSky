using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_GameAssets : SCG_Controller {

    private Model_Game gameModel;

    List<GameObject> bullets_Active;
    public List<GameObject> bullets_Inactive;
    List<float> bullets_Times;

    List<GameObject> bExplosion_Active;
    public List<GameObject> bExplosion_Inactive;
    List<float> bExplosion_Times;

    List<GameObject> rockets_Active;
    public List<GameObject> rockets_Inactive;
    List<float> rockets_Times;

    List<GameObject> rExplosion_Active;
    public List<GameObject> rExplosion_Inactive;
    List<float> rExplosion_Times;

    List<GameObject> dExplosion_Active;
    public List<GameObject> dExplosion_Inactive;
    List<float> dExplosion_Times;

    List<GameObject> mExplosion_Active;
    public List<GameObject> mExplosion_Inactive;
    List<float> mExplosion_Times;

    List<GameObject> mslExplosion_Active;
    List<GameObject> mslExplosion_Inactive;
    List<float> mslExplosion_Times;

    List<GameObject> lanceHit_Active;
    List<GameObject> lanceHit_Inactive;
    List<float> lanceHit_Times;

    List<GameObject> SFX_Active;
    List<GameObject> SFX_Inactive;
    List<float> SFX_Times;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        SCG_EventManager.instance.Register<Event_PlayerBulletHit>(EffectsEventHandler);
        SCG_EventManager.instance.Register<Event_PlayerRocketHit>(EffectsEventHandler);
        SCG_EventManager.instance.Register<Event_LanceHit>(EffectsEventHandler);
        SCG_EventManager.instance.Register<Event_ExplosionBallHit>(EffectsEventHandler);
        SCG_EventManager.instance.Register<Event_EnemyDeath>(EffectsEventHandler);
        SCG_EventManager.instance.Register<Event_EnemyMineHit>(EffectsEventHandler);
        SCG_EventManager.instance.Register<Event_EnemyMissileHit>(EffectsEventHandler);
    }

	void Start () {

        bullets_Active = new List<GameObject>();
        bullets_Inactive = new List<GameObject>();
        bullets_Times = new List<float>();
        _PrepInactive(gameModel.bulletPrefab, bullets_Inactive, 100);

        bExplosion_Active = new List<GameObject>();
        bExplosion_Inactive = new List<GameObject>();
        bExplosion_Times = new List<float>();
        _PrepInactive(gameModel.bulletExplosionPrefab, bExplosion_Inactive, 50);

        rockets_Active = new List<GameObject>();
        rockets_Inactive = new List<GameObject>();
        rockets_Times = new List<float>();
        _PrepInactive(gameModel.rocketPrefab, rockets_Inactive, 40);

        rExplosion_Active = new List<GameObject>();
        rExplosion_Inactive = new List<GameObject>();
        rExplosion_Times = new List<float>();
        _PrepInactive(gameModel.rocketExplosionPrefab, rExplosion_Inactive, 40);

        dExplosion_Active = new List<GameObject>();
        dExplosion_Inactive = new List<GameObject>();
        dExplosion_Times = new List<float>();
        _PrepInactive(gameModel.deathExplosionPrefab, dExplosion_Inactive, 30);

        mExplosion_Active = new List<GameObject>();
        mExplosion_Inactive = new List<GameObject>();
        mExplosion_Times = new List<float>();
        _PrepInactive(gameModel.mineExplosionPrefab, mExplosion_Inactive, 5);

        mslExplosion_Active = new List<GameObject>();
        mslExplosion_Inactive = new List<GameObject>();
        mslExplosion_Times = new List<float>();
        _PrepInactive(gameModel.missileExplosionPrefab, mslExplosion_Inactive, 15);

        lanceHit_Active = new List<GameObject>();
        lanceHit_Inactive = new List<GameObject>();
        lanceHit_Times = new List<float>();
        _PrepInactive(gameModel.lanceHitPrefab, lanceHit_Inactive, 50);

        SFX_Active = new List<GameObject>();
        SFX_Inactive = new List<GameObject>();
        SFX_Times = new List<float>();
        _PrepInactive(gameModel.sfxPlayerPrefab, SFX_Inactive, 50);

        priority = 9;
        Schedule(this);
    }

	public override void ScheduledUpdate () {
		if (bullets_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(bullets_Times, gameModel.t_Guns_BulletLifetime);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(bullets_Active.Count == bullets_Times.Count, "Active element and tracking mismatch: bullets");
                _StowActiveBullet(0);
            }
        }

        if (bExplosion_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(bExplosion_Times, 1);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(bExplosion_Active.Count == bExplosion_Times.Count, "Active element and tracking mismatch: bExplosion");
                _StowActiveNonPhysicsManagedGO(bExplosion_Active, bExplosion_Times, bExplosion_Inactive, 0);
            }
        }

        if (rockets_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(rockets_Times, gameModel.t_Rockets_Lifetime);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(rockets_Active.Count == rockets_Times.Count, "Active element and tracking mismatch: rockets");
                Make(MyGameAsset.RocketExplosion, rockets_Active[0].transform.position);
                _StowActiveNonPhysicsManagedGO(rockets_Active, rockets_Times, rockets_Inactive, 0);
            }
        }

        if (rExplosion_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(rExplosion_Times, gameModel.t_Rockets_ExplosionBallLifetime);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(rExplosion_Active.Count == rExplosion_Times.Count, "Active element and tracking mismatch: rExplosion");
                _StowActiveNonPhysicsManagedGO(rExplosion_Active, rExplosion_Times, rExplosion_Inactive, 0);
            }
        }

        if (dExplosion_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(dExplosion_Times, 2f);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(dExplosion_Active.Count == dExplosion_Times.Count, "Active element and tracking mismatch: dExplosion");
                _StowActiveNonPhysicsManagedGO(dExplosion_Active, dExplosion_Times, dExplosion_Inactive, 0);
            }
        }

        if (mExplosion_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(mExplosion_Times, .5f);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(mExplosion_Active.Count == mExplosion_Times.Count, "Active element and tracking mismatch: mExplosion");
                _StowActiveNonPhysicsManagedGO(mExplosion_Active, mExplosion_Times, mExplosion_Inactive, 0);
            }
        }

        if (mslExplosion_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(mExplosion_Times, 1.5f);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(mslExplosion_Active.Count == mslExplosion_Times.Count, "Active element and tracking mismatch: mslExplosion");
                _StowActiveNonPhysicsManagedGO(mslExplosion_Active, mslExplosion_Times, mslExplosion_Inactive, 0);
            }
        }

        if (lanceHit_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(lanceHit_Times, .1f);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(lanceHit_Active.Count == lanceHit_Times.Count, "Active element and tracking mismatch: lanceHit");
                _StowActiveNonPhysicsManagedGO(lanceHit_Active, lanceHit_Times, lanceHit_Inactive, 0);
            }
        }

        if (SFX_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(SFX_Times, 1.0f);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(SFX_Active.Count == SFX_Times.Count, "Active element and tracking mismatch: sfx");
                _StowActiveNonPhysicsManagedGO(SFX_Active, SFX_Times, SFX_Inactive, 0);
            }
        }
    }

    void EffectsEventHandler(SCG_Event e)
    {
        Event_PlayerBulletHit bH = e as Event_PlayerBulletHit;

        if (bH != null)
        {
            if (_IsBulletActiveCheck(bH.bullet.gameObject))
            {
                Make(MyGameAsset.BulletExplosion, bH.location);

                int indexOfBullet = bullets_Active.IndexOf(bH.bullet.gameObject);
                _StowActiveBullet(indexOfBullet);
                return;
            }
        }

        Event_PlayerRocketHit rH = e as Event_PlayerRocketHit;

        if (rH != null)
        {
            if (_IsGOActiveCheck(rH.rocket.gameObject, rockets_Active))
            {
                //Debug.Log("Rocket hit registered by GAManager");

                Make(MyGameAsset.RocketExplosion, rH.location);

                int indexOfRocket = rockets_Active.IndexOf(rH.rocket.gameObject);
                _StowActiveNonPhysicsManagedGO(rockets_Active, rockets_Times, rockets_Inactive, indexOfRocket);
                return;
            }
        }

        Event_LanceHit lH = e as Event_LanceHit;

        if (lH != null)
        {
            Make(MyGameAsset.LanceHit, lH.location);
            return;
        }

        Event_ExplosionBallHit x = e as Event_ExplosionBallHit;

        if (x != null)
        {
            Make(MyGameAsset.BulletExplosion, x.location);
            return;
        }

        Event_EnemyDeath eD = e as Event_EnemyDeath;

        if (eD != null)
        {
            //GameObject g = Make(MyGameAsset.DeathExplosion, eD.location);
            //g.GetComponent<Behavior_DeathExplosionTimer>().Explode();
            GameObject g = Make(MyGameAsset.SFX, eD.location);
            g.GetComponent<AudioSource>().PlayOneShot(gameModel.sfx_EnemyLittleExplosion);
        }

        Event_EnemyMineHit m = e as Event_EnemyMineHit;

        if (m != null)
        {
            GameObject g = Make(MyGameAsset.MineExplosion, m.location);
            //g.GetComponent<Behavior_DeathExplosionParent>().Explode();
        }

        Event_EnemyMissileHit msl = e as Event_EnemyMissileHit;

        if (msl != null)
        {
            GameObject g = Make(MyGameAsset.MissileExplosion, msl.location);
        }
    }

    public GameObject Make(MyGameAsset type, Vector3 where)
    {
        if (type == MyGameAsset.Bullet)
        {
            GameObject _bullet;
            if (bullets_Inactive.Count >= 1)
            {
                _bullet = _Make_GenericActivate(bullets_Inactive, bullets_Active, bullets_Times, where);
            }
            else
            {
                _bullet = _Make_GenericNewObj(gameModel.bulletPrefab, bullets_Active, bullets_Times, where);
            }
            //Debug.Log(_bullet.name);
            return _bullet;
        }
        else if (type == MyGameAsset.BulletExplosion)
        {
            GameObject _bExplosion;
            if (bExplosion_Inactive.Count >= 1)
            {
                _bExplosion = _Make_GenericActivate(bExplosion_Inactive, bExplosion_Active, bExplosion_Times, where);
                _bExplosion.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                _bExplosion = _Make_GenericNewObj(gameModel.bulletExplosionPrefab, bExplosion_Active, bExplosion_Times, where);
            }
            return _bExplosion;
        }
        else if (type == MyGameAsset.Rocket)
        {
            GameObject _rocket;
            if (rockets_Inactive.Count >= 1)
            {
                _rocket = _Make_GenericActivate(rockets_Inactive, rockets_Active, rockets_Times, where);
            }
            else
            {
                _rocket = _Make_GenericNewObj(gameModel.rocketPrefab, rockets_Active, rockets_Times, where);
            }
            return _rocket;
        }
        else if (type == MyGameAsset.RocketExplosion)
        {
            GameObject _rExplosion;
            if (rExplosion_Inactive.Count >= 1)
            {
                _rExplosion = _Make_GenericActivate(rExplosion_Inactive, rExplosion_Active, rExplosion_Times, where);
            }
            else
            {
                _rExplosion = _Make_GenericNewObj(gameModel.rocketExplosionPrefab, rExplosion_Active, rExplosion_Times, where);
            }
            return _rExplosion;
        }
        else if (type == MyGameAsset.DeathExplosion)
        {
            GameObject _dExplosion;
            if (dExplosion_Inactive.Count >= 1)
            {
                _dExplosion = _Make_GenericActivate(dExplosion_Inactive, dExplosion_Active, dExplosion_Times, where);
            }
            else
            {
                _dExplosion = _Make_GenericNewObj(gameModel.deathExplosionPrefab, dExplosion_Active, dExplosion_Times, where);
            }
            return _dExplosion;
        }
        else if (type == MyGameAsset.MineExplosion)
        {
            GameObject _mExplosion;
            if (mExplosion_Inactive.Count >= 1)
            {
                _mExplosion = _Make_GenericActivate(mExplosion_Inactive, mExplosion_Active, mExplosion_Times, where);
            }
            else
            {
                _mExplosion = _Make_GenericNewObj(gameModel.mineExplosionPrefab, mExplosion_Active, mExplosion_Times, where);
            }
            _mExplosion.GetComponent<Behavior_DeathExplosionParent>().Explode();
            return _mExplosion;
        }
        else if (type == MyGameAsset.MissileExplosion)
        {
            GameObject _mslExplosion;
            if (mslExplosion_Inactive.Count >= 1)
            {
                _mslExplosion = _Make_GenericActivate(mslExplosion_Inactive, mslExplosion_Active, mslExplosion_Times, where);
            }
            else
            {
                _mslExplosion = _Make_GenericNewObj(gameModel.missileExplosionPrefab, mslExplosion_Active, mslExplosion_Times, where);
            }
            _mslExplosion.GetComponent<ParticleSystem>().Play();
            return _mslExplosion;
        }
        else if (type == MyGameAsset.LanceHit)
        {
            GameObject _lanceHit;
            if (lanceHit_Inactive.Count >= 1)
            {
                _lanceHit = _Make_GenericActivate(lanceHit_Inactive, lanceHit_Active, lanceHit_Times, where);
            }
            else
            {
                _lanceHit = _Make_GenericNewObj(gameModel.lanceHitPrefab, lanceHit_Active, lanceHit_Times, where);
            }
            _lanceHit.GetComponent<ParticleSystem>().Play();
            return _lanceHit;
        }
        else if (type == MyGameAsset.SFX)
        {
            GameObject _sfx;
            if (SFX_Inactive.Count >= 1)
            {
                _sfx = _Make_GenericActivate(SFX_Inactive, SFX_Active, SFX_Times, where);
            }
            else
            {
                _sfx = _Make_GenericNewObj(gameModel.sfxPlayerPrefab, SFX_Active, SFX_Times, where);
            }
            return _sfx;
        }
        else return null;
    }

    #region Generic
    private void _PrepInactive(GameObject managedObject, List<GameObject> inactive, int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(managedObject, Vector3.zero, Quaternion.identity, transform);
            inactive.Add(g);
            g.SetActive(false);
        }
    }

    private GameObject _Make_GenericActivate(List<GameObject> inactive, List<GameObject> active, List<float> managedTimes, Vector3 location)
    {
        GameObject _managedGO = inactive[0];
        inactive.Remove(_managedGO);
        _managedGO.transform.position = location;
        _managedGO.SetActive(true);
        active.Add(_managedGO);
        managedTimes.Add(0);
        return _managedGO;
    }

    private GameObject _Make_GenericNewObj (GameObject prefab, List<GameObject> active, List<float> managedTimes, Vector3 location)
    {
        GameObject _managedGO = Instantiate(prefab, location, Quaternion.identity, transform);
        active.Add(_managedGO);
        managedTimes.Add(0);
        return _managedGO;
    }

    private int _UpdateAndCheckForOverTime(List<float> managedTimes, float cutoff)
    {
        int numOverLimit = 0;
        for (int i = 0; i < managedTimes.Count; i++)
        {
            managedTimes[i] += Time.deltaTime;
            if (managedTimes[i] >= cutoff)
                numOverLimit++;
        }
        return numOverLimit;
    }

    private void _StowActiveBullet(int bulletIndex)
    {
        bullets_Times.RemoveAt(bulletIndex);
        GameObject bullet = bullets_Active[bulletIndex];
        bullets_Active.Remove(bullet);
        bullets_Inactive.Add(bullet);
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.SetActive(false);
    }

    private void _StowActiveNonPhysicsManagedGO(List<GameObject> active, List<float> managedTimes, List<GameObject> inactive, int index)
    {
        managedTimes.RemoveAt(index);
        GameObject _NPGO = active[index];
        if (_NPGO == null)
        {
            Debug.Log("ITS NULLLLLL");
        }
        active.RemoveAt(index);
        inactive.Add(_NPGO);
        _NPGO.SetActive(false);
        Debug.Assert(active.Count == managedTimes.Count, "Stow tracking mismatch!!");
    }

    private bool _IsBulletActiveCheck(GameObject bullet)
    {
        if (bullets_Active.Contains(bullet))
            return true;
        else return false;
    }

    private bool _IsGOActiveCheck(GameObject bullet, List<GameObject> active)
    {
        if (active.Contains(bullet))
            return true;
        else return false;
    }
    #endregion
}

public enum MyGameAsset { Bullet, BulletExplosion, Rocket, RocketExplosion, DeathExplosion, MineExplosion, MissileExplosion, SFX, LanceHit }
