using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statics;

public class HealthPickup : MonoBehaviour
{
    [SerializeField, Range(0, 100)] int _recoverDamageAmout = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageShip damageShip = other.GetComponent<DamageShip>();

        if (damageShip != null)
        {
            damageShip.RecoverDamage(_recoverDamageAmout);
            Destroy(gameObject);
        }
            
    }

}
