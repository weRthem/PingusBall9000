using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class ThirdPersonMovementController : MonoBehaviour
{
	public float mouseX;
	private float horizontalMovement;
	private float verticalMovement;
	// Start is called before the first frame update
	void Start()
    {
		//GetComponent<Player>().SetAxisDataFromPlayer(0, 0);
	}

	private void Update()
	{
		//PlayerJump();
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		PlayerMovement();
		GetComponent<Player>().networkObject.SendRpc(PlayerBehavior.RPC_SEND_PLAYERS_INPUT_DATA, BeardedManStudios.Forge.Networking.Receivers.Server, mouseX, horizontalMovement, verticalMovement);
	}


	private void PlayerMovement()
	{
		horizontalMovement = Input.GetAxis("Horizontal");
		verticalMovement = Input.GetAxis("Vertical");

		//GetComponent<Player>().SetPlayerAxis(horizontalMovement, verticalMovement);
	}

	private void PlayerJump()
	{
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		Ray ray = new Ray(transform.position, -Vector3.up);
		RaycastHit hitInfo;
		Debug.Log(Physics.Raycast(ray, out hitInfo, 1.1f));
		if (Physics.Raycast(ray, out hitInfo, 1.1f))
		{
			if (hitInfo.collider.gameObject.GetComponent<Player>()) return;

			//GetComponent<Rigidbody>().AddForce(0, jumpPower, 0);
		}
	}
}
