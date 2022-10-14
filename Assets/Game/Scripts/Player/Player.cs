using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [SerializeField] private Enemy _target;
    [SerializeField] private Arrow _arrowPref;
    [SerializeField] private Transform _arrowTransform;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private float _energyAmount;
    [SerializeField] private float _powerAmount;
    [SerializeField] private float _energyConsumeRate;
    [SerializeField] private float _powerAgregateRate;
    [SerializeField] private float _energyCostPerShoot;

    [SerializeField] private Transform _chargeHand;

    

    private float _curentPower = 0f;
    private float _currentEnergy;
    private Arrow _arrow;
    private Animator _anim;
    public bool isShooting;
    public bool isDead = false;

    private bool _readyToShoot = true;


    void Start()
    {
        Instance = this;
        _currentEnergy = _energyAmount;
        _anim = GetComponent<Animator>();
        Invoke("GetArrow", 0.2f);
    }

    void Update()
    {
        if(GameController.Instance.currentState == GameController.GameState.actionPhase && !isDead)
        {
            if (_target == null)
            {
                FindTarget();
            }
            if (Input.GetMouseButton(0))
            {
                ChargeShot();
            }
            if (Input.GetMouseButtonUp(0))
            {
                CalculateSuccessShot();
                _readyToShoot = true;
            }
            if (!isShooting)
            {
                RestoreEnergy();
            }
        }

        if (isDead)
        {
            Death();
        }
    }
    
    private void GetArrow()
    {
        _arrow = Instantiate(_arrowPref, _arrowTransform);
        Quaternion worldRotation = transform.rotation * _arrow.transform.localRotation;
        _arrow.transform.rotation = worldRotation;
    }

    private void Shoot(bool isHit)
    {
        if (_readyToShoot)
        {
            _readyToShoot = false;
            var aimPoint = GameController.Instance.GetHitBox(_target, isHit);
            isShooting = false;
            _currentEnergy -= _energyCostPerShoot;
            _curentPower = 0f;
            _arrow.transform.parent = null;
            _anim.SetBool("IsShooting", false);
            _arrow.ShootTarget(aimPoint);
            _arrow = null;
            Invoke("GetArrow", 0.1f);
        }
    }
   
    public void FindTarget()
    {
        float distance = 100f;
        var enemies = GameController.Instance._enemies;
        if(enemies != null)
        {
            foreach (var target in GameController.Instance._enemies)
            {
                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget < distance)
                {
                    distance = distanceToTarget;
                    _target = target;
                }
            }
            TurnToTarget();
        }
    }

    private void TurnToTarget()
    {
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime);
    }

    private void ChargeShot()
    {
        if (_readyToShoot)
        {
            isShooting = true;
            _anim.SetBool("IsShooting", true);
            _arrow.transform.parent = _chargeHand.transform.parent;
            _currentEnergy = Mathf.Max(0, _currentEnergy - _energyConsumeRate * Time.deltaTime);
            _curentPower = Mathf.Min(_powerAmount, _curentPower + _powerAgregateRate * Time.deltaTime);
            if (_currentEnergy == 0 || _curentPower == _powerAmount)
            {
                CalculateSuccessShot();
            }
        }
        
    }
    private void RestoreEnergy()
    {
        if(_currentEnergy < _energyAmount)
        {
            _currentEnergy =  Mathf.Min(100, _currentEnergy + 10f * Time.deltaTime);
           
        }
    }
    public float GetEnergyBarFill()
    {
        return _currentEnergy / _energyAmount;
    }
    public float GetPowerBarFill()
    {
        return _curentPower / _powerAmount;
    }

    private void CalculateSuccessShot()
    {
        int chance = Random.Range(0, 100);
        bool success = false;
        if (_curentPower < 33 || _currentEnergy < 30)
        {
            if (chance > 40) 
            {
                success = false;
            }
        }
        if (_curentPower < 66)
        {
            if (chance > 70)
            {
                success = false;
                _arrow._damage = (int)(_arrow._damage * 1.5f);
            }
        }
        else
        {
            success = true;
            _arrow._damage = (int)(_arrow._damage * 2);
        }
        Shoot(success);
    }

    public void Death()
    {
        _anim.SetBool("IsDead", true);
        GameController.Instance.NextGameState();
    }
    
}
