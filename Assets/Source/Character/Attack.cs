using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public float delay;
    [SerializeField] private Projectile projectile; 
    [SerializeField] private bool[] attackSpotsCanFire;
    [HideInInspector] public AttackSpot[] attackSpots;

    public void Commit()
    {
        for (int i = 0; i < attackSpots.Length; i++)
        {
            if (attackSpotsCanFire[i]) 
                attackSpots[i].Fire(projectile);
        }
    }
}
