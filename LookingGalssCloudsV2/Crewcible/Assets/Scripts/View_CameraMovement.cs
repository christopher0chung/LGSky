using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_CameraMovement : MonoBehaviour
{
    Model_Play playModel;
    Model_Game gameModel;

    public Transform holoPlay;

    private Transform player;

    private Vector3 pos_Initial;
    private Vector3 euler_Initial;

    public float pitchMax;
    public float panScalar;
    public float popMagnitudeMax;
    public float shakeMagnitudeMax;

    private Vector3 rotToApply;

    bool explosionShake;
    float explosionShakeScalar;
    float explosionShakeTimer;
    float explosionShakeInterval = 1.2f;
    float explosionShakeSubTimer;
    float explosionShakeSubInterval = .07f;

    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        player = ServiceLocator.instance.Player;

        pos_Initial = holoPlay.position;
        euler_Initial = holoPlay.eulerAngles;

        SCG_EventManager.instance.Register<Event_EnemyBulletHit>(EventHandler);
    }

    // Update is called once per frame
    void Update()
    {
        ExplosionShakeObserve();

        rotToApply.x = euler_Initial.x + pitchMax * ((playModel.worldSpeed_Current - gameModel.worldSpeed_min) / (gameModel.worldSpeed_max - gameModel.worldSpeed_min));
        //Debug.Log(((playModel.worldSpeed_Current - gameModel.worldSpeed_min) / (gameModel.worldSpeed_max - gameModel.worldSpeed_min)));
        rotToApply.y = euler_Initial.y + panScalar * player.transform.position.x;

        holoPlay.rotation = Quaternion.Slerp(holoPlay.rotation, Quaternion.Euler(rotToApply), 8 * Time.unscaledDeltaTime);
    }

    private void ExplosionShakeObserve()
    {
        if (playModel.deathExplosionTrigger == true)
        {
            playModel.deathExplosionTrigger = false;
            explosionShake = true;
        }

        if (explosionShake)
        {
            explosionShakeTimer += Time.unscaledDeltaTime;
            explosionShakeSubTimer += Time.unscaledDeltaTime;
            if (explosionShakeSubTimer >= explosionShakeSubInterval)
            {
                explosionShakeSubTimer -= explosionShakeSubInterval;
                Vector2 shakeRotOffset = shakeMagnitudeMax * 
                    ((explosionShakeInterval - explosionShakeTimer) / explosionShakeInterval) * 
                    ((explosionShakeInterval - explosionShakeTimer) / explosionShakeInterval) *
                    Vector3.Normalize(Random.insideUnitCircle);
                Vector3 eulerToPush = euler_Initial;
                eulerToPush += (Vector3)shakeRotOffset;
                holoPlay.rotation = Quaternion.Euler(eulerToPush);
            }

            if (explosionShakeTimer >= explosionShakeInterval)
            {
                explosionShake = false;
                explosionShakeTimer = 0;
                explosionShakeSubTimer = 0;
            }
        }
    }

    public void EventHandler(SCG_Event e)
    {
        Event_EnemyBulletHit bH = e as Event_EnemyBulletHit;

        if (bH != null)
        {
            Vector2 shakeRotOffset = popMagnitudeMax * Vector3.Normalize(Random.insideUnitCircle);
            Vector3 eulerToPush = holoPlay.eulerAngles;
            eulerToPush += (Vector3)shakeRotOffset;
            holoPlay.rotation = Quaternion.Euler(eulerToPush);
        }
    }
}
