using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetPlayerName : MonoBehaviour
{
	public const string playerNameSave = "PlayerName", characterTypeSave = "Character";

	[SerializeField] Text nameText;
	// Start is called before the first frame update
	private PlayableCharacters selectedCharacter = PlayableCharacters.DINGUS;

    void Start()
    {
		SceneManager.activeSceneChanged += SetName;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Application.targetFrameRate = 30;

		if (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
		{
			nameText.text = PlayerPrefs.GetString("PlayerName");
		}
	}

	void SetName(Scene arg0, Scene arg1)
	{
		string name = nameText.text;
		if (string.IsNullOrEmpty(name.Trim()))
		{
			name = "Player" + Random.Range(0, 1000);
		}

		PlayerPrefs.SetString(playerNameSave, name);
		PlayerPrefs.SetInt(characterTypeSave, (int)selectedCharacter);
		PlayerPrefs.Save();
	}

	public void LoadServerBrowser()
	{
		SceneManager.LoadScene(2);
	}

	public void SetCharacter(PlayableCharacters newSelectedCharacter)
	{
		Debug.Log(newSelectedCharacter);
		selectedCharacter = newSelectedCharacter;
	}
}
