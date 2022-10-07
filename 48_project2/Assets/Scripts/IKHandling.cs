using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandling : MonoBehaviour
{
	Animator animator;

	public float leftHandWeight = 1;
	Transform leftHandTarget;

	public float RightHandWeight = 1;
	Transform rightHandTarget;

	public Transform weaponHold;
	Gun weapon;
	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		//ref to animator
		if (weaponHold != null)
		{

			weapon = weaponHold.GetComponentInChildren<Gun>();


			foreach (var child in weapon.GetComponentsInChildren<Transform>())
			{

				if (child.gameObject.name == "left_hand_target")
				{
					leftHandTarget = child;
				}
				if (child.gameObject.name == "right_hand_target")
				{
					rightHandTarget = child;
				}
			}
		}

	}

	// Update is called once per frame
	void Update()
	{
		weapon = weaponHold.GetComponentInChildren<Gun>();
		if (weapon != null)
		{

			foreach (var child in weapon.GetComponentsInChildren<Transform>())
			{

				if (child.gameObject.name == "left_hand_target")
				{
					leftHandTarget = child;
				}
				if (child.gameObject.name == "right_hand_target")
				{
					rightHandTarget = child;
				}
			}
		}
	}
	void OnAnimatorIK()
	{
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
		animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
		animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
		animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);

		animator.SetIKPositionWeight(AvatarIKGoal.RightHand, RightHandWeight);
		animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
		animator.SetIKRotationWeight(AvatarIKGoal.RightHand, RightHandWeight);
		animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);

	}
	private T GetChildComponentByName<T>(string name) where T : Component
	{
		foreach (T component in GetComponentsInChildren<T>(true))
		{
			if (component.gameObject.name == name)
			{
				return component;
			}
		}
		return null;
	}
}
