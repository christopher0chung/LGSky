using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset_BoidsManager : MonoBehaviour {

    public GameObject boid;
    //public int numberOfBoids;

    public int[] boidsPerFlock = new int[1];

    [Range(0f, 1f)]
    public float _weightRandom;
    [Range(0f, 1f)]
    public float _weightSeparation;
    [Range(0f, 1f)]
    public float _weightAlightment;
    [Range(0f, 1f)]
    public float _weightCohesion;
    [Range(0f, 1f)]
    public float _weightPOI;

    #region Private Variables
    private List<List<Asset_Boid>> flocks = new List<List<Asset_Boid>>();
    //private List<Asset_Boid> boids = new List<Asset_Boid>();
    private Vector3 newPOI = new Vector3(0, 5, 10);
    //private float newPOITime;
    //private float newPOITimer;

    #endregion


    void Start () {

        //_GetNewPOI();

        for (int j = 0; j < boidsPerFlock.Length; j++)
        {
            flocks.Add(new List<Asset_Boid>());

            for (int i = 0; i < boidsPerFlock[j]; i++)
            {
                GameObject b = Instantiate(boid, newPOI + Random.insideUnitSphere * 5 + Vector3.forward * 20, Quaternion.identity, null);
                Asset_Boid bd = b.GetComponent<Asset_Boid>();

                bd.ManagerRegister(this, j);
                flocks[j].Add(bd);
            }
        }

        //for (int i = 0; i < numberOfBoids; i++)
        //      {
        //          GameObject b = Instantiate(boid, newPOI + Random.insideUnitSphere * 5 + Vector3.forward * 20, Quaternion.identity, null);
        //          Asset_Boid bd = b.GetComponent<Asset_Boid>();

        //          bd.ManagerRegister(this);
        //          boids.Add(bd);
        //      }
        _SetTuningValues();

        //newPOITime = Random.Range(0.0f, 15.0f);
        //newPOITimer = Random.Range(0.0f, 15.0f);

        _SetNewPOI();
	}

    private float timer;

    //public void Update()
    //{
    //    timer += Time.deltaTime;

    //    if (timer > 10)
    //    {
    //        List<Asset_Boid> newFlock = new List<Asset_Boid>();
    //        for (int i = 0; i < 50; i++)
    //        {
    //            GameObject g = Instantiate(boid, newPOI + Random.insideUnitSphere * 5 + Vector3.forward * 20, Quaternion.identity, null);
    //            g.GetComponent<Asset_Boid>().ManagerRegister(this, flocks.Count - 1);
    //            newFlock.Add(g.GetComponent<Asset_Boid>());
    //        }
    //        flocks.Add(newFlock);
    //        timer = 0;
    //        Debug.Log("NEw FLocksokdosk");
    //    }
    //}

    //public void Update()
    //{
    //    // Sends new Point of Interest to managed flock on variable interval

    //    newPOITimer += Time.deltaTime;

    //    if(newPOITimer >= newPOITime)
    //    {
    //        newPOITimer = 0;
    //        newPOITime = Random.Range(8.0f, 15.0f);
    //        _GetNewPOI();
    //        _SetNewPOI();
    //    }
    //}

    #region Public Functions

    public List<Asset_Boid> RequestMyNeighbors(Asset_Boid b)
    {
        // Using manager based neighbor calulation for better scheduling
        // Permits significanly greater number of simultaneously active boids with less chance of variable frame hits

        // Initialize list to return
        List<Asset_Boid> _neighborsToReturn = new List<Asset_Boid>();

        // Iterate through boid's flock
        for (int i = 0; i < flocks[b.flockIndex].Count; i++)
        {
            // For every boid that is not the requester, assess
            if (b != flocks[b.flockIndex][i])
            {
                // Add every boid that is within 20 units of requester
                if (Vector3.Distance(b.transform.position, flocks[b.flockIndex][i].transform.position) <= 20)
                {
                    _neighborsToReturn.Add(flocks[b.flockIndex][i]);
                }

                // Ceiling of list length is 21 to constrain size
                if (_neighborsToReturn.Count > 20)
                    return (_neighborsToReturn);
            }
        }

        // If 21 is not reached, it is must be less
        // If 0, return null instead of empty list
        // If 1-20, return list contents
        if (_neighborsToReturn.Count == 0)
            return null;
        else
            return _neighborsToReturn;
    }

    public void DestructionNotice(Asset_Boid boid)
    {
        foreach(List<Asset_Boid> flock in flocks)
        {
            if (flock.Contains(boid))
            {
                flocks[boid.flockIndex].Remove(boid);
                foreach (Asset_Boid b in flocks[boid.flockIndex])
                {
                    b.IfTrackingRemoveFromTracker(boid);
                }
            }
        }
    }
    #endregion

    #region Internal Functions

    private void _SetTuningValues()
    {
        foreach (List<Asset_Boid> l in flocks)
        {
            foreach (Asset_Boid b in l)
            {
                b.UpdateWeights(_weightRandom, _weightSeparation, _weightAlightment, _weightCohesion, _weightPOI);
            }
        }
    }

    private void _GetNewPOI()
    {
        newPOI = Random.insideUnitSphere * 10;
    }

    private void _SetNewPOI()
    {
        foreach (List<Asset_Boid> l in flocks)
        {
            foreach (Asset_Boid b in l)
            {
                b.SetNewPOI(newPOI);
            }
        }
    }

    #endregion
}
