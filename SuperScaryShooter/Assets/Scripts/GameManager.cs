using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool StartSpawning = true;
    public TextMeshProUGUI RoundTxt;
    [Tooltip("What round/day you are on, the higher it is the stronger and more enemies there are")]
    public int DayCount;
    [Tooltip("The variable that keeps track of the enemies that died in the day/round")]
    public int enemiesDead;
    [Tooltip("The variable that keeps track of the enemies that are alive in the day/round")]
    public int enemiesAlive;
    [Tooltip("The variable that keeps track of how many enemies you have killed")]
    public int KillCount;
    public int levelIndex = -1;

    [Header("Wave Spawning")]
    [Tooltip("The parent to help organize all the enemies")]
    public Transform EnemiesParent;
    [Tooltip("Who we chasing")]
    public Transform Player;
    [Tooltip("The layermask for the spawn points to keep them close to you")]
    public LayerMask whatIsSpawnpoints;
    [Tooltip("The radius for the spawning sphere that checks if the enemies can spawn around you")]
    public float PlayerSpawnRadius = 100f;
    [Tooltip("The rate that the enemies will spawn at")]
    public float spawnRate = 1f;
    [Tooltip("The inital delay between the enemies spawning when a new day begins")]
    public float spawnRateDelay = 2f;
    [Tooltip("The amount of rest you have when you have defeated all the enemies and advanced to the nearest day")]
    public float restTime = 5f;
    [Tooltip("The total amount of enemies that should spawn in during the wave")]
    public int enemyCount;
    [Tooltip("Enemies increase with each passing day, this is the amount of enemies that will increase")]
    public int enemyIncreaseInterval = 1;
    [Tooltip("A bool to check if we have stopped spawning the enemies")]
    public bool waveSpawningDone = true;
    //public GameObject enemy;

    [Header("Spawn Points")]
    public GameObject[] spawnPoints;
    public Collider[] enemySpawns;

    [Header("All Enemy Types")]
    public GameObject[] availableEnemies;

    [Header("Enemies Active")]
    public GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        //RoundChange();
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemies");
        if (enemiesAlive <= enemiesDead && waveSpawningDone == true && StartSpawning)
        {
            RoundChange();
        }
        enemySpawns = Physics.OverlapSphere(Player.position, PlayerSpawnRadius, whatIsSpawnpoints);
    }
    public void RoundChange()
    {
        enemiesDead = 0;
        DayCount++;
        RoundTxt.text = "Day " + DayCount.ToString();
        StartCoroutine(WaveSpawn());
        if(IsDivisible(DayCount, 7))
        {
            var Weaponsystem = GameObject.FindAnyObjectByType<WeaponSystem>();
            var randomNumber = Random.Range(0, Weaponsystem.availableWeapons.Count);
            var weaponToAdd = Weaponsystem.availableWeapons[randomNumber];
            Weaponsystem.equippedWeapons.Add(weaponToAdd);
        }

    }
    IEnumerator WaveSpawn()
    {
        //-1 is spawning based off of a random spawn point
        if (levelIndex == -1)
        {
            enemiesAlive = enemyCount;
            waveSpawningDone = false;
            Debug.Log("SpawningEnemies");
            for (int i = 0; i < enemiesAlive; i++)
            {
                int randomEnemy = Random.Range(0, availableEnemies.Length);
                int randomSpawn = Random.Range(0, spawnPoints.Length);
                GameObject SpawnedEnemy = Instantiate(availableEnemies[randomEnemy], spawnPoints[randomSpawn].transform);
                SpawnedEnemy.transform.parent = EnemiesParent;
                yield return new WaitForSeconds(spawnRate);
            }

            //spawnRate -= 0.1f;
            enemyCount += 1;
            Debug.Log("DoneSpawing");
            yield return new WaitForSeconds(spawnRateDelay);
            waveSpawningDone = true;
        }
        //0 MUST BE EMPTY, THAT IS THE HUB WORLD
        if (levelIndex == 1)
        {
            //maybe do a physics sphere cast and see how many spawnpoints are nearby then do the spawning from those points
            //random number out of the possible spawn points that are in the area, then spawn the enemy at that spawn point
            enemiesAlive = enemyCount;
            waveSpawningDone = false;
            Debug.Log("SpawningEnemies");
            for (int i = 0; i < enemiesAlive; i++)
            {
                int randomEnemy = Random.Range(0, availableEnemies.Length);
                int randomSpawn = Random.Range(0, enemySpawns.Length);
                GameObject SpawnedEnemy;
                if (enemySpawns == null)
                {
                    SpawnedEnemy = Instantiate(availableEnemies[randomEnemy], spawnPoints[0].transform);
                }
                else
                {
                    SpawnedEnemy = Instantiate(availableEnemies[randomEnemy], enemySpawns[randomSpawn].transform);
                }
                SpawnedEnemy.transform.parent = EnemiesParent;
                yield return new WaitForSeconds(spawnRate);
            }

            //spawnRate -= 0.1f;
            enemyCount += enemyIncreaseInterval;
            Debug.Log("DoneSpawing");
            yield return new WaitForSeconds(spawnRateDelay);
            waveSpawningDone = true;
        }
        yield return null;
    }
    public bool IsDivisible(int x, int n)
    {
        return (x % n) == 0;
    }
}
