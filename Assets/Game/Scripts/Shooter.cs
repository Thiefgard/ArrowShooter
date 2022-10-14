using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform Target;
    public float FlightTime = 0.7f;

    public Rigidbody Projectile;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            LaunchCanopyProjectile(Projectile, transform.position, Target.position, FlightTime);
        }

    }



    public void LaunchCanopyProjectile(Rigidbody projectile, Vector3 start, Vector3 end, float timeCorrection = 0.7f)
    {
        float time = GetTime(start, end, timeCorrection);
        Vector3 v = CalculateVelocity(start, end, time);
        transform.rotation = Quaternion.LookRotation(v);
        Rigidbody obj = Instantiate(projectile, transform.position, Quaternion.identity);
        obj.velocity = v;
    }

    private float GetTime(Vector3 start, Vector3 end, float timeCorrection = 0.7f)
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
