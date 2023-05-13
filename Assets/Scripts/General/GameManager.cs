using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{

    #region VARIABLES
    [Header("HUD")]
    public Animator HudWaveAnimator;
    public TMP_Text waveNumber;

    [Header("Enemies")]
    public string[] enemiesPrefab;
    public string boss;
    public Transform[] spawnPoints;

    [Header("Waves")]
    public float spawnDelay = 0.2f;
    public int waveEnemyNumberMultiplier = 2;
    public int bossWave = 5;

    private int currentWaveEnemies;
    [SerializeField]
    private int remainingWaveEnemies;

    public int currentWave = 0;
    private float spawnTimer = 0;

    public static GameManager instance;

    [Header ("SpawningZone")]
    public Transform spawningZoneBegining;
    public Transform spawningZoneEnding;
    public float height = 10;

    [Header("Score Management")]
    private int currentScore = 0;
    private bool thousand = false;
    public static Action OnThousand;
    #endregion

    #region EVENTS
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        GenerateNewWave();
    }

    void Update()
    {
        if(spawnTimer <= spawnDelay)
        {
            spawnTimer += Time.deltaTime;
        }

        if (currentWave % bossWave == 0 && currentWaveEnemies > 0 && spawnTimer > spawnDelay)
        {
            GenerateBoss();
        }
        else if (currentWaveEnemies > 0 && spawnTimer > spawnDelay)
        {
            GenerateEnemy(enemiesPrefab);
            spawnTimer = 0;
        }
        

        if(remainingWaveEnemies <= 0)
        {
            GenerateNewWave();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((spawningZoneBegining.position.x + spawningZoneEnding.position.x) / 2, 0, (spawningZoneBegining.position.z + spawningZoneEnding.position.z) / 2);
        Vector3 size = new Vector3(spawningZoneEnding.position.x - spawningZoneBegining.position.x, 1, spawningZoneEnding.position.z - spawningZoneBegining.position.z);
        Gizmos.DrawWireCube(center, size);
    }

    private void OnEnable()
    {
        //Nos suscribimos para conocer cuando muere un enemigo
        EnemyHealth.OnDead += EnemyDead;
        EnemyHealth.OnDead += () => AddToScore(20);
        EnemyHealth.OnBossDead += OnBossDead;
        EnemyHealth.OnBossDead += () => AddToScore(100);
        EvolvingAmmunition.OnRecollected += (string name) => AddToScore(10);
        Destroyable.OnDestroyObject += () => AddToScore(10);
    }

    private void OnDestroy()
    {
        EnemyHealth.OnDead -= EnemyDead;
    }
    #endregion

    #region METHODS

    /// <summary>
    /// Genera un enemigo aleatorio a partir de la lista recibida
    /// </summary>
    /// <param name="enemyPool"></param>
    public void GenerateEnemy(string [] enemyPool)
    {
        if (enemyPool.Length == 0) return;

        int randomEnemy= UnityEngine.Random.Range(0, enemyPool.Length);
        Vector3 newSpawnPoint = GenerateRandomSpawnPoint();
        PoolEntity portal = PoolsManager.instance.PullElement("EnemyPortal",
            newSpawnPoint,
            Quaternion.identity);
        StartCoroutine(CreateEnemyAfterDelay(portal, enemyPool[randomEnemy], newSpawnPoint));
        currentWaveEnemies--;
    }

    /// <summary>
    /// Genera un enemigo aleatorio a partir de la lista recibida
    /// </summary>
    /// <param name="enemyPool"></param>
    public void GenerateBoss()
    {
        Vector3 newSpawnPoint = GenerateRandomSpawnPoint();
        PoolEntity portal = PoolsManager.instance.PullElement("EnemyPortal",
            newSpawnPoint,
            Quaternion.identity);
        StartCoroutine(CreateEnemyAfterDelay(portal, boss, newSpawnPoint));
        currentWaveEnemies = 0;
    }

    /// <summary>
    /// Para crear al enemigo después de un delay
    /// </summary>
    /// <param name="EnemyPosition"></param>
    /// <returns></returns>
    IEnumerator CreateEnemyAfterDelay(PoolEntity portal, string enemyName, Vector3 EnemyPosition)
    {
        yield return new WaitForSeconds(2);
        PoolsManager.instance.PullElement(enemyName,
               EnemyPosition,
               Quaternion.identity);
        yield return new WaitForSeconds(1);
        portal.ReturnToPool();
    }

    /// <summary>
    /// Mtodo a ejecutar cuando el enemigo muera
    /// </summary>
    public void EnemyDead()
    {
        remainingWaveEnemies--;
    }

    /// <summary>
    /// Cuando el boss es derrotado
    /// </summary>
    public void OnBossDead()
    {
        remainingWaveEnemies = 0;
    }

    /// <summary>
    /// Gestiones necesarias para generar una nueva oleada
    /// </summary>
    public void GenerateNewWave()
    {
        currentWave++;
        waveNumber.text = currentWave.ToString();
        HudWaveAnimator.SetTrigger("Show");

        currentWaveEnemies = currentWave * waveEnemyNumberMultiplier;

        remainingWaveEnemies = currentWaveEnemies;
    }

    /// <summary>
    /// Genera un punto aleatorio dentro de la zona de spawn
    /// </summary>
    private Vector3 GenerateRandomSpawnPoint()
    {
        float posX = UnityEngine.Random.Range(spawningZoneBegining.position.x, spawningZoneEnding.position.x);
        float posZ = UnityEngine.Random.Range(spawningZoneBegining.position.z, spawningZoneEnding.position.z);

        return new Vector3(posX, 0, posZ);
    }

    /// <summary>
    /// Añade puntos a la puntacion actual
    /// </summary>
    /// <param name="score"></param>
    public void AddToScore(int score)
    {
        currentScore += score;
        GUIManager.instance.RenderScore(currentScore);
        if(currentScore >= 1000 && !thousand)
        {
            thousand = true;
            OnThousand?.Invoke();
        }
    }

    /// <summary>
    /// retrona la puntuacion de la ronda actual
    /// </summary>
    /// <returns></returns>
    public int GetcurrentScore()
    {
        return currentScore;
    }

    #endregion
}
