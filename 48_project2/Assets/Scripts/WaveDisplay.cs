using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveDisplay : MonoBehaviour
{
	public Spawner spawner;
	public Text waveDisplay;

	// Update is called once per frame
	void Update()
	{

		if (spawner != null)
		{
			if (spawner.isEndless)
			{
				waveDisplay.text = "WAVE " + Spawner.currentLevelNo.ToString();
				// print("Wave: " + Spawner.currentLevelNo.ToString());
			}
		}
	}
}
