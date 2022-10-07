using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{

	public enum FireMode { Auto, burst, Single }
	public FireMode fireMode;
	public Transform[] projectileSpawn;
	public Projectile projectile;

	// milliseconds between shots
	// higher > less shots
	public float fireRate = 100;
	//speed where projectile leaves gun
	public float muzzleVelocity = 35;
	public int burstCount;
	public int startingTotalProjectiles = 100;
	public int totalProjectiles;
	public int projectilesPerMagazine;
	public float reloadingTime = .3f;
	bool isReloading;

	[Header("Recoil")]
	public Vector2 recoilPosKickRange = new Vector2(0.05f, 0.2f);
	public Vector2 recoilAngleKickRange = new Vector2(3, 5);
	public float kickRecoilTimeMove = .1f;
	public float kickRecoilTimeRotate = .1f;

	float nextShotTime;


	[Header("Effects")]
	public Transform shell;
	public Transform shellEjection;
	MuzzleFlash muzzleFlash;
	Player player;
	Vector3 recoilVelocity;
	public AudioClip shootAudio;
	public AudioClip reloadAudio;

	float recoilAngle;
	float recoilRotationVelocity;

	// Singleshots cannot shoot until trigger is released
	bool triggerReleasedFromLastShot;
	int shotRemainingInBurst;
	int projectilesRemainingInMagazine;

	void Start()
	{
		muzzleFlash = GetComponent<MuzzleFlash>();

		shotRemainingInBurst = burstCount;
		projectilesRemainingInMagazine = projectilesPerMagazine;

		totalProjectiles = startingTotalProjectiles;
	}


	void LateUpdate()
	{
		//animating recoil
		if (this.transform.parent && this.transform.parent.gameObject.name.Contains("Hold Weapon"))
		{

			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilVelocity, kickRecoilTimeMove);
			recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotationVelocity, kickRecoilTimeRotate);
			transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

			if (!isReloading && projectilesRemainingInMagazine == 0)
			{
				Reload();
			}
		}
	}
	void Shoot()
	{

		if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMagazine > 0)
		{

			if (fireMode == FireMode.burst)
			{
				if (shotRemainingInBurst == 0)
				{
					return;
				}
				shotRemainingInBurst--;
			}
			else if (fireMode == FireMode.Single)
			{
				if (!triggerReleasedFromLastShot)
				{
					return;
				}
			}
			//shoot all projectiles
			for (int i = 0; i < projectileSpawn.Length; i++)
			{
				if (projectilesRemainingInMagazine == 0)
				{
					break;
				}
				projectilesRemainingInMagazine--;
				nextShotTime = Time.time + fireRate / 1000;
				Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
				newProjectile.SetSpeed(muzzleVelocity);
			}

			Instantiate(shell, shellEjection.position, shellEjection.rotation);
			//animating gun movement
			muzzleFlash.Activate();
			transform.localPosition -= Vector3.forward * Random.Range(recoilPosKickRange.x, recoilPosKickRange.y);
			recoilAngle += Random.Range(recoilAngleKickRange.x, recoilAngleKickRange.y);
			recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

			AudioManager.instance.PlaySound(shootAudio, transform.position);
		}
	}
	public void Aim(Vector3 point)
	{
		if (!isReloading)
		{
			transform.LookAt(point);
		}
	}

	public void Reload(){

		if( !isReloading && projectilesRemainingInMagazine != projectilesPerMagazine && totalProjectiles > 0){

			totalProjectiles -= (projectilesPerMagazine -  projectilesRemainingInMagazine);
			StartCoroutine (ReloadAnimate());

			AudioManager.instance.PlaySound(reloadAudio, transform.position);
		}
	}
	IEnumerator ReloadAnimate()
	{
		isReloading = true;
		yield return new WaitForSeconds(0.2f);

		float percent = 0;
		float reloadSpeed = 1 / reloadingTime;

		Vector3 initialRotation = transform.localEulerAngles;
		float endReloadAngle = 30;

		while (percent < 1)
		{
			percent += Time.deltaTime * reloadSpeed;

			float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
			float reloadAngle = Mathf.Lerp(0, endReloadAngle, interpolation);
			transform.localEulerAngles = initialRotation + Vector3.left * reloadAngle;
			yield return null;
		}

		isReloading = false;
		projectilesRemainingInMagazine = projectilesPerMagazine;
	}
	public void onTriggerHold()
	{
		Shoot();
		triggerReleasedFromLastShot = false;
	}
	public void onTriggerRelease()
	{
		triggerReleasedFromLastShot = true;
		// reset burst shots
		shotRemainingInBurst = burstCount;

	}
	public int projectilesRemaining()
	{
		return projectilesRemainingInMagazine;
	}
}
