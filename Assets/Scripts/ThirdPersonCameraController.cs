using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] float rotationSpeed = 10f;
	[SerializeField] Transform target;

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
		if (!GetComponentInParent<PlayerCharacterController>().MyPlayerAvatar) return;

		if (Input.GetKeyDown(KeyCode.I))
		{
			isCameraInverted = !isCameraInverted;
		}

		LerpToPlayer();
	}

	private void LerpToPlayer()
	{
		Vector3 targetPos = GetComponentInParent<PlayerCharacterController>().MyPlayerAvatar.transform.position;

		targetPos.Set(targetPos.x, targetPos.y + 1.3f, targetPos.z);

		if (targetPos == target.position) return;

		target.position = Vector3.SmoothDamp(target.position, targetPos, ref velocity, 0.001f);
	}

	private void LateUpdate()
	{
		CameraControl();
	}

	void CameraControl()
	{
		if (!GetComponentInParent<PlayerCharacterController>().MyPlayerAvatar) return;
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

		mouseY = Mathf.Clamp(mouseY, -45, 60);

		transform.LookAt(target);

		target.rotation = Quaternion.Euler(mouseY, mouseX, 0);

		GetComponentInParent<PlayerCharacterController>().networkObject.mouseX = mouseX;
	}
}
