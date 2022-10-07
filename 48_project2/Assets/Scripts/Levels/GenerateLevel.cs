using UnityEngine;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour
{
	public Room firstRoom;
	public Room[] rooms;
	public int depth = 3;
	List<Room> allRooms = new List<Room>();
	// Start is called before the first frame update
	void Start()
	{
		firstRoom.GetInDoors()[0].gameObject.AddComponent<BoxCollider>();
		allRooms.Add(firstRoom);
		generate(depth, firstRoom);
	}

	public void generate(int depth, Room curRoom)
	{
		if (depth == 0){
			foreach (Door door in curRoom.GetOutDoors()){
				door.gameObject.AddComponent<BoxCollider>();
			}
			return;
		}
		foreach (Door door in curRoom.GetOutDoors())
		{
			List<Room> spawnableRoom = new List<Room>();
			foreach (Room r in rooms)
			{
				matchDoors(door, r, r.GetInDoors()[0]);
				if (!overlap(r, curRoom)) spawnableRoom.Add(r);
			}
			if (spawnableRoom.Count == 0)
			{
				Debug.Log("No rooms");
				// TODO close this door with a mesh?
				door.gameObject.AddComponent<BoxCollider>();
				continue;
			}
			Room newRoom = spawnableRoom[Random.Range(0, spawnableRoom.Count)];
			newRoom = Instantiate(newRoom, Vector3.zero, Quaternion.identity);
			matchDoors(door, newRoom, newRoom.GetInDoors()[0]);
			allRooms.Add(newRoom);

			generate(depth - 1, newRoom);
		}
	}

	void matchDoors(Door oldDoor, Room newRoom, Door newDoor)
	{
		var forwardVectorToMatch = -oldDoor.transform.forward;

		// var correctiveRotation = Quaternion.FromToRotation(newDoor.transform.forward, forwardVectorToMatch);
		// newRoom.transform.rotation *= correctiveRotation;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newDoor.transform.forward);
		newRoom.transform.RotateAround(newDoor.transform.position, Vector3.up, correctiveRotation);

		var correctiveTranslation = oldDoor.transform.position - newDoor.transform.position;
		Vector3 doorSize = Vector3.Scale(transform.localScale, newDoor.GetComponent<MeshFilter>().sharedMesh.bounds.size);
		Vector3 doorCorrection = new Vector3(-doorSize.x, 0, doorSize.z);
		doorCorrection = oldDoor.transform.rotation * doorCorrection;
		correctiveTranslation += doorCorrection;

		newRoom.transform.position += correctiveTranslation;
	}   
	bool overlap(Room newRoom, Room oldRoom)
    {
        foreach (Room room in allRooms)
        {
            if (room == oldRoom) continue;
            foreach (Transform child1 in room.transform)
            {
                if(!child1.gameObject.name.Contains("Wall"))continue;
                foreach (Transform child2 in newRoom.transform)
                {
                    if(!child2.gameObject.name.Contains("Wall"))continue;
                    if (child1.GetComponent<MeshRenderer>().bounds.Intersects(child2.GetComponent<MeshRenderer>().bounds))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}

}
