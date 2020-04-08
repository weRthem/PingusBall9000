using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class Player : PlayerBehavior
{

	public static Player player;

	public string Name { get; private set; }
	public Vector3 PlayerVelocity { get; private set; }
	public Transform PlayerCameraTransform;
	[SerializeField] TextMesh namePlate = null;
	[SerializeField] float walkSpeed = 5f;


	protected override void NetworkStart()
	{
		base.NetworkStart();

		NetworkManager.Instance.Networker.disconnected += OnDisconnected;
		networkObject.UpdateInterval = 16;

		if (!networkObject.IsOwner)
		{
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(4).gameObject.SetActive(false);
			GetComponent<ThirdPersonMovementController>().enabled = false;
			//Destroy(GetComponent<Rigidbody>());
		}
		else
		{
			player = this;
			GetPlayerName();
		}

		if (!networkObject.IsServer)
		{
			Destroy(GetComponent<Rigidbody>());
		}

	}

	private void Update()
	{
		/*
		if (!networkObject.IsOwner)
		{
			transform.position = networkObject.position;
			transform.rotation = networkObject.rotation;
			return;
		}

		networkObject.position = transform.position;
		networkObject.rotation = transform.rotation;
		*/
	}

	public void GetPlayerName()
	{
		if (!networkObject.IsOwner) return;

		Name = PlayerPrefs.GetString("PlayerName");

		networkObject.SendRpc(RPC_UPDATE_NAME, Receivers.AllBuffered, Name);
	}

	public override void updateName(RpcArgs args)
	{
		Debug.Log("called update name");
		Name = args.GetNext<string>();
		if (!networkObject.IsOwner)
		{
			namePlate.text = Name;
		}
		else
		{
			namePlate.gameObject.transform.parent.gameObject.SetActive(false);
		}
	}

	private void OnDisconnected(NetWorker sender)
	{
		NetworkManager.Instance.Networker.disconnected -= OnDisconnected;

		MainThreadManager.Run(() =>
		{
			foreach (var no in sender.NetworkObjectList)
			{
				if (no.Owner.IsHost)
				{
					BMSLogger.Instance.Log("Server disconnected");
					UnityEngine.SceneManagement.SceneManager.LoadScene(0);
				}
			}

			NetworkManager.Instance.Disconnect();
			Cursor.lockState = CursorLockMode.None;
		});
	}

	public override void SendPlayersInputData(RpcArgs args)
	{
		float mouseX = args.GetNext<float>();
		float horizontalAxis = args.GetNext<float>();
		float verticalAxis = args.GetNext<float>();

		horizontalAxis = Mathf.Clamp(horizontalAxis, -1, 1);
		verticalAxis = Mathf.Clamp(verticalAxis, -1, 1);

		Vector3 forwardVector = transform.forward * verticalAxis * walkSpeed;
		Vector3 sidewaysVector = transform.right * horizontalAxis * walkSpeed;

		Vector3 playerMovement = forwardVector + sidewaysVector;
		Rigidbody myRigidbody = GetComponent<Rigidbody>();
		playerMovement.y = myRigidbody.velocity.y;

		myRigidbody.velocity = playerMovement;
		transform.rotation = Quaternion.Euler(0, mouseX, 0);

		networkObject.SendRpc(RPC_SET_PLAYERS_POS_AND_ROT, Receivers.All, transform.position, transform.rotation);

	}

	public override void SetPlayersPosAndRot(RpcArgs args)
	{
		if (networkObject.IsServer) return;

		transform.position = args.GetNext<Vector3>();
		transform.rotation = args.GetNext<Quaternion>();
	}
}
