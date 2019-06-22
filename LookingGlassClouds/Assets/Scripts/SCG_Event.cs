using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SCG_Event
{
    public delegate void Handler(SCG_Event e);
}

public class Event_PlayerBulletHit : SCG_Event
{
    public Enemy_Base enemyHit;
    public float enemyDamageTaken;
    public Vector3 location;
    public Behavior_Bullet bullet;

    public Event_PlayerBulletHit(Enemy_Base enemyHit, float damage, Vector3 where, Behavior_Bullet bullet)
    {
        this.enemyHit = enemyHit;
        enemyDamageTaken = damage;
        location = where;
        this.bullet = bullet;
    }
}

public class Event_EnemyDeath : SCG_Event
{
    public Enemy_Base enemyToBeDestroyed;
    public Vector3 location;

    public Event_EnemyDeath(Enemy_Base enemyToBeDestroyed, Vector3 where)
    {
        this.enemyToBeDestroyed = enemyToBeDestroyed;
        location = where;
    }
}

public class Event_PlayerRocketHit : SCG_Event
{
    public Vector3 location;
    public Debug_RocketFlight rocket;

    public Event_PlayerRocketHit (Vector3 where, Debug_RocketFlight rocket)
    {
        location = where;
        this.rocket = rocket;
    }
}