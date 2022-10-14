using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        WaitPhase,
        actionPhase,
        EndGame
    }
    public static GameController Instance;
    public GameState currentState;
    public List<Enemy> _enemies;
    public bool isAction;

    void Start()
    {
        currentState = GameState.WaitPhase;
        Instance = this;
        isAction = false;
        UIController.Instance.ShowStateUI(currentState);
    }

    void Update()
    {
        if(currentState == GameState.WaitPhase && !isAction)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isAction = true;
                Invoke("NextGameState", 0.5f);
            }
        }
       
    }
    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        Player.Instance.FindTarget();
    }
    public void NextGameState()
    {
        currentState++;
        UIController.Instance.ShowStateUI(currentState);
    }
    public Transform GetHitBox(IDamagable target , bool isHit)
    {
        return target.FindPlaceToHit(isHit);
    }

}
