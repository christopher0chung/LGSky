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

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        target = ServiceLocator.instance.Player;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 11 * Time.fixedDeltaTime);

        if (timer <= 6)
            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        else
            Destroy(this.gameObject);
    }

    Vector3 shieldOrientation;
    Vector3 shieldStrikeVector;
    public void OnTriggerEnter(Collider other)
    {
        if (playModel.currentPlayerState != PlayerState.Alive)
            return;

        if (other.gameObject.name == "Shield")
        {
            shieldOrientation = Vector3.Normalize(playModel.shieldDirection);
            shieldStrikeVector = Vector3.Normalize(transform.position - target.position);
            float dot = Vector3.Dot(shieldOrientation, shieldStrikeVector);

            if (dot <= playModel.shieldSize)
            {
                //Miss
                Debug.Log(dot + "is less than " + playModel.shieldSize);
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
