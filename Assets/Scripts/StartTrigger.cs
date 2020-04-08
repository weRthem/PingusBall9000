using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class StartTrigger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		if (FindObjectOfType<GameBall>() != null)
			Destroy(gameObject);
    }

	private void OnTriggerEnter(Collider other)
	{
		if (!NetworkManager.Instance.IsServer) return;
		if (other.GetComponent<Player>() == null) return;

		GameBall ball = NetworkManager.Instance.InstantiateGameBall() as GameBall;

		ball.Reset();

		Destroy(gameObject);
	}
}
