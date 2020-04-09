using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] float rotationSpeed = 10f;
	[SerializeField] Transform target, player;
	bool isCameraInverted = false;
	float mouseX, mouseY;
	private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			isCameraInverted = !isCameraInverted;
		}
	}

	private void LateUpdate()
	{
		CameraControl();
	}

	void CameraControl()
	{
		if (isCameraInverted)
		{
			mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
			mouseY += Input.GetAxis("Mouse Y") * rotationSpeed;
		}
		else
		{
			mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
			mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
		}

		mouseY = Mathf.Clamp(mouseY, -35, 60);

		transform.LookAt(target);

		target.rotation = Quaternion.Euler(mouseY, mouseX, 0);

		player.GetComponent<ThirdPersonMovementController>().mouseX = mouseX;

		//player.rotation = Quaternion.Euler(0, mouseX, 0);
	}
}
