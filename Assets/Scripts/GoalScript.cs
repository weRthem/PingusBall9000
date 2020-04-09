using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class GoalScript : MonoBehaviour
{
	[SerializeField] bool isBlue = false;
	[SerializeField] GameLogic gameLogic = null;

	private void OnTriggerEnter(Collider other)
	{
		if (!gameLogic.networkObject.IsServer) return;
		if (!other.gameObject.GetComponent<GameBall>()) return;

		string scoringPlayer = other.gameObject.GetComponent<GameBall>().LastPlayerToTouch;

		gameLogic.networkObject.SendRpc(GameLogic.RPC_PLAYER_SCORED, BeardedManStudios.Forge.Networking.Receivers.AllBuffered, scoringPlayer, isBlue);

		other.GetComponent<GameBall>().Reset();
	}

}
