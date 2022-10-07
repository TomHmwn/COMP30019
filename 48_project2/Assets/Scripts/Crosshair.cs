using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
	public LayerMask targetMask;
	public Color highlightedDotColor;
	public SpriteRenderer dot;
	Color initialDotColor;

	void Start()
	{
		Cursor.visible = false;
		initialDotColor = dot.color;
	}

	public void gottenTarget(Ray ray)
	{
		if (Physics.Raycast(ray, 300f, targetMask))
		{
			dot.color = highlightedDotColor;
		}
		else
		{
			dot.color = initialDotColor;
		}
	}
	// Update is called once per frame
	void Update()
	{
		transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
	}
}
