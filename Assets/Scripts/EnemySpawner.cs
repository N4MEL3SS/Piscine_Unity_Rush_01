using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] AvailableEnemies;
    public float SpawnTime;
    
    private GameObject _currentEnemy;
    private NavMeshAgent _agent;
    private float _spawnTime;

    private PlayerMovement _player;
    // Start is called before the first frame update
    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();
        SpawnEnemy();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_currentEnemy == null)
        {
            if (_spawnTime >= SpawnTime)
            {
                SpawnEnemy();
                _spawnTime = 0;
            }
            _spawnTime += Time.deltaTime;
        }
        else
        {
            if (!_currentEnemy.GetComponent<EnemyLogic>().IsAlive)
                return;
            if (_agent.baseOffset >= 0.02f)
            {
                _agent.isStopped = false;
            }
            else
            {
                _agent.baseOffset += 0.0004f;
            }
        }   
    }
   
    private void SpawnEnemy()
    {
        _currentEnemy = Instantiate(AvailableEnemies[Random.Range(0, AvailableEnemies.Length)], transform.position, Quaternion.identity);
        var enemy = _currentEnemy.GetComponent<EnemyLogic>();
        enemy.SetLevel(_player.Level);
        _agent = _currentEnemy.GetComponent<NavMeshAgent>();
        _agent.baseOffset = -0.1f;
        _agent.isStopped = true;
    }

}
