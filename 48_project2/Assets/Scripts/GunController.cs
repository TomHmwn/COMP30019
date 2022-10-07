using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{

	public Transform weaponHoldster;
	public Gun[] guns;
	Gun equippedGun;

	void Awake()
	{
		if (equippedGun == null)
		{
			// Equip handgun at the beginning
			EquipGun(guns[0]);
		}
	}
	public void EquipGun(Gun gunToEquip)
	{
		if (equippedGun != null)
		{
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate(gunToEquip, weaponHoldster.position, weaponHoldster.rotation) as Gun;
		equippedGun.transform.parent = weaponHoldster;
	}
	public void EquipGun(int weaponIndex)
	{
		EquipGun(guns[weaponIndex]);
	}
	public void onTriggerHold()
	{
		if (equippedGun != null)
		{
			equippedGun.onTriggerHold();
		}
	}
	public void onTriggerRelease()
	{
		if (equippedGun != null)
		{
			equippedGun.onTriggerRelease();
		}
	}
	public void Aim(Vector3 point)
	{
		if (equippedGun != null)
		{
			equippedGun.Aim(point);
		}
	}
	public float getGunHeight
	{
		get
		{
			return weaponHoldster.position.y;
		}
	}
	public void Reload()
	{
		if (equippedGun != null)
		{
			equippedGun.Reload();
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		print(other.gameObject.name);
		if (other.gameObject.name.Contains("Machinegun"))
		{
			EquipGun(guns[1]);
			Destroy(other.gameObject);
		}
		if (other.gameObject.name.Contains("Shotgun"))
		{
			EquipGun(guns[2]);
			Destroy(other.gameObject);
		}
		if (other.gameObject.name.Contains("Handgun"))
		{
			EquipGun(guns[0]);
			Destroy(other.gameObject);
		}
		if(other.gameObject.name.Contains("Ammo")){
			if(equippedGun != null){
				equippedGun.totalProjectiles = equippedGun.startingTotalProjectiles;
				Destroy(other.gameObject);
			}
		}
	}
	public Gun GetEquippedGun()
	{
		return equippedGun;
	}
}