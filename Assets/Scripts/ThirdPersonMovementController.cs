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

	}

	private void Update()
	{
		PlayerJump();
		Player player = GetComponent<Player>();

		player.networkObject.mouseX = mouseX;
		player.networkObject.horizontalAxis = horizontalMovement;
		player.networkObject.verticalAxis = verticalMovement;
		player.networkObject.position = transform.position;
		player.networkObject.rotation = transform.rotation;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		PlayerMovement();
		//GetComponent<Player>().networkObject.SendRpc(PlayerBehavior.RPC_SEND_PLAYERS_INPUT_DATA, BeardedManStudios.Forge.Networking.Receivers.Server, mouseX, horizontalMovement, verticalMovement);
	}


	private void PlayerMovement()
	{
		horizontalMovement = Input.GetAxis("Horizontal");
		verticalMovement = Input.GetAxis("Vertical");
	}

	private void PlayerJump()
	{
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		GetComponent<Player>().networkObject.SendRpc(PlayerBehavior.RPC_PLAYER_JUMP, BeardedManStudios.Forge.Networking.Receivers.Server);
	}
}
