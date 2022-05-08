using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKnowledgePointInner : MonoBehaviour {
	[HideInInspector] public bool mouseTouching { get; private set; }

	public void OnEnter() {
		mouseTouching = true;
	}
	public void OnLeave() {
		mouseTouching = false;
	}
}
