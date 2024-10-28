using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 7;  

    public interface IDamageable {
        void TakeDamage(int damage);
    }   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var damageable = other.GetComponent<IDamageable>();

            if(damageable != null){
                damageable.TakeDamage(damage);
            }
        }
    }
}
