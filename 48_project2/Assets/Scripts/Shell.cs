using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
	public Rigidbody myRigidbody;
	public float forceMin;
	public float forceMax;
	float lifeTime = 4;
	float fadeTime = 2;
	// Start is called before the first frame update
	void Start()
	{
		// shell flying out randomly out of gun
		float force = Random.Range(forceMin, forceMax);
		myRigidbody.AddTorque(Random.insideUnitSphere * force);
		myRigidbody.AddForce(transform.right * force);
		StartCoroutine(Fade());
	}

	// Update is called once per frame
	void Update()
	{

	}
	IEnumerator Fade()
	{
		yield return new WaitForSeconds(lifeTime);

		float fadePercent = 0;
		float fadeSpeed = 1 / fadeTime;
		Material mat = GetComponent<Renderer>().material;
		Color originalColor = mat.color;

		while (fadePercent < 1)
		{
			fadePercent += Time.deltaTime * fadeSpeed;
			mat.color = Color.Lerp(originalColor, Color.clear, fadePercent);
			yield return null;
		}
		Destroy(gameObject);
	}
}
