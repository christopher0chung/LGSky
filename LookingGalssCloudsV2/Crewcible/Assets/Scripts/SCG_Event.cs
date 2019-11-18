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

public class Event_PlayerSwordHit : SCG_Event
{
    public Enemy_Base enemyHit;
    public float enemyDamageTaken;
    public Vector3 location;

    public Event_PlayerSwordHit(Enemy_Base enemyHit, float damage, Vector3 where)
    {
        this.enemyHit = enemyHit;
        enemyDamageTaken = damage;
        location = where;
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
    public Enemy_Base enemyHit;
    public float damageTaken;
    public Vector3 location;
    public Behavior_Rockets rocket;

    public Event_PlayerRocketHit(Enemy_Base enemyHit, float damage, Vector3 where, Behavior_Rockets rocket)
    {
        this.enemyHit = enemyHit;
        damageTaken = damage;
        location = where;
        this.rocket = rocket;
    }
}

public class Event_PlayerShieldBlock : SCG_Event { }

public class Event_ExplosionBallHit : SCG_Event
{
    public Enemy_Base enemyHit;
    public float enemyDamageTaken;
    public Vector3 location;

    public Event_ExplosionBallHit(Enemy_Base enemyHit, float damage, Vector3 where)
    {
        this.enemyHit = enemyHit;
        enemyDamageTaken = damage;
        location = where;
    }
}

public class Event_EnemyBulletHit : SCG_Event
{
    public Event_EnemyBulletHit() { }
}

public class Event_EnemyBulletBlock : SCG_Event
{
    public Event_EnemyBulletBlock() { }
}

public class Event_EnemyMineHit : SCG_Event
{
    public Enemy_Base enemyToBeDestroyed;
    public Vector3 location;

    public Event_EnemyMineHit(Enemy_Base enemyToBeDestroyed, Vector3 where)
    {
        this.enemyToBeDestroyed = enemyToBeDestroyed;
        location = where;
    }
}

public class Event_EnemyMissileHit : SCG_Event
{
    public Enemy_Base enemyToBeDestroyed;
    public Vector3 location;

    public Event_EnemyMissileHit(Enemy_Base enemyToBeDestroyed, Vector3 where)
    {
        this.enemyToBeDestroyed = enemyToBeDestroyed;
        location = where;
    }
}

public class Event_BonusPoints : SCG_Event
{
    public int score;

    public Event_BonusPoints(int bonus)
    {
        score = bonus;
    }
}

public class Event_Restart : SCG_Event
{
    public Event_Restart() { }
}