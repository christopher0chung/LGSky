using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_PlayEffects : MonoBehaviour
{
    Model_Play playModel;
    Model_Game gameModel;

    public Material shieldMat;
    public ParticleSystem playerShotPS;

    private Color c0;
    private Color c1;

    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();        
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        SCG_EventManager.instance.Register<Event_PlayerShieldBlock>(EventHandler);
        SCG_EventManager.instance.Register<Event_EnemyBulletHit>(EventHandler);
    }

    void LateUpdate()
    {
        c0 = Color.Lerp(c0, gameModel.c_Shield_Inside, Time.deltaTime * 10);
        c1 = Color.Lerp(c1, gameModel.c_Shield_Outside, Time.deltaTime * 10);

        shieldMat.SetColor("_Color0", c0);
        shieldMat.SetColor("_Color1", c1);

        shieldMat.SetFloat("_Cutoff", playModel.shieldSize);
        shieldMat.SetVector("_Forward", playModel.shieldDirection);

    }

    public void EventHandler(SCG_Event e)
    {
        Event_PlayerShieldBlock psb = e as Event_PlayerShieldBlock;
        if (psb != null)
        {
            c0 = playModel.appliedBulletColor;
            c1 = playModel.appliedBulletColor;
            return;
        }

        Event_EnemyBulletHit ebb = e as Event_EnemyBulletHit;
        if (ebb != null)
        {
            playerShotPS.Emit(10);
        }
    }
}
