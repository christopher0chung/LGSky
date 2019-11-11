using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnemyBullet : MonoBehaviour
{

    public float speed;
    private Transform target;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
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
}
