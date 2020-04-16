using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

public class PlayerCharacterController : PlayerCharacterControllerBehavior
{
	public static PlayerCharacterController localPlayer = null;
	public Player MyPlayerAvatar { get; set; }

	private void Start()
	{

	}

	protected override void NetworkStart()
	{
		base.NetworkStart();

		networkObject.UpdateInterval = 16;
	}

	private void Update()
	{
		if (networkObject.IsServer)
		{
			Debug.Log(MyPlayerAvatar);
		}

		if (networkObject.IsOwner)
		{
			PlayerJump();
		}
		else if (transform.GetChild(0).gameObject.activeInHierarchy)
		{
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(false);
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

		MyPlayerAvatar.networkObject.SendRpc(PlayerBehavior.RPC_PLAYER_JUMP, Receivers.Server);
	}

	public void StartUpPlayer()
	{
		networkObject.SendRpc(RPC_GIVE_OWNER_TO_PLAYER, Receivers.AllBuffered, MyPlayerAvatar.networkObject.NetworkId);
	}

	public override void GiveOwnerToPlayer(RpcArgs args)
	{
		uint playerID = args.GetNext<uint>();

		Player[] players = FindObjectsOfType<Player>();

		Debug.Log(playerID);
		localPlayer = this;

		foreach (Player p in players)
		{
			Debug.Log(p.networkObject.NetworkId);

			if (p.networkObject.NetworkId == playerID)
			{
				MyPlayerAvatar = p;
				break;
			}
		}

		if (networkObject.IsOwner)
		{
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).gameObject.SetActive(true);
			MyPlayerAvatar.GetPlayerName();
		}
	}

	public override void DestroyPlayer(RpcArgs args)
	{
		NetworkManager.Instance.Networker.NetworkObjectList.Remove(MyPlayerAvatar.networkObject);
		MyPlayerAvatar.networkObject.Destroy();
		NetworkManager.Instance.Networker.NetworkObjectList.Remove(networkObject);
		networkObject.Destroy();
	}
}
