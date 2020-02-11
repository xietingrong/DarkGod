using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gameroot : MonoBehaviour {

	void Start () {
        UIPanelManager panelManager = UIPanelManager.Instance;
        panelManager.PushPanel(UIPanelType.MainMenu);
	}
}
