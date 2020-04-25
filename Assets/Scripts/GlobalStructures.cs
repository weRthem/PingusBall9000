
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayableCharacters { DINGUS, DOOPER, HOOPER};

[System.Serializable]
public struct PlayerClasses
{
	public PlayableCharacters character;
	public MonoBehaviour characterClass;
}
