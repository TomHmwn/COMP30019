using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	public Level[] levels;
	public Enemy[] enemies;
	Alive player;
	Transform playerT;

	int remainingEnemiesToSpawn;
	int enemiesRemainingAlive;
	// Time between each spawns
	float nextSpawnTime;

	Level currentLevel;
	public static int currentLevelNo { get; private set; }

	bool isDisabled;

	//endless
	public bool isEndless;
	int startingEnemyCount;

	// how spread out they are spawn
	public float areaToSpreadX = 10;
	public float areaToSpreadY = 10;
	public float raycastDistance = 100f;


	public event System.Action<int> OnNewLevel;
	MapGenerator map;
	void Start()
	{
		// map = FindObjectOfType<MapGenerator> ();

		player = FindObjectOfType<Player>();
		playerT = player.transform;
		player.OnDeath += OnPlayerDeath;


		startingEnemyCount = 5;
		currentLevelNo = 0;

		NextLevel();
	}

	void Update()
	{
		if (!isDisabled)
		{
		
			if (remainingEnemiesToSpawn > 0 && Time.time > nextSpawnTime)
            {
                // Debug.Log(nextSpawnTime);
                nextSpawnTime = Time.time + currentLevel.timeBetweenSpawns;

                // randomly spawn 1-2 enemies
                for(int j=0;j<Random.Range(1,3);j++)
                    if(remainingEnemiesToSpawn==0)break;
                    remainingEnemiesToSpawn--;
                    for (int i = 0; i < 10; i++)
                    {
                        Vector3 randPosition = new Vector3(Random.Range(-areaToSpreadX, areaToSpreadX), 0, Random.Range(-areaToSpreadY, areaToSpreadX));
                        Vector3 pos = randPosition + transform.position;
                        RaycastHit hit;

                        if (Physics.Raycast(pos, Vector3.down, out hit, raycastDistance))
                        {
                            if(hit.transform.name.Contains("Floor")){
                                StartCoroutine(SpawnEnemy(hit.point));
                                break;
                            }
                        }
                    }
            }
		}
	}
	IEnumerator SpawnEnemy(Vector3 pos)
	{

		int randomIndex = Random.Range(0, enemies.Length);

		// Debug.DrawLine(pos,hit.point,Color.red);
		// Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

		// GameObject clone = Instantiate(objectToSpawn, hit.point, spawnRotation);
		float height = enemies[randomIndex].GetComponent<MeshFilter>().sharedMesh.bounds.size.y;
		Enemy spawnedEnemy = Instantiate(enemies[randomIndex], pos + new Vector3(0, height, 0), Quaternion.identity) as Enemy;
		spawnedEnemy.OnDeath += OnEnemyDeath;
		yield return null;
	}

	// void ResetPlayerPosition(){
	//     playerT.position = map.transform.position;
	// }
	void NextLevel()
	{
		if (currentLevelNo > 0)
		{
			AudioManager.instance.PlaySound("Level Complete", Vector3.zero);
		}
		if (!isEndless)
		{
			currentLevelNo++;
			print("level: " + currentLevelNo);
			if (currentLevelNo - 1 < levels.Length)
			{

				currentLevel = levels[currentLevelNo - 1];

				remainingEnemiesToSpawn = currentLevel.enemyCount;
				enemiesRemainingAlive = remainingEnemiesToSpawn;
				if (OnNewLevel != null)
				{
					OnNewLevel(currentLevelNo);
				}
			}
		}
		else if (isEndless)
		{
			currentLevelNo++;
			// print("Wave: " + currentLevelNo);

			if (!isDisabled)
			{
				Level wave = new Level(startingEnemyCount, 3);

				currentLevel = wave;

				remainingEnemiesToSpawn = startingEnemyCount;
				enemiesRemainingAlive = remainingEnemiesToSpawn;
				startingEnemyCount = Mathf.RoundToInt(startingEnemyCount * 1.5f);
				if (OnNewLevel != null)
				{
					OnNewLevel(currentLevelNo);
				}

			}

		}
		// ResetPlayerPosition(); 
	}

	void OnEnemyDeath()
	{
		// print("enemy died");
		enemiesRemainingAlive--;

		if (enemiesRemainingAlive == 0)
		{
			NextLevel();
		}
	}
	void OnPlayerDeath()
	{
		isDisabled = true;

	}
	[System.Serializable]
	public class Level
	{

		public Level(int enemyCount, float timeBetweenSpawns)
		{
			this.enemyCount = enemyCount;
			this.timeBetweenSpawns = timeBetweenSpawns;
		}
		public int enemyCount;
		public float timeBetweenSpawns;

	}

}
