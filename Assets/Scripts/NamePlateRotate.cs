using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamePlateRotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		transform.LookAt(Player.player.PlayerCameraTransform);
    }
}
