using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeRotation : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		RandomizeYrotation();
	}

	void RandomizeYrotation()
	{
		Quaternion randYRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
		transform.rotation = randYRotation;
	}
}
