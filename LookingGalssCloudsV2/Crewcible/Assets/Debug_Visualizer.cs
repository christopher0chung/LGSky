using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Visualizer : MonoBehaviour
{
    public GameObject prefab;

    float tuneIntervalX = .2f;
    float tuneIntervalY = .2f;

    float z;
    float sweepTimerX;
    float sweepTimerY;

    float oldProjectionX;
    float projectionX;


    float oldProjectionY;
    float projectionY;

    float currentX;
    float currentY;

    Vector3 lastPos;
    Vector3 currentPos;

    Quaternion currentRot;

    private void Start()
    {
        sweepTimerX = Random.Range(0.00f, tuneIntervalX);
    }

    private void FixedUpdate()
    {
        z += Time.fixedDeltaTime + z * .005f;

        sweepTimerX += Time.fixedDeltaTime;
        if (sweepTimerX >= tuneIntervalX)
        {
            sweepTimerX -= tuneIntervalX;
            oldProjectionX = projectionX;
            projectionX = oldProjectionX + Random.Range(-1.00f, 1.00f) * Vector3.Distance(currentPos / 20, Vector3.zero);
            tuneIntervalX = Random.Range(.2f, .3f);
        }

        sweepTimerY += Time.fixedDeltaTime;
        if (sweepTimerY >= tuneIntervalY)
        {
            sweepTimerY -= tuneIntervalY;
            oldProjectionY = projectionY;
            projectionY = oldProjectionY + Random.Range(-1.00f, 1.00f) * Vector3.Distance(currentPos / 20, Vector3.zero);
            tuneIntervalY = Random.Range(.2f, .3f);
        }

        lastPos = currentPos;
        currentPos.x = currentX;
        currentPos.y = currentY;
        currentPos.z = z;

        currentX = Mathf.Lerp(oldProjectionX, projectionX, Easings.SineEaseInOut(sweepTimerX/tuneIntervalX));
        currentY = Mathf.Lerp(oldProjectionY, projectionY, Easings.SineEaseInOut(sweepTimerX/tuneIntervalY));

        GameObject g = Instantiate(prefab, currentPos, Quaternion.identity);
        currentRot = Quaternion.LookRotation(currentPos - lastPos);
        g.transform.rotation = currentRot;
    }
}
