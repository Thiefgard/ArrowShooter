using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform _target;
    [SerializeField] Transform _hitTransform;
    [SerializeField] Transform _missPointsTransform;
    [SerializeField] int _health;
    private NavMeshAgent _agent;
    private Animator _anim;

    [SerializeField] private float _timeBetweenWalkAround;
    private float _currentTime = 0;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(GameController.Instance.currentState == GameController.GameState.WaitPhase)
        {
            if(_currentTime >= _timeBetweenWalkAround)
            {
                WalkAround();
            }
        }
        if(GameController.Instance.currentState == GameController.GameState.actionPhase)
        {
            if(_target == null)
            {
                _target = Player.Instance.transform;
                MoveTo(_target.position);
            }
            if(Vector3.Distance(transform.position, _target.position) <= 2.5f)
            {
                Hit();
            }
        }
        if(GameController.Instance.currentState == GameController.GameState.EndGame)
        {
            _agent.isStopped = true;
        }

        if(_health == 0)
        {
            Die();
        }

        _currentTime += Time.deltaTime;
        UpdateMoveAnimation();
    }

    private void MoveTo(Vector3 destination)
    {
        _agent.destination = destination;
    }
    
    private void UpdateMoveAnimation()
    {
        Vector3 velocity = _agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        _anim.SetFloat("forwardSpeed", speed);
    }
    private void Die()
    {
        _agent.isStopped = true;
        _anim.SetBool("isDeath", true);
        GameController.Instance.RemoveEnemy(this);
        Destroy(GetComponent<BoxCollider>());
        Destroy(gameObject, 3f);
    }
    

    public void GetHit(int damage)
    {
        _health = Mathf.Max(0, _health - damage);
    }

    public Transform FindPlaceToHit(bool success)
    {
        if (success) return _hitTransform;
        else
        {
            List<Transform> missTargets = new List<Transform>();
            foreach (Transform p in _missPointsTransform)
            {
                if (p != _missPointsTransform)
                {
                    missTargets.Add(p);
                }
            }
            return missTargets[Random.Range(0, missTargets.Count)];
        }
    }

    private void Hit()
    {
        _agent.isStopped = true;
        _anim.SetBool("isAttack", true);
    }
    

    private void WalkAround()
    {
        LayerMask targetLayer = LayerMask.NameToLayer("Road");
        Ray ray = new Ray(transform.position, new Vector3(Random.Range(-3, 3), -1, Random.Range(-3, 3)));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if(hit.collider != null)
        {
            if (hit.collider.gameObject.layer == targetLayer)
            {
                MoveTo(hit.point);
                _currentTime = 0;
            }
        }
    }
   

    //anim func
    private void KillPlayer()
    {
        Player.Instance.isDead = true;
    }
}
