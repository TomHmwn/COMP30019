using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static int score { get; private set; }
	static float lastEnemyKillTime;
	static int streakCount;
	static float streakExpiryTime = 1;
	void Start(){
		score = 0;
	}
	public static void OnEnemyKilled(int point)
	{
		if (Time.time < lastEnemyKillTime + streakExpiryTime)
		{
			streakCount++;
		}
		else
		{
			streakCount = 0;
		}
		lastEnemyKillTime = Time.time;

		score += point + (int)Mathf.Pow(2, streakCount);
	}

}
