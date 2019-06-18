using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_GameAssets : MonoBehaviour {

    public Model_Game gameModel;

    List<GameObject> bulletsActive;
    List<GameObject> bulletsInactive;
    List<float> bulletActiveTime;

    List<GameObject> bulletExplosionActive;
    List<GameObject> bulletExplosionInactive;
    List<float> bulletExplosionActiveTime;

    void Awake()
    {
        SCG_EventManager.instance.Register<Event_PlayerBulletHit>(EffectsEventHandler);
    }

	void Start () {

        bulletsActive = new List<GameObject>();
        bulletsInactive = new List<GameObject>();
        bulletActiveTime = new List<float>();

		for(int i = 0; i < 100; i++)
        {
            GameObject bullet = Instantiate(gameModel.bulletPrefab, null);
            bulletsInactive.Add(bullet);
            bullet.SetActive(false);
        }

        bulletExplosionActive = new List<GameObject>();
        bulletExplosionInactive = new List<GameObject>();
        bulletExplosionActiveTime = new List<float>();

        for (int i = 0; i < 50; i++)
        {
            GameObject explosion = Instantiate(gameModel.bulletExplosionPrefab, null);
            bulletExplosionInactive.Add(explosion);
            explosion.SetActive(false);
        }
    }

	void Update () {
		if (bulletActiveTime.Count > 0)
        {
            int numOverLimit = 0;
            for (int i = 0; i < bulletActiveTime.Count; i++)
            {
                bulletActiveTime[i] += Time.deltaTime;
                if (bulletActiveTime[i] >= 2)
                    numOverLimit++;
            }
            for (int i = 0; i < numOverLimit; i++)
            {

                StowActiveBullet(0);
            }
        }

        if (bulletExplosionActiveTime.Count > 0)
        {
            int numOverLimit = 0;
            for (int i = 0; i < bulletExplosionActiveTime.Count; i++)
            {
                bulletExplosionActiveTime[i] += Time.deltaTime;
                if (bulletExplosionActiveTime[i] >= .75)
                    numOverLimit++;
            }
            for (int i = 0; i < numOverLimit; i++)
            {
                bulletExplosionActiveTime.RemoveAt(0);
                GameObject bE = bulletExplosionActive[0];
                bulletExplosionActive.Remove(bE);
                bulletExplosionInactive.Add(bE);
                bE.SetActive(false);
            }
        }
    }

    void EffectsEventHandler(SCG_Event e)
    {
        Event_PlayerBulletHit h = e as Event_PlayerBulletHit;

        if (h != null)
        {
            Make(MyGameAsset.BulletExplosion, h.location);
            int indexOfBullet = bulletsActive.IndexOf(h.bullet.gameObject);

            StowActiveBullet(indexOfBullet);
        }
    }

    public GameObject Make(MyGameAsset type, Vector3 where)
    {
        if (type == MyGameAsset.Bullet)
        {
            GameObject bullet;
            if (bulletsInactive.Count >= 1)
            {
                bullet = bulletsInactive[0];
                bulletsInactive.Remove(bullet);
                bullet.transform.position = where;
                bullet.SetActive(true);
                bulletsActive.Add(bullet);
                bulletActiveTime.Add(0);
            }
            else
            {
                bullet = Instantiate(gameModel.bulletPrefab, where, Quaternion.identity);
                bulletsActive.Add(bullet);
                bulletActiveTime.Add(0);
            }
            Debug.Log(bullet);
            return bullet;
        }
        else if (type == MyGameAsset.BulletExplosion)
        {
            GameObject bulletExplosion;
            if (bulletExplosionInactive.Count >= 1)
            {
                bulletExplosion = bulletExplosionInactive[0];
                bulletExplosionInactive.Remove(bulletExplosion);
                bulletExplosion.transform.position = where;
                bulletExplosion.SetActive(true);
                bulletExplosionActive.Add(bulletExplosion);
                bulletExplosionActiveTime.Add(0);
                bulletExplosion.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                bulletExplosion = Instantiate(gameModel.bulletExplosionPrefab, where, Quaternion.identity);
                bulletExplosionActive.Add(bulletExplosion);
                bulletExplosionActiveTime.Add(0);
            }
            return bulletExplosion;
        }
        else return null;
    }

    private void StowActiveBullet(int bulletIndex)
    {
        bulletActiveTime.RemoveAt(bulletIndex);
        GameObject bullet = bulletsActive[bulletIndex];
        bulletsActive.Remove(bullet);
        bulletsInactive.Add(bullet);
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.SetActive(false);
    }
}

public enum MyGameAsset { Bullet, BulletExplosion }
