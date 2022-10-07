using UnityEngine;

public class Room : MonoBehaviour
{
	public string[] Tags;


	public Door[] GetDoors()
	{
		return GetComponentsInChildren<Door>();
	}
	public InDoor[] GetInDoors()
	{
		return GetComponentsInChildren<InDoor>();
	}
	public OutDoor[] GetOutDoors()
	{
		return GetComponentsInChildren<OutDoor>();
	}

	public Bounds GetBounds()
	{
		Bounds ans = new Bounds();
		foreach (Transform child in transform)
		{
			ans.Encapsulate(child.GetComponent<MeshRenderer>().bounds);
		}
		return ans;
	}
}