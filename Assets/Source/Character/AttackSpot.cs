using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpot : MonoBehaviour
{
    public void Fire(Projectile projectile)
    {
        Projectile proj = Instantiate(projectile, transform.position, transform.rotation, transform);
    }
}
