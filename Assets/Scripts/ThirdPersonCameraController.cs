using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] float rotationSpeed = 10f;
	[SerializeField] Transform target;

	Player player = null;
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
		if (Player.player != null && player == null)
		{
			player = Player.player;
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			isCameraInverted = !isCameraInverted;
		}

		if (player)
		{
			LerpToPlayer();
		}
	}

	private void LerpToPlayer()
	{
		Vector3 targetPos = player.transform.position;

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
		if (player == null) return;
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
