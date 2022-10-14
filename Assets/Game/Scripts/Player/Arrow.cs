using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool isShooting = false;
    [SerializeField] private float _flightTime;
    [SerializeField] private GameObject _hitParticle;

    private Enemy _target;

    public int _damage;
    private Rigidbody _rb;


    void FixedUpdate()
    {
        if(_rb != null)
        {
            if (_rb.velocity != Vector3.zero)
                _rb.rotation = Quaternion.LookRotation(_rb.velocity);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable enemy = collision.collider.GetComponent<IDamagable>();
        if (enemy != null )
        {
            var particle = Instantiate(_hitParticle);
            particle.transform.position = collision.collider.ClosestPoint(transform.position);
            Destroy(particle, 0.4f);
            Destroy(_rb);
            gameObject.transform.parent = collision.transform;
            enemy.GetHit(_damage);
        }
    }

    public void ShootTarget( Transform aimPoint)
    {
        LaunchCanopyProjectile(transform.position, aimPoint.position, _flightTime);
    }
    private void LaunchCanopyProjectile( Vector3 start, Vector3 end, float timeCorrection = 1f)
    {
        float time = GetTime(start, end, timeCorrection);
        Vector3 v = CalculateVelocity(start, end, time);
        transform.rotation = Quaternion.LookRotation(v);
        _rb = gameObject.AddComponent(typeof(Rigidbody)).GetComponent<Rigidbody>();
        _rb.angularDrag = 0f;
        _rb.mass = 10f;
        _rb.velocity = v;
    }
    private float GetTime(Vector3 start, Vector3 end, float timeCorrection = 1f)
    {
        return Vector3.Distance(start, end) * Time.fixedDeltaTime + timeCorrection;
    }
    private Vector3 CalculateVelocity(Vector3 start, Vector3 end, float time)
    {
        //define the distance x and y first
        Vector3 distance = end - start;
        Vector3 distance_x_z = distance;
        distance_x_z.Normalize();
        distance_x_z.y = 0;

        //creating a float that represents our distance 
        float sy = distance.y;
        float sxz = distance.magnitude;

        //calculating initial x velocity
        float Vxz = sxz / time;

        ////calculating initial y velocity
        float Vy = sy / time + 0.502f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distance_x_z * Vxz;
        result.y = Vy;

        return result;
    }

}
