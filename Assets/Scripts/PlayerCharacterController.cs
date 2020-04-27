using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

public class PlayerCharacterController : PlayerCharacterControllerBehavior
{
	public static PlayerCharacterController localPlayer = null;
	public Player MyPlayerAvatar { get; set; }
	public string playerName { get; private set; } = "";

	[SerializeField] Slider runSlider = null;
	[SerializeField] Image sliderHandle = null;

	private bool hasRunOutOfRun = false;

	private void Start()
	{

	}

	protected override void NetworkStart()
	{
		base.NetworkStart();

		// TODO send RPC to all through the player script that updates the material and applies the correct Monobehaviour to them as well

		networkObject.UpdateInterval = 16;
	}

	private void Update()
	{
		if (networkObject.IsOwner)
		{
			PlayerJump();
			runSlider.value = MyPlayerAvatar.networkObject.runEnergy;

			if (MyPlayerAvatar.networkObject.runEnergy < 0 && !hasRunOutOfRun)
			{
				hasRunOutOfRun = true;
				sliderHandle.color = Color.red;
			}else if (MyPlayerAvatar.networkObject.runEnergy > runSlider.maxValue / 4 && hasRunOutOfRun)
			{
				hasRunOutOfRun = false;
				sliderHandle.color = Color.white;
			}

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
			networkObject.isPressingShift = Input.GetKey(KeyCode.LeftShift);
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

	public void GetPlayersName()
	{
		playerName = PlayerPrefs.GetString(SetPlayerName.playerNameSave);
		PlayableCharacters playerCharacter = (PlayableCharacters)PlayerPrefs.GetInt(SetPlayerName.characterTypeSave);
		Debug.Log(playerName);
		Debug.Log(playerCharacter);
		networkObject.SendRpc(RPC_SEND_PLAYER_NAME_AND_CLASS_TO_ALL_CLIENTS, Receivers.Server, playerName, (int)playerCharacter);
	}

	public override void SetUpNewPlayer(RpcArgs args)
	{
		MainThreadManager.Run(() =>
		{
			uint playerID = args.GetNext<uint>();

			Player[] players = FindObjectsOfType<Player>();

			Debug.Log(playerID);

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
				GetPlayersName();
				localPlayer = this;
			}
		});
	}

	public override void DestroyPlayer(RpcArgs args)
	{
		MainThreadManager.Run(() => 
		{
			if (networkObject.IsServer)
			{
				Debug.Log("I am server");
				GameLogic.Instance.PlayerDisconnected(MyPlayerAvatar.IsBlueTeam);
				//GameLogic.Instance.networkObject.SendRpc(GameLogic.RPC_PLAYER_DISCONNECTED, Receivers.Server, MyPlayerAvatar.IsBlueTeam); GameLogic.Instance.networkObject.SendRpc(GameLogic.RPC_PLAYER_DISCONNECTED, Receivers.Server, MyPlayerAvatar.IsBlueTeam);
			}
			NetworkManager.Instance.Networker.NetworkObjectList.Remove(MyPlayerAvatar.networkObject);
			MyPlayerAvatar.networkObject.Destroy();
			NetworkManager.Instance.Networker.NetworkObjectList.Remove(networkObject);
			networkObject.Destroy();
		});

	}

	public override void SendPlayerNameAndClassToAllClients(RpcArgs args)
	{

		MainThreadManager.Run(() =>
		{
			Debug.Log("you");

			playerName = args.GetNext<string>();
			MyPlayerAvatar.namePlateHolder.SetActive(true);
			MyPlayerAvatar.namePlateHolder.GetComponentInChildren<TextMesh>().text = playerName;
			MyPlayerAvatar.SetPlayerClass((PlayableCharacters)args.GetNext<int>());
			// Move this part to the Player to be called after setting up the character class
			//MyPlayerAvatar.networkObject.SendRpc(Player.RPC_UPDATE_PLAYERS_NAME_FOR_CLIENTS, Receivers.AllBuffered, playerName);
		});
	}
}
