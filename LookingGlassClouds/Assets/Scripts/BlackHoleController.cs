using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlackHoleController : MonoBehaviour {

    public Material blackHoleMaterial;

    [Range(0f, 1f)] public float effect;

    public float maxSize;

	void Update () {
        blackHoleMaterial.SetFloat("_SuctionScale", Mathf.Lerp(-3, 0, effect));
        if (effect < .2f)
        {
            transform.localScale = Vector3.one * (5 * effect * maxSize);
        }
        else if (effect >= .2f && effect < .8f)
            transform.localScale = Vector3.one * maxSize;
        else
            transform.localScale = Vector3.one * Mathf.Lerp(maxSize, 0, (effect - .8f) * 5);
	}
}
