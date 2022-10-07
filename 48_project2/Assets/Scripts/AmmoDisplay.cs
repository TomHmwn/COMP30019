using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
	public Player player;
	Gun gunInfo;
	Transform holdster;
	public Text ammoDisplay;
	public Text gunName;

	void Start()
	{
		foreach (var child in player.GetComponentsInChildren<Transform>())
		{
			if (child.gameObject.name == "Hold Weapon")
			{
				holdster = child;
			}
		}

        // holdster = player.GetComponentInChildren<Transform>();
        if(holdster!=null)
            gunInfo = holdster.GetComponentInChildren<Gun> ();
         if(gunInfo != null){
            // print(gunInfo.gameObject.name);
            ammoDisplay.text = gunInfo.projectilesRemaining().ToString() + "/" + gunInfo.totalProjectiles;
            // print(gunInfo.projectilesRemaining().ToString());
			if(gunInfo.gameObject.name.Contains("Handgun")){
				gunName.text = "HANDGUN";
			}
			if(gunInfo.gameObject.name.Contains("Machinegun")){
				gunName.text = "MACHINEGUN";
			}
			if(gunInfo.gameObject.name.Contains("Shotgun")){
				gunName.text = "SHOTGUN";
			}
         }

		gunInfo = holdster.GetComponentInChildren<Gun>();
		// print(gunInfo.gameObject.name);
	}
	void Update()
	{
		if (player)
			foreach (var child in player.GetComponentsInChildren<Transform>())
			{
				if (child.gameObject.name == "Hold Weapon")
				{
					holdster = child;
				}
			}

		// holdster = player.GetComponentInChildren<Transform>();
		if (holdster != null)
			gunInfo = holdster.GetComponentInChildren<Gun>();
		if (gunInfo != null)
		{
			// print(gunInfo.gameObject.name);
			ammoDisplay.text = gunInfo.projectilesRemaining().ToString() + "/" + gunInfo.totalProjectiles;
			if(gunInfo.gameObject.name.Contains("Handgun")){
				gunName.text = "HANDGUN";
			}
			if(gunInfo.gameObject.name.Contains("Machinegun")){
				gunName.text = "MACHINEGUN";
			}
			if(gunInfo.gameObject.name.Contains("Shotgun")){
				gunName.text = "SHOTGUN";
			}

			// print(gunInfo.projectilesRemaining().ToString());
		}

	}
}
