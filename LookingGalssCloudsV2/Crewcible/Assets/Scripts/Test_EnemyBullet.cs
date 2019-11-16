using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnemyBullet : MonoBehaviour
{

    public float speed;
    private Transform target;

    float timer;

    Model_Play playModel;
    Model_Game gameModel;
    // Start is called before the first frame update
    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        target = ServiceLocator.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 11 * Time.deltaTime);

        if (timer <= 6)
            transform.position += transform.forward * speed * Time.deltaTime;
        else
            Destroy(this.gameObject);
    }

    Vector3 shieldOrientation;
    Vector3 shieldStrikeVector;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Shield")
        {
            shieldOrientation = playModel.shieldDirection;
            shieldStrikeVector = Vector3.Normalize(transform.position - other.transform.position);
            if (Vector3.Dot(shieldOrientation, shieldStrikeVector) <= playModel.shieldSize)
            {
                //Miss
            }
            else
            {
                ServiceLocator.instance.SFX.pitch = Random.Range(.95f, 1.05f);
                ServiceLocator.instance.SFX.PlayOneShot(gameModel.sfx_Shield_Block);
                Destroy(this.gameObject);
            }
        }
        else if (other.gameObject.name == "Pods")
        {
            ServiceLocator.instance.SFX.PlayOneShot(gameModel.sfx_Gun_Shot);
            SCG_EventManager.instance.Fire(new Event_EnemyBulletHit());
        }





    }
}
