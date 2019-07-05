using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Waves : MonoBehaviour {

    private Manager_Enemy enemies;
    private Controller_Effects effects;

    public EnemyType[] waveType;
    public float[] waveTimeInMinutes;
    public int[] waveNumber;
    public Vector3[] waveOrigins;

    private void Awake()
    {
        enemies = ServiceLocator.instance.Controller.GetComponent<Manager_Enemy>();
        effects = ServiceLocator.instance.Controller.GetComponent<Controller_Effects>();
    }

    private void Start()
    {
        Debug.Assert(waveType.Length == waveTimeInMinutes.Length, "type/time mismatch");
        Debug.Assert(waveTimeInMinutes.Length == waveNumber.Length, "time/number mismatch");
        Debug.Assert(waveNumber.Length == waveOrigins.Length, "number/origin mismatch");
    }

    private float _lastTimer;
    private float _timer;

    private void Update()
    {
        _lastTimer = _timer;
        _timer += Time.deltaTime/60;

        for (int i = 0; i < waveTimeInMinutes.Length; i++)
        {
            if (_lastTimer < waveTimeInMinutes[i] && _timer >= waveTimeInMinutes[i])
            {
                if (waveType[i] == EnemyType.SwarmBoys)
                    _BoidWave(waveOrigins[i], waveNumber[i]);
                else if (waveType[i] == EnemyType.RingDudes)
                    _RingWave(waveOrigins[i], waveNumber[i]);
                else if (waveType[i] == EnemyType.Mines)
                    _MineWave(waveOrigins[i], waveNumber[i]);
            }
        }

        _Update_RingWave();
        _Update_MineWave();
    }
    #region SwarmBoys
    private void _BoidWave(Vector3 location, int number)
    {
        effects.Scan();
        enemies.Spawn(EnemyType.SwarmBoys, location, number);
    }
    #endregion

    #region RingWave
    List<float> nextRingTime = new List<float>();
    Queue<Vector3> nextRingLoc = new Queue<Vector3>();
    Queue<int> nextRingNum = new Queue<int>();
    private void _RingWave(Vector3 location, int number)
    {
        effects.SpinningLight();
        nextRingTime.Add(0);
        nextRingNum.Enqueue(number);
        nextRingLoc.Enqueue(location);
    }

    private void _Update_RingWave()
    {
        if (nextRingTime.Count > 0)
        {
            for (int i = 0; i < nextRingTime.Count; i ++)
            {
                nextRingTime[i] += Time.deltaTime;
            }

            if (nextRingTime[0] >= 5)
            {
                nextRingTime.RemoveAt(0);
                enemies.Spawn(EnemyType.RingDudes, nextRingLoc.Dequeue(), nextRingNum.Dequeue());
            }
        }
    }
    #endregion

    #region Mines
    List<float> nextMineTime = new List<float>();
    Queue<Vector3> nextMineLoc = new Queue<Vector3>();
    Queue<int> nextMineNum = new Queue<int>();
    private void _MineWave(Vector3 location, int number)
    {
        effects.Light(0);
        nextMineTime.Add(0);
        nextMineNum.Enqueue(number);
        nextMineLoc.Enqueue(location);
    }

    private void _Update_MineWave()
    {
        if (nextMineTime.Count > 0)
        {
            for (int i = 0; i < nextMineTime.Count; i++)
            {
                nextMineTime[i] += Time.deltaTime;
            }

            if (nextMineTime[0] >= 5)
            {
                nextMineTime.RemoveAt(0);

                int num = nextMineNum.Dequeue();
                int rng;

                if (num <= 1)                   //point/random
                    rng = 0;
                else if (num <= 3)              //flank
                    rng = Random.Range(0, 2);
                else if (num <= 5)              //chevron
                    rng = Random.Range(0, 3);
                else                            //all
                    rng = Random.Range(0, 5);

                Debug.Log((MineWaveShapes)rng);

                if (rng == 0)
                {
                    Vector3 dqPos = nextMineLoc.Dequeue();
                    enemies.Spawn(EnemyType.Mines, ServiceLocator.instance.Player.position + dqPos, num);

                    if (num > 1)
                    {
                        for (int i = 1; i < num; i++)
                        {
                            Vector3 offset = Random.insideUnitCircle * 20;
                            offset.z = offset.y;
                            offset.y = 0;
                            enemies.Spawn(EnemyType.Mines, ServiceLocator.instance.Player.position + dqPos + offset, num);
                        }
                    }
                }
                else if (rng == 1)
                {
                    Vector3 dqPos = nextMineLoc.Dequeue();

                    for (int i = 0; i < num; i++)
                    {
                        float divisor = num - 1;
                        enemies.Spawn(EnemyType.Mines, 
                            ServiceLocator.instance.Player.position + 
                            dqPos + 
                            Vector3.right * -10 + 
                            Vector3.right * 20 * i / divisor, 
                            1);
                    }
                }
                else if (rng == 2)
                {
                    // temp uses 0
                    Vector3 dqPos = nextMineLoc.Dequeue();
                    enemies.Spawn(EnemyType.Mines, ServiceLocator.instance.Player.position + dqPos, num);

                    if (num > 1)
                    {
                        for (int i = 1; i < num; i++)
                        {
                            Vector3 offset = Random.insideUnitCircle * 20;
                            offset.z = offset.y;
                            offset.y = 0;
                            enemies.Spawn(EnemyType.Mines, ServiceLocator.instance.Player.position + dqPos + offset, num);
                        }
                    }
                }
                else if (rng == 3)
                {
                    // temp uses 0
                    Vector3 dqPos = nextMineLoc.Dequeue();
                    enemies.Spawn(EnemyType.Mines, ServiceLocator.instance.Player.position + dqPos, num);

                    if (num > 1)
                    {
                        for (int i = 1; i < num; i++)
                        {
                            Vector3 offset = Random.insideUnitCircle * 20;
                            offset.z = offset.y;
                            offset.y = 0;
                            enemies.Spawn(EnemyType.Mines, ServiceLocator.instance.Player.position + dqPos + offset, num);
                        }
                    }
                }
                else if (rng == 4)
                {
                    // temp uses 0
                    Vector3 dqPos = nextMineLoc.Dequeue();
                    enemies.Spawn(EnemyType.Mines, ServiceLocator.instance.Player.position + dqPos, num);

                    if (num > 1)
                    {
                        for (int i = 1; i < num; i++)
                        {
                            Vector3 offset = Random.insideUnitCircle * 20;
                            offset.z = offset.y;
                            offset.y = 0;
                            enemies.Spawn(EnemyType.Mines, ServiceLocator.instance.Player.position + dqPos + offset, num);
                        }
                    }
                }
            }
        }
    }

    private enum MineWaveShapes { Point, Flank, Chevron, LeftOblique, RightOblique}
    //System.Enum.GetValues(typeof(MineWaveShapes)).Length
    #endregion
}
