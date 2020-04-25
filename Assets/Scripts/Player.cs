﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class Player : PlayerBehavior
{
	public PlayerCharacterController playerCharacterController = null;
	public string Name { get; private set; }
	public GameObject namePlateHolder = null;

	[SerializeField] float walkSpeed = 5f;
	[SerializeField] float jumpPower = 350f;
	[SerializeField] float runBoost = 2f;
	[SerializeField] Slider runSlider = null;
	[SerializeField] private float maxRunEnergy = 60f;
	public PlayerClasses playerClasses;
	private Dictionary<int, MonoBehaviour> testDict;

	private float runEnergy = 0f;
	private bool clientRanOutOfRun = false;
	private bool serverRanOutOfRun = false;

	public bool IsBlueTeam { get; set; } = false;

	public delegate void RightClickMethods(Vector3 startPos, Quaternion startRot);
	public RightClickMethods RightClickMethodsList;

	public delegate void LeftClickMethods(Vector3 startPos, Quaternion startRot);
	public LeftClickMethods LeftClickMethodsList;

	protected override void NetworkStart()
	{
		base.NetworkStart();

		if (networkObject.IsServer)
		{
			networkObject.isBlueTeam = IsBlueTeam;
			runEnergy = maxRunEnergy;
			NetworkManager.Instance.Networker.playerAccepted += PlayerJoined;
		}
		else
		{
			Invoke("GetTeam", 0.1f);
		}

		NetworkManager.Instance.Networker.disconnected += OnDisconnected;

		networkObject.UpdateInterval = 32;
	}

	private void Update()
	{
		if (!networkObject.IsServer)
		{
			transform.position = networkObject.position;
			transform.rotation = networkObject.rotation;

			if (IsBlueTeam != networkObject.isBlueTeam)
			{
				GetTeam();
			}

			if (GetComponent<Rigidbody>())
			{
				Destroy(GetComponent<Rigidbody>());
			}
			return;
		}

		MovePlayer();
	}

	private void GetTeam()
	{
		IsBlueTeam = networkObject.isBlueTeam;
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

	private void MovePlayer()
	{
		float mouseX = playerCharacterController.networkObject.mouseX;
		float horizontalAxis = playerCharacterController.networkObject.horizontalAxis;
		float verticalAxis = playerCharacterController.networkObject.verticalAxis;
		float moveSpeed = walkSpeed;

		horizontalAxis = Mathf.Clamp(horizontalAxis, -1f, 1f);
		verticalAxis = Mathf.Clamp(verticalAxis, -1f, 1f);

		bool canRun = playerCharacterController.networkObject.isPressingShift && runEnergy > 0;
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

		networkObject.runEnergy = runEnergy;

		Vector3 forwardVector = transform.forward * verticalAxis * moveSpeed;
		Vector3 sidewaysVector = transform.right * horizontalAxis * moveSpeed;

		Vector3 playerMovement = forwardVector + sidewaysVector;

		Rigidbody myRigidbody = GetComponent<Rigidbody>();
		playerMovement.y = myRigidbody.velocity.y;

		transform.rotation = Quaternion.Euler(0, mouseX, 0);

		myRigidbody.velocity = playerMovement;
		networkObject.position = transform.position;
		networkObject.rotation = transform.rotation;
	}

	public override void PlayerJump(RpcArgs args)
	{
		Ray ray = new Ray(transform.position, -Vector3.up);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1.2f))
		{
			if (hitInfo.collider.gameObject.GetComponent<Player>()) return;

			GetComponent<Rigidbody>().AddForce(0, jumpPower, 0);
		}
	}

	private void PlayerJoined(NetworkingPlayer player, NetWorker sender)
	{
		/*if (!networkObject.IsServer) return;

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
		networkObject.SendRpc(RPC_UPDATE_PLAYER_TEAM, Receivers.AllBuffered, IsBlueTeam);*/
	}

	private void OnDisconnected(NetWorker sender)
	{
		NetworkManager.Instance.Networker.playerAccepted -= PlayerJoined;
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

	public override void UpdatePlayersNameForClients(RpcArgs args)
	{
		MainThreadManager.Run(() =>
		{
			Name = args.GetNext<string>();
			namePlateHolder.gameObject.SetActive(true);
			GetComponentInChildren<TextMesh>().text = Name;
		});
	}

	public override void RightClick(RpcArgs args)
	{
		RightClickMethodsList(args.GetNext<Vector3>(), args.GetNext<Quaternion>());
	}

	public override void LeftClick(RpcArgs args)
	{
		LeftClickMethodsList(args.GetNext<Vector3>(), args.GetNext<Quaternion>());
	}
}
