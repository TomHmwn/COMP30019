using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	public ParticleSystem teleportEffect;

	Transform target;
	public int distance = 10;
	public float cooldown = 5f;
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(CountDown());
		target = GameObject.FindGameObjectWithTag("Player")?.transform;
	}

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator CountDown()
	{
		while (true)
		{
			yield return new WaitForSeconds(cooldown);
			if (target == null) continue;
			print("Teleporting...");
			Vector3 offset = new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f)).normalized;
			Vector3 newPosition = target.position + offset * distance;

			if (Vector3.Distance(transform.position, target.position) > distance)
			{
				transform.position = newPosition;
			}
			Destroy(Instantiate(teleportEffect.gameObject, transform.position, Quaternion.identity), teleportEffect.main.startLifetime.constant);
		}



	}
}
