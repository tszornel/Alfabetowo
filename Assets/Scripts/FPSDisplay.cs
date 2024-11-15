using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPSDisplay : MonoBehaviour
{
	//public Text fpsText;
	public TMP_Text fpsText;
	float deltaTime;
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		SetFPS();
	}
	void SetFPS()
	{
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		fpsText.text = string.Format("FPS: {0:00.} ({1:00.0} ms", fps, msec,")");
	}
}