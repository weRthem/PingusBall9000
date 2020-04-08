using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class GameBall : GameBallBehavior
{
	private ulong updateTime = 16;
	private Rigidbody rigidbodyRef = null;
	private GameLogic gameLogic = null;

    // Start is called before the first frame update
    void Start()
    {
		rigidbodyRef = GetComponent<Rigidbody>();
		gameLogic = GameLogic.Instance;
		networkObject.UpdateInterval = updateTime;

		if (!networkObject.IsServer) {
			Destroy(GetComponent<Rigidbody>());
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (!networkObject.IsOwner)
		{
			transform.position = networkObject.position;
			transform.rotation = networkObject.rotation;
			return;
		}
		
		networkObject.rotation = transform.rotation;
		networkObject.position = transform.position;
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (!networkObject.IsServer) return;

		if (collision.gameObject.GetComponent<Player>() == null) return;

		if (collision.gameObject.GetComponent<Player>().networkObject.IsOwner) return;

		// try crossing the balls pos with the playerVelocity?
		//Vector3 relativeForceIdea = Vector3.Cross(transform.position, collision.transform.position) + collision.gameObject.GetComponent<Player>().PlayerVelocity;

		//Vector3 relativeForceIdea = new Vector3(xVel, yVel, zVel);

		//relativeForceIdea = relativeForceIdea.normalized;

		//wrigidbodyRef.AddForce(relativeForceIdea * Vector3.Magnitude(collision.gameObject.GetComponent<Player>().PlayerVelocity), ForceMode.Impulse);
	}

	public void Reset()
	{
		transform.position = Vector3.up * 10;

		GetComponent<Rigidbody>().velocity = Vector3.zero;

		Vector3 force = new Vector3(0, 0, 0);
		force.x = Random.Range(300, 500);
		force.z = Random.Range(300, 500);

		if (Random.value < 0.5f)
			force.x *= -1;

		if (Random.value < 0.5f)
			force.z *= -1;

		GetComponent<Rigidbody>().AddForce(force);
	}
}
