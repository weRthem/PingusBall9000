﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamePlateRotate : MonoBehaviour
{
	[SerializeField] Color orangeColor;
	[SerializeField] Color blueColor;
    // Update is called once per frame
    void LateUpdate()
    {
		if (!Player.player) return;

		Player myPlayer = GetComponentInParent<Player>();
		if (myPlayer.IsBlueTeam)
		{
			GetComponentInChildren<TextMesh>().color = blueColor;
		}
		else
		{
			GetComponentInChildren<TextMesh>().color = orangeColor;
		}

		transform.LookAt(Player.player.PlayerCameraTransform);
    }
}
