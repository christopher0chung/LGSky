using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_FractionalNormalization : MonoBehaviour
{
    public List<float> a;
    public List<float> b;

    public int iterationA;
    public int iterationB;

    private float scratch;

    void Start()
    {
        a = new List<float>();
        b = new List<float>();

        scratch = 1;
        for (int i = 0; i < iterationA; i++)
        {
            scratch = Mathf.Lerp(scratch, 0, .03f);
            a.Add(scratch);
        }

        scratch = 1;
        for (int i = 0; i < iterationB; i++)
        {
            float tempA = (float)iterationA;
            float tempB = (float)iterationB;

            float ratio = tempB / tempA;

            scratch = Mathf.Lerp(scratch, 0, .03f / ratio);
            //ratio is fraction of time -> divide by time
            //same as mult by deltaTime
            b.Add(scratch);
        }
    }
}
