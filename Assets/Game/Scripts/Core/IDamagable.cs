
using UnityEngine;

public interface IDamagable 
{
    public Transform FindPlaceToHit(bool success);
    public void GetHit(int damage);

}
