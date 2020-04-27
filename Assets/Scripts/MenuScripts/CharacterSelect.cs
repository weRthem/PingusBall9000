using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
	[SerializeField] SetPlayerName playerName = null;

	[SerializeField] PlayableCharacters thisCharacter;

	void Start()
	{
		GetComponent<Toggle>().onValueChanged.AddListener(SelectCharacter);
	}

	private void SelectCharacter(bool isToggledOn)
	{
		if (!isToggledOn) return;
		playerName.SetCharacter(thisCharacter);
	}
}
