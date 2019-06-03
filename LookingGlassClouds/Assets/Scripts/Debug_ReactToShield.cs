using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ReactToShield : MonoBehaviour {

    public Material shieldMat;

    private Color c0;
    private Color c1;

    private Color workingColorC0;
    private Color workingColorC1;

    private AudioSource myAS;
    public AudioClip ding;

    void Start()
    {
        c0 = shieldMat.GetColor("_Color0");
        c1 = shieldMat.GetColor("_Color1");

        workingColorC0 = c0;
        workingColorC1 = c1;

        myAS = GetComponent<AudioSource>();
    }

    void Update()
    {
        workingColorC0 = Color.Lerp(workingColorC0, c0, .05f);
        workingColorC1 = Color.Lerp(workingColorC1, c1, .05f);

        shieldMat.SetColor("_Color0", workingColorC0);
        shieldMat.SetColor("_Color1", workingColorC1);
    }

    void OnDisable()
    {
        shieldMat.SetColor("_Color0", c0);
        shieldMat.SetColor("_Color1", c1);
    }

    public void OnTriggerEnter(Collider other)
    {
        float collisionDot = Vector3.Dot(shieldMat.GetVector("_Forward"), Vector3.Normalize(other.transform.position - transform.position));
        float dotThresh = shieldMat.GetFloat("_Cutoff");

        if (other.gameObject.name == "EnemyBullet(Clone)" && collisionDot >= dotThresh)
        {
            Destroy(other.gameObject);
            workingColorC0 = c0 * 2;
            workingColorC1 = c1 * 2;
            myAS.PlayOneShot(ding);
        }
    }

}
