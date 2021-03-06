﻿using System.Collections;
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

public class Event_LanceHit : SCG_Event
{
    public Enemy_Base enemyHit;
    public Vector3 location;
    public bool trueEnterFalseExit;

    public Event_LanceHit(Enemy_Base enemyHit, Vector3 where, bool tEFE)
    {
        this.enemyHit = enemyHit;
        location = where;
        trueEnterFalseExit = tEFE;
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
    public Behavior_RocketsAlt altRocket;

    public Event_PlayerRocketHit(Enemy_Base enemyHit, float damage, Vector3 where, Behavior_Rockets rocket)
    {
        this.enemyHit = enemyHit;
        damageTaken = damage;
        location = where;
        this.rocket = rocket;
    }

    public Event_PlayerRocketHit(Enemy_Base enemyHit, float damage, Vector3 where, Behavior_RocketsAlt rocket)
    {
        this.enemyHit = enemyHit;
        damageTaken = damage;
        location = where;
        this.altRocket = rocket;
    }
}

public class Event_PlayerShieldBlock : SCG_Event { }

public class Event_ExplosionBallHit : SCG_Event
{
    public Enemy_Base enemyHit;
    public Vector3 location;
    public bool trueEnterFalseExit;

    public Event_ExplosionBallHit(Enemy_Base enemyHit, Vector3 where, bool tEFE)
    {
        this.enemyHit = enemyHit;
        location = where;
        trueEnterFalseExit = tEFE;
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

public class Event_Audio : SCG_Event
{
    public AudioEvent type;

    public Event_Audio (AudioEvent eventType)
    {
        type = eventType;
    }
}

public class Event_DumpReg : SCG_Event
{

}

public class Event_Restart : SCG_Event { }

public class Event_LevelClear : SCG_Event
{

}

public class Event_Respawn : SCG_Event { }

public class Event_PlayerExplode : SCG_Event { }

public class Event_LifeUpTick : SCG_Event { }