using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_Score : MonoBehaviour
{
    public Text score;

    Model_ScoreAndDifficulty sadModel;

    private void Start()
    {
        sadModel = ServiceLocator.instance.Model.GetComponent<Model_ScoreAndDifficulty>();
    }

    void Update()
    {
        score.text = sadModel.score.ToString();
    }
}
