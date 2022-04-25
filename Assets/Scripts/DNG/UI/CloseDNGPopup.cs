using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDNGPopup : MonoBehaviour {
	[SerializeField] private GameObject popup;

    public void OnClick() {
		popup.SetActive(false);
	}
}
