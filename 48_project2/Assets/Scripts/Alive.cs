using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : MonoBehaviour, IDamagable
{
	public float startingHealth;
	public float health { get; protected set; }
	protected bool dead;

	public HealthHUD healthBar;
	// event to notify other classes that a object has died
	public event System.Action OnDeath;
	protected virtual void Start()
	{
		health = startingHealth;
		healthBar.SetMaxHealth(startingHealth);
	}
	public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 dirHit)
	{
		// apply raycast hit

		TakeDamage(damage);
	}
	public virtual void TakeDamage(float damage)
	{
		health -= damage;
		healthBar.SetHealth(health);
		if (health <= 0 && !dead)
		{
			Die();
		}
	}

	[ContextMenu("Self Destruct")]
	public virtual void Die()
	{
		dead = true;
		if (OnDeath != null)
		{
			OnDeath();
		}
		GameObject.Destroy(gameObject);
	}
}
