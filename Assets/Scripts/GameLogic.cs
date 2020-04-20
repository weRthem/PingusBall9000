using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class GameLogic : GameLogicBehavior
{
	[SerializeField] Text scoreLabel;
	[SerializeField] Text OrangeScoreText;
	[SerializeField] Text BlueScoreText;

	private uint blueScore = 0;
	private uint orangeScore = 0;

	private uint bluePlayers = 0;
	private uint orangePlayers = 0;

	public static GameLogic Instance;
	// Start is called before the first frame update
	void Start()
    {
		QualitySettings.vSyncCount = 1;
		Instance = this;
    }

	protected override void NetworkStart()
	{
		base.NetworkStart();


		if (networkObject.IsServer)
		{
			NetworkManager.Instance.Networker.playerAccepted += PlayerConnected;

			PlayerBehavior newPlayer = NetworkManager.Instance.InstantiatePlayer(position: new Vector3(0, 7, 0));
			PlayerCharacterControllerBehavior newPlayerController = NetworkManager.Instance.InstantiatePlayerCharacterController(position: new Vector3(0, 0, 0));

			Player playerComponent = newPlayer.GetComponent<Player>();
			PlayerCharacterController pcc = newPlayerController.GetComponent<PlayerCharacterController>();

			playerComponent.IsBlueTeam = true;
			bluePlayers++;
			Debug.Log("Added a blue Player. Blue player count: " + bluePlayers);

			playerComponent.playerCharacterController = pcc;
			PlayerCharacterController.localPlayer = pcc;
			pcc.MyPlayerAvatar = playerComponent;
			pcc.GetPlayersName();
		}

		NetworkManager.Instance.Networker.playerDisconnected += DisconnectPlayer;
	}

	public void PlayerDisconnected(bool playerWasBlueTeam)
	{
		if (playerWasBlueTeam)
		{
			bluePlayers--;
		}
		else
		{
			orangePlayers--;
		}
	}

	public override void PlayerScored(RpcArgs args)
	{
		string playerName = args.GetNext<string>();
		bool orangeScored = args.GetNext<bool>();

		if (orangeScored)
		{
			orangeScore++;
			OrangeScoreText.text = orangeScore.ToString();
		}
		else
		{
			blueScore++;
			BlueScoreText.text = blueScore.ToString();
		}

		scoreLabel.text = playerName + " scored the last point";
	}

	public override void PlayerDisconnected(RpcArgs args)
	{
		bool playerWasBlueTeam = args.GetNext<bool>();

		if (playerWasBlueTeam)
		{
			bluePlayers--;
		}
		else
		{
			orangePlayers--;
		}

		Debug.Log("Orange Players: " + orangePlayers + " Blue Players: " + bluePlayers);
	}

	private void DisconnectPlayer(NetworkingPlayer player, NetWorker sender)
	{
		Debug.Log("Player: " + player.Name + " disconnected");

		MainThreadManager.Run(() =>
		{
			List<NetworkObject> toDelete = new List<NetworkObject>();
			foreach (NetworkObject netObject in sender.NetworkObjectList)
			{
				if (netObject.Owner == player)
				{
					toDelete.Add(netObject);
				}
			}


			if (toDelete.Count > 0)
			{
				for (int i = toDelete.Count - 1; i >= 0; i--)
				{
					toDelete[i].SendRpc(PlayerCharacterControllerBehavior.RPC_DESTROY_PLAYER, Receivers.All);
				}
			}

		});
	}

	private void PlayerConnected(NetworkingPlayer player, NetWorker sender)
	{
		MainThreadManager.Run(() =>
		{
			PlayerBehavior newPlayer = NetworkManager.Instance.InstantiatePlayer(position: new Vector3(0, 7, 0));
			PlayerCharacterControllerBehavior newPlayerController = NetworkManager.Instance.InstantiatePlayerCharacterController(position: new Vector3(0, 0, 0));

			Player playerComponent = newPlayer.GetComponent<Player>();
			PlayerCharacterController pcc = newPlayerController.GetComponent<PlayerCharacterController>();

			if (bluePlayers > orangePlayers)
			{
				playerComponent.IsBlueTeam = false;
				orangePlayers++;
				Debug.Log("Added a orange Player. orange player count: " + orangePlayers);
			}
			else
			{
				playerComponent.IsBlueTeam = true;
				bluePlayers++;
				Debug.Log("Added a blue Player. Blue player count: " + bluePlayers);
			}

			playerComponent.playerCharacterController = pcc;
			pcc.MyPlayerAvatar = playerComponent;

			pcc.networkObject.AssignOwnership(player);
			pcc.networkObject.SendRpc(PlayerCharacterControllerBehavior.RPC_SET_UP_NEW_PLAYER, Receivers.AllBuffered, playerComponent.networkObject.NetworkId);
		});
	}
}
