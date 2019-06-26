using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ReactToShield : MonoBehaviour {

    public Model_Game gameModel;
    public Model_Energy energyModel;

    public Material shieldMat;

    private Color c0;
    private Color c1;

    private Color workingColorC0;
    private Color workingColorC1;

    private AudioSource myAS;
    public AudioClip ding;

    private Transform player;

    public LayerMask layerMask;
    private RaycastHit rCH;

    //public GameObject marker;

    void Start()
    {
        c0 = shieldMat.GetColor("_Color0");
        c1 = shieldMat.GetColor("_Color1");

        workingColorC0 = c0;
        workingColorC1 = c1;

        myAS = GetComponent<AudioSource>();

        player = ServiceLocator.instance.Player;
    }

    void Update()
    {
        workingColorC0 = Color.Lerp(workingColorC0, c0, .05f);
        workingColorC1 = Color.Lerp(workingColorC1, c1, .05f);

        shieldMat.SetColor("_Color0", workingColorC0);
        shieldMat.SetColor("_Color1", workingColorC1);

        transform.position = player.position;
    }

    void OnDisable()
    {
        shieldMat.SetColor("_Color0", c0);
        shieldMat.SetColor("_Color1", c1);
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 behind = other.transform.position - other.transform.forward;

        Physics.Raycast(behind, other.transform.forward, out rCH, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);

        Vector3 contactPoint = rCH.point;

        //Instantiate(marker, contactPoint, Quaternion.identity, null);

        float collisionDot = Vector3.Dot(Vector3.Normalize(gameModel.shieldForwardDirection), Vector3.Normalize(contactPoint - transform.position));

        float dotThresh = energyModel.shieldSize_Cutoff;

        Debug.Log("On Trigger Enter, the cDot is: " + collisionDot + " --- dotThresh is: " + dotThresh);

        if (other.gameObject.name == "EnemyBullet(Clone)" && collisionDot >= dotThresh)
        {
            Destroy(other.gameObject);
            workingColorC0 = c0 * 2;
            workingColorC1 = c1 * 2;
            myAS.PlayOneShot(ding);
            SCG_EventManager.instance.Fire(new Event_EnemyBulletBlock());
        }
        else Debug.Log("Missed shot by a dot-product value of: " + (dotThresh - collisionDot));
    }
}
