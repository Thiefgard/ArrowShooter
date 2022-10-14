using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] Enemy _enemyPref;
    [SerializeField] Enemy _enemy;

    

    void Update()
    {
        if(_enemy == null)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemy = Instantiate(_enemyPref);
        enemy.transform.position = transform.position;
        _enemy = enemy;
        GameController.Instance.AddEnemy(enemy);
    }
}
