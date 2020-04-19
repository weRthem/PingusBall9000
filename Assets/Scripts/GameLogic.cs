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

	private int blueScore = 0;
	private int orangeScore = 0;

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

			playerComponent.playerCharacterController = pcc;
			PlayerCharacterController.localPlayer = pcc;
			pcc.MyPlayerAvatar = playerComponent;
			pcc.GetPlayersName();
		}

		NetworkManager.Instance.Networker.playerDisconnected += DisconnectPlayer;
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

			playerComponent.playerCharacterController = pcc;
			pcc.MyPlayerAvatar = playerComponent;

			pcc.networkObject.AssignOwnership(player);
			pcc.networkObject.SendRpc(PlayerCharacterControllerBehavior.RPC_GIVE_OWNER_TO_PLAYER, Receivers.AllBuffered, playerComponent.networkObject.NetworkId);
		});
	}
}
