using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_GameAssets : MonoBehaviour {

    public Model_Game gameModel;

    List<GameObject> bullets_Active;
    List<GameObject> bullets_Inactive;
    List<float> bullets_Times;

    List<GameObject> bExplosion_Active;
    List<GameObject> bExplosion_Inactive;
    List<float> bExplosion_Times;

    List<GameObject> rockets_Active;
    List<GameObject> rockets_Inactive;
    List<float> rockets_Times;

    List<GameObject> rExplosion_Active;
    List<GameObject> rExplosion_Inactive;
    List<float> rExplosion_Times;

    void Awake()
    {
        SCG_EventManager.instance.Register<Event_PlayerBulletHit>(EffectsEventHandler);
        SCG_EventManager.instance.Register<Event_PlayerRocketHit>(EffectsEventHandler);
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
    }

	void Update () {
		if (bullets_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(bullets_Times, 2);
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
            int numOverLimit = _UpdateAndCheckForOverTime(rockets_Times, 10f);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(rockets_Active.Count == rockets_Times.Count, "Active element and tracking mismatch: rockets");
                _StowActiveNonPhysicsManagedGO(rockets_Active, rockets_Times, rockets_Inactive, 0);
            }
        }

        if (rExplosion_Times.Count > 0)
        {
            int numOverLimit = _UpdateAndCheckForOverTime(rExplosion_Times, 1);
            for (int i = 0; i < numOverLimit; i++)
            {
                Debug.Assert(rExplosion_Active.Count == rExplosion_Times.Count, "Active element and tracking mismatch: rExplosion");
                _StowActiveNonPhysicsManagedGO(rExplosion_Active, rExplosion_Times, rExplosion_Inactive, 0);
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
            }
        }

        Event_PlayerRocketHit rH = e as Event_PlayerRocketHit;

        if (rH != null)
        {
            if (_IsGOActiveCheck(rH.rocket.gameObject, rockets_Active))
            {
                Debug.Log("Rocket hit registered by GAManager");

                Make(MyGameAsset.RocketExplosion, rH.location);

                int indexOfRocket = rockets_Active.IndexOf(rH.rocket.gameObject);
                _StowActiveNonPhysicsManagedGO(rockets_Active, rockets_Times, rockets_Inactive, indexOfRocket);
            }
        }
    }

    private void _PrepInactive(GameObject managedObject, List<GameObject> inactive, int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(managedObject, Vector3.zero, Quaternion.identity);
            inactive.Add(g);
            g.SetActive(false);
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
            Debug.Log(_bullet);
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
            if (rExplosion_Times.Count >= 1)
            {
                _rExplosion = _Make_GenericActivate(rExplosion_Inactive, rExplosion_Active, rExplosion_Times, where);
            }
            else
            {
                _rExplosion = _Make_GenericNewObj(gameModel.rocketExplosionPrefab, rExplosion_Active, rExplosion_Times, where);
            }
            return _rExplosion;
        }
        else return null;
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
        GameObject _managedGO = Instantiate(prefab, location, Quaternion.identity);
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
}

public enum MyGameAsset { Bullet, BulletExplosion, Rocket, RocketExplosion }
