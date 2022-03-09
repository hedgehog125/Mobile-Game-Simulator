using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppButton : MonoBehaviour {
	[SerializeField] private string sceneName;

    public void OnClick() {
		SceneManager.LoadScene(sceneName);
	}
}
