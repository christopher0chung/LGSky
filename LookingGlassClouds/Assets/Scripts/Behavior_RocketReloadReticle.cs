using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_RocketReloadReticle : MonoBehaviour {

    private Model_Play playModel;
    private Model_Game gameModel;

    private List<GameObject> dots;
    private int max;
    private int currentInts;
    private float currentFloat;

    public Color reloaded;
    public Color reloading;
    public Material dotMat;

    void Awake()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }

	void Start () {
        max = transform.childCount;

        dots = new List<GameObject>();
		for (int i = 0; i < max; i++)
        {
            dots.Add(transform.GetChild(i).gameObject);
            dots[i].SetActive(false);
        }
	}
	

	void Update () {
        currentFloat = (1 - playModel.rocket_reloadProgress) * max;
        currentInts = (int)currentFloat;
        
        if (playModel.currentPlayerState == PlayerState.Alive)
        {
            if (gameModel.leftStation == Stations.Rockets || gameModel.rightStation == Stations.Rockets)
            {
                for (int i = 0; i < currentInts; i++)
                {
                    dots[i].SetActive(true);
                }
                if (currentInts != max)
                {
                    for (int i = currentInts; i < max; i++)
                    {
                        dots[i].SetActive(false);
                    }
                }
            }
            else
            {
                foreach (GameObject g in dots)
                    g.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject g in dots)
                g.SetActive(false);
        }

        if (currentInts == max)
            dotMat.SetColor("_HDRUnlit", reloaded);
        else
            dotMat.SetColor("_HDRUnlit", reloading);

	}
}
