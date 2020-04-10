using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetPlayerName : MonoBehaviour
{
	[SerializeField] Text nameText;
    // Start is called before the first frame update
    void Start()
    {
		SceneManager.activeSceneChanged += SetName;
		Cursor.lockState = CursorLockMode.None;
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

		PlayerPrefs.SetString("PlayerName", name);
		PlayerPrefs.Save();
	}
}
