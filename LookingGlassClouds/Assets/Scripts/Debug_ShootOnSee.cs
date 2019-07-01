using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ShootOnSee : MonoBehaviour {

    public Transform whoAmILookingFor;
    public GameObject bullet;
    public GameObject bullet_PotShot;

    private Model_Game gameModel;

    private bool haveIFired;
    private float coolDownTimer;

    private float potShotTimer;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }

    void Start()
    {
        whoAmILookingFor = ServiceLocator.instance.Player;
        potShotTimer = Random.Range(0, gameModel.m_LockingBaddyPotshotTime);
    }

	void Update () {

        if (Vector3.Dot(transform.GetChild(0).forward, Vector3.Normalize(whoAmILookingFor.position - transform.position)) >= .75f
            && Vector3.Distance(whoAmILookingFor.position, transform.position) < 7
            && !haveIFired)
        {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity, null);

            Vector3 shootDir = Quaternion.Euler(Random.insideUnitSphere * 3) *Vector3.Normalize(whoAmILookingFor.position - transform.position);
            //newBullet.GetComponent<Rigidbody>().AddForce(shootDir * 3, ForceMode.Impulse);
            newBullet.GetComponent<Debug_PointToFwd>().flyDir = shootDir;
            haveIFired = true;
        }

        if (haveIFired)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= 3)
            {
                haveIFired = false;
                coolDownTimer = 0;
            }
        }

        potShotTimer += Time.deltaTime;

        if (potShotTimer >= gameModel.m_LockingBaddyPotshotTime)
        {
            potShotTimer -= gameModel.m_LockingBaddyPotshotTime;
            if (Vector3.Distance(transform.position, whoAmILookingFor.position) < 10)
            {
                GameObject newBullet = Instantiate(bullet_PotShot, transform.position, Quaternion.identity, null);

                Vector3 shootDir = Vector3.Normalize(whoAmILookingFor.position - transform.position);
                newBullet.GetComponent<Debug_PointToFwd>().flyDir = shootDir;
            }
        }

    }
}
