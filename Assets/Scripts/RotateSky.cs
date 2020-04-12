using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSky : MonoBehaviour
{
	public float rotationSpeed = 5f;

    void Update()
    {
		RenderSettings.skybox.SetFloat("_Rotation", rotationSpeed * Time.time);
    }
}
