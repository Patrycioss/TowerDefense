using System;
using System.Collections.Generic;
using Event;
using GameInformation;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

[Singleton]
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _path;
    public GameObject path => _path;
    
    public readonly CustomEventT<GameObject> onEnemySpawned = new();

    public List<GameObject> enemies
    {
        get
        {
            List<GameObject> newEnemies = new();
            for (int i = 1; i < transform.childCount; i++) 
                newEnemies.Add(transform.GetChild(i).gameObject);

            return newEnemies;
        }
    }

    public bool spawning = false;

    private List<Wave> _waves = new();
    private int _waveIndex;

    private void Awake()
    {
        if (_path == null) 
            Debug.LogWarning("No path set for " + name);
    }

    
    private void Start()
    {
        _waves = GameManager.instance.waves;
        
        if (_waves.Count == 0) 
            Debug.LogError("No waves set for " + name);
    }

    /// <summary>
    /// Spawns the next wave
    /// </summary>
    /// <param name="pCallback"> callback function it will call on finish </param>
    /// <returns> the duration of the wave</returns>
    public float SpawnNextWave(Action pCallback)
    {
        if (_waveIndex >= _waves.Count) return 0;
        
        Wave wave = _waves[_waveIndex];
        _waveIndex++;

        
        spawning = true;

        void OnWaveFinished()
        {
            spawning = false;
            pCallback();
        }

        wave.StartWave(SpawnEnemy, OnWaveFinished);
        
        
        
        
        return wave.GetDurationSeconds();
    }

    private void SpawnEnemy(Enemy pEnemy)
    {
        GameObject newEnemy = Instantiate(pEnemy.transform.parent.gameObject, transform.position, Quaternion.identity);
        newEnemy.transform.parent = transform;
        enemies.Add(newEnemy);
        onEnemySpawned.Raise(newEnemy);
    }

    public void RemoveEnemy(GameObject pEnemyObject)
    {
        if (enemies.Contains(pEnemyObject)) 
            enemies.Remove(pEnemyObject);
    }
}
