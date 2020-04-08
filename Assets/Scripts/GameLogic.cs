using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class GameLogic : MonoBehaviour //GameLogicBehavior
{
	[SerializeField] Text scoreLabel;

	public static GameLogic Instance;
	// Start is called before the first frame update
	void Start()
    {
		QualitySettings.vSyncCount = 1;
		Instance = this;
		//NetworkManager.Instance.InstantiatePlayer(position: new Vector3(0, 5, 0));
		//NetworkManager.Instance.Networker.playerDisconnected += DisconnectPlayer;
    }

	//public override void PlayerScored(RpcArgs args)
	//{
	//	string playerName = args.GetNext<string>();

	//	scoreLabel.text = playerName + " scored the last point";
	//}

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
					sender.NetworkObjectList.Remove(toDelete[i]);
					toDelete[i].Destroy();
				}
			}

		});
	}
}
