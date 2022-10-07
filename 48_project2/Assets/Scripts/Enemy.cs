using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Alive
{

	public enum State { Idle, Chasing, Attacking, Wander };
	State currentState;

	public ParticleSystem deathEffect;
	public static event System.Action OnDeathStatic;

	public float speed = 5;
	NavMeshAgent pathfinder;

	public float agroDistance = 20f;
	Transform target;
	Alive targetLife;
	Material skin;

	Color originalColour;

	float attackDistanceThreshold = .5f;
	float timeBetweenAttacks = 1;

	public float damage = 1;

	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;

	bool hasTarget;
	ItemDrop getItem;

	public int enemyScore;

	Material[] enemyMats;

	Transform enemy;
	float rotateSpeed = 8.0f;

	Rigidbody myRigidbody;

	protected override void Start()
	{
		base.Start();
		myRigidbody = GetComponent<Rigidbody>();
		getItem = GetComponent<ItemDrop>();

		enemyMats = GetComponentsInChildren<Renderer>().Select(x => x.material).ToArray();

		pathfinder = GetComponent<NavMeshAgent>();

		foreach (var child in pathfinder.GetComponentsInChildren<Renderer>())
		{

			if (child.gameObject.name == "SM_Robot_001")
			{
				skin = child.material;
				break;
			}
		}
		// skin = pathfinder.GetComponentInChildren<Renderer> ().material;
		originalColour = skin.color;

		// if target exist
		if (GameObject.FindGameObjectWithTag("Player") != null)
		{

			// currentState = State.Chasing;
			hasTarget = true;

			target = GameObject.FindGameObjectWithTag("Player").transform;
			targetLife = target.GetComponent<Alive>();
			targetLife.OnDeath += onTargetDeath;

			myCollisionRadius = GetComponent<CapsuleCollider>().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

			foreach (CapsuleCollider collider in this.GetComponentsInChildren<CapsuleCollider>())
				Physics.IgnoreCollision(target.GetComponent<CapsuleCollider>(), collider);
		}
		StartCoroutine(UpdatePath());
		StartCoroutine(Wander());

	}

	void Update()
	{
		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			float distToTarget = Vector3.Distance(target.position, transform.position);
			if ((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude < agroDistance)
			{
				currentState = State.Chasing;
			}
			else
			{
				currentState = State.Wander;
				float wanderSpeed = 1f;
				transform.position += transform.forward * wanderSpeed * Time.deltaTime;

			}

		}


		foreach (Material enemyMat in enemyMats)
		{
			if (enemyMat != null)
			{

				float dissolveVal = enemyMat.GetFloat("_Amount");
				dissolveVal -= 0.01f;
				enemyMat.SetFloat("_Amount", dissolveVal);
			}
		}

		// Make enemy material darker based on its health
		skin.color = originalColour * (health / startingHealth);
		if (hasTarget)
		{

			if (Time.time > nextAttackTime)
			{
				float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
				{
					nextAttackTime = Time.time + timeBetweenAttacks;
					AudioManager.instance.PlaySound("Enemy Attack", transform.position);
					StartCoroutine(Attack());
				}

			}
		}
		if (transform.position.y < -50)
        {
            TakeDamage(health);
        }

	}
	void onTargetDeath()
	{
		hasTarget = false;
		currentState = State.Idle;

	}

	// lunging animation
	IEnumerator Attack()
	{

		currentState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 targetDir = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - targetDir * (myCollisionRadius);

		float attackSpeed = 3;
		float percent = 0;

		skin.color = Color.red;
		bool appliedDamage = false;

		while (percent <= 1)
		{
			// reduce health
			if (percent >= 0.5f && !appliedDamage)
			{
				appliedDamage = true;
				targetLife.TakeDamage(damage);
			}
			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
			// transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
			myRigidbody.MovePosition(Vector3.Lerp(originalPosition, attackPosition, interpolation));
			yield return null;
		}

		skin.color = originalColour;
		currentState = State.Chasing;
		// pathfinder.enabled = true;
	}

	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 dirHit)
	{
		AudioManager.instance.PlaySound("Impact", transform.position);
		if (damage >= health)
		{
			//get points
			ScoreManager.OnEnemyKilled(enemyScore);

			//drop loot
			if (getItem != null)
			{
				getItem.DropItem();
			}
			if (OnDeathStatic != null)
			{
				OnDeathStatic();
			}

			AudioManager.instance.PlaySound("Enemy Death", transform.position);
			Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, dirHit)), deathEffect.main.startLifetime.constant);
		}
		base.TakeHit(damage, hitPoint, dirHit);
	}

	IEnumerator UpdatePath()
	{
		float refreshRate = 0f;

		while (hasTarget)
		{
			if (currentState == State.Chasing)
			{
				Vector3 targetDir = (target.position - transform.position).normalized;
				targetDir.y = 0;
				Vector3 targetPosition = target.position - targetDir * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
				if (!dead)
				{
					float dist = Vector3.Distance(targetPosition, transform.position);
					// pathfinder.speed = speed;
					// pathfinder.SetDestination (targetPosition);
					// look at player
					transform.rotation = Quaternion.Slerp(transform.rotation,
										Quaternion.LookRotation(targetDir)
										, rotateSpeed * Time.deltaTime);
					// transform.LookAt(targetPosition);
					// move to player
					if (dist > 0.25)
					{
						// transform.position += transform.forward * speed * Time.deltaTime;	
						myRigidbody.MovePosition(myRigidbody.position + transform.forward * speed * Time.deltaTime);
					}
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}

	}
	Vector3 wayPoint;
	float Range = 10f;
	IEnumerator Wander()
	{
		float _ranNum = Random.Range(1, 10);
		while (hasTarget)
		{

			if (currentState == State.Wander)
			{
				// does nothing except pick a new destination to go to

				wayPoint = new Vector3(Random.Range(transform.position.x - Range, transform.position.x + Range), 1, Random.Range(transform.position.z - Range, transform.position.z + Range));
				wayPoint.y = 1;
				// don't need to change direction every frame seeing as you walk in a straight line only
				transform.LookAt(wayPoint);
				// move 


				// Debug.Log(wayPoint + " and " + (transform.position - wayPoint).magnitude);


			}

			yield return new WaitForSeconds(_ranNum);
		}
	}
}