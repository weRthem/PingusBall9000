using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class PlayerCharacterController : PlayerCharacterControllerBehavior
{
	public static PlayerCharacterController localPlayer = null;

	// Start is called before the first frame update
	protected override void NetworkStart()
	{
		base.NetworkStart();

		if (!networkObject.IsOwner)
		{
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(false);
		}
	}

	private void Update()
	{

		//if (Player.player == null && localPlayer == null)
		//{
		//	localPlayer = Player.player;
		//}

		if (networkObject.IsOwner)
		{
			PlayerJump();
		}
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		if (networkObject.IsOwner)
		{
			PlayerMovement();
		}
	}


	private void PlayerMovement()
	{
		networkObject.horizontalAxis = Input.GetAxis("Horizontal");
		networkObject.verticalAxis = Input.GetAxis("Vertical");
	}

	private void PlayerJump()
	{
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		localPlayer.networkObject.SendRpc(PlayerBehavior.RPC_PLAYER_JUMP, BeardedManStudios.Forge.Networking.Receivers.Server);
	}
}
