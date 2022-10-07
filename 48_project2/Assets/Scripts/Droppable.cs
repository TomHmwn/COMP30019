using System.Collections;
using UnityEngine;

public class Droppable : MonoBehaviour
{


	public float waitTime = 5.0f;
	private void Start()
	{
		StartCoroutine(itemAlive());
	}
	public IEnumerator itemAlive()
	{

		yield return new WaitForSeconds(waitTime);
		Debug.Log("destroy");
		Destroy(this.gameObject);

	}
}