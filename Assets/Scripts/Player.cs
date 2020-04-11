﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	[SerializeField] float jumpPower = 350f;
	[SerializeField] float runBoost = 2f;
	[SerializeField] Slider runSlider = null;
	[SerializeField] private float maxRunEnergy = 60f;
	private float runEnergy = 0f;
	//private bool ranOutOfRun = false;
	private bool clientRanOutOfRun = false;
	private bool serverRanOutOfRun = false;

	private int blueTeamCount = 0;
	private int orangeTeamCount = 0;
	public bool IsBlueTeam = false;

	protected override void NetworkStart()
	{
		base.NetworkStart();

		if (networkObject.IsServer)
		{
			//ranOutOfRun = false;
			runEnergy = maxRunEnergy;
		}


		if (networkObject.IsServer && networkObject.IsOwner)
		{
			IsBlueTeam = true;
			blueTeamCount++;
			networkObject.SendRpc(RPC_UPDATE_PLAYER_TEAM, Receivers.AllBuffered, IsBlueTeam);
		}

		NetworkManager.Instance.Networker.playerAccepted += PlayerJoined;
		NetworkManager.Instance.Networker.disconnected += OnDisconnected;
		networkObject.UpdateInterval = 16;

		if (!networkObject.IsOwner)
		{
			//transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(4).gameObject.SetActive(false);
			GetComponent<ThirdPersonMovementController>().enabled = false;
			//Destroy(GetComponent<Rigidbody>());
		}
		else
		{
			player = this;
			GetPlayerName();
		}
	}

	private void Update()
	{
		if (networkObject.IsServer)
		{
			MovePlayer();
			ValidatePlayerPosition();
		}

		if (networkObject.IsOwner)
		{
			networkObject.isRunning = Input.GetKey(KeyCode.LeftShift);
			runSlider.value = runEnergy;

			/*if (runEnergy <= 0 && !clientRanOutOfRun)
			{
				Debug.Log("Setting runSlider tp red");
				clientRanOutOfRun = true;
				runSlider.transform.GetChild(2).GetComponentInChildren<Image>().color = Color.red;
			}else if (clientRanOutOfRun && runEnergy > maxRunEnergy / 4)
			{
				Debug.Log("Setting runSlider back to normal");
				runSlider.transform.GetChild(2).GetComponentInChildren<Image>().color = Color.white;
				clientRanOutOfRun = false;
			}*/
		}

		if (!networkObject.IsOwner && !networkObject.IsServer)
		{
			// change to a rpc that sends from the server
			transform.position = networkObject.position;
			transform.rotation = networkObject.rotation;
		}
	}

	private void RestoreRunEnergy()
	{
		if (runEnergy >= maxRunEnergy) return;

		runEnergy += 1 * Time.deltaTime;

		float runResetThreshold = maxRunEnergy * 0.25f;

		if (runEnergy > runResetThreshold)
		{
			serverRanOutOfRun = false;
		}
	}

	private void ValidatePlayerPosition()
	{
		if (Vector3.Distance(transform.position, networkObject.position) > 2f)
		{
			networkObject.SendRpc(RPC_SET_POS_TO_SERVER, Receivers.Owner, transform.position);
		}
	}

	private void MovePlayer()
	{
		float mouseX = networkObject.mouseX;
		float horizontalAxis = networkObject.horizontalAxis;
		float verticalAxis = networkObject.verticalAxis;

		horizontalAxis = Mathf.Clamp(horizontalAxis, -1f, 1f);
		verticalAxis = Mathf.Clamp(verticalAxis, -1f, 1f);

		float moveSpeed = walkSpeed;

		bool canRun = networkObject.isRunning && runEnergy > 0;
		bool isNotStandingStill = Mathf.Abs(horizontalAxis) > Mathf.Epsilon || Mathf.Abs(verticalAxis) > Mathf.Epsilon;

		if (canRun && isNotStandingStill && !serverRanOutOfRun)
		{
			moveSpeed += runBoost;
			runEnergy -= 4 * Time.deltaTime;
		}
		else if (runEnergy < 0 && !serverRanOutOfRun)
		{
			Debug.Log("Setting player to out of run");
			serverRanOutOfRun = true;
			RestoreRunEnergy();
		}
		else
		{
			RestoreRunEnergy();
		}

		Vector3 forwardVector = transform.forward * verticalAxis * moveSpeed;
		Vector3 sidewaysVector = transform.right * horizontalAxis * moveSpeed;

		Vector3 playerMovement = forwardVector + sidewaysVector;

		Rigidbody myRigidbody = GetComponent<Rigidbody>();
		playerMovement.y = myRigidbody.velocity.y;

		transform.rotation = Quaternion.Euler(0, mouseX, 0);

		networkObject.SendRpc(RPC_SET_PLAYERS_POS_AND_ROT, Receivers.ServerAndOwner, playerMovement, transform.rotation, runEnergy);
	}

	public void GetPlayerName()
	{
		if (!networkObject.IsOwner) return;

		Name = PlayerPrefs.GetString("PlayerName");

		networkObject.SendRpc(RPC_UPDATE_NAME, Receivers.AllBuffered, Name);
	}

	public override void UpdateName(RpcArgs args)
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

	public override void SetPlayersPosAndRot(RpcArgs args)
	{
		GetComponent<Rigidbody>().velocity = args.GetNext<Vector3>();
		transform.rotation = args.GetNext<Quaternion>();
		if (networkObject.IsOwner && !networkObject.IsServer)
		{
			runEnergy = args.GetNext<float>();
			Debug.Log(runEnergy);
		}
	}

	public override void PlayerJump(RpcArgs args)
	{
		Ray ray = new Ray(transform.position, -Vector3.up);
		RaycastHit hitInfo;
		//Debug.Log(Physics.Raycast(ray, out hitInfo, 1.1f));
		if (Physics.Raycast(ray, out hitInfo, 1.2f))
		{
			if (hitInfo.collider.gameObject.GetComponent<Player>()) return;

			GetComponent<Rigidbody>().AddForce(0, jumpPower, 0);
		}
	}

	public override void SetPosToServer(RpcArgs args)
	{
		Debug.Log("reseting player");
		transform.position = args.GetNext<Vector3>();
	}

	private void PlayerJoined(NetworkingPlayer player, NetWorker sender)
	{
		if (!networkObject.IsServer) return;

		Debug.Log("a player joined");
		if (blueTeamCount > orangeTeamCount)
		{
			orangeTeamCount++;
			IsBlueTeam = false;
		}
		else
		{
			blueTeamCount++;
			IsBlueTeam = true;
		}
		networkObject.SendRpc(RPC_UPDATE_PLAYER_TEAM, Receivers.AllBuffered, IsBlueTeam);
	}

	private void OnDisconnected(NetWorker sender)
	{
		NetworkManager.Instance.Networker.playerAccepted -= PlayerJoined;
		NetworkManager.Instance.Networker.disconnected -= OnDisconnected;

		if (networkObject.IsServer)
		{
			if (IsBlueTeam)
			{
				blueTeamCount--;
			}
			else
			{
				orangeTeamCount--;
			}
		}

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

	public override void UpdatePlayerTeam(RpcArgs args)
	{
		IsBlueTeam = args.GetNext<bool>();
		Debug.Log("Is blue team: " + IsBlueTeam);
	}
}
