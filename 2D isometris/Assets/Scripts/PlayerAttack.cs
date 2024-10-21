using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private int attackCombo = 0;
    private bool canAttack = true;
    public float comboResetTime = 1f;
    public float lungeDistance = 2f;
    public int damage = 10;
    
    private Rigidbody2D rb;
    private Coroutine comboResetCoroutine;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return) && canAttack){
            PerformAttack();
        }
    }

    void PerformAttack(){
        canAttack = false;

        attackCombo++;

        if (comboResetCoroutine != null){
            StopCoroutine(comboResetCoroutine);
        }

        if(attackCombo == 1){
            // animator.SetTrigger("");
            DealDamage();
        } else if(attackCombo == 2){
            // animator.SetTrigger("");
            DealDamage();
        } else if (attackCombo == 3){
            // animator.SetTrigger("");
            DealDamage();
            Lunge();
            attackCombo = 0;
        }

        comboResetCoroutine = StartCoroutine(ResetComboTimer());
    }

    void DealDamage(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1.5f);

        if(hit.collider != null && hit.collider.GetComponent<Enemy>()){
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null){
                // enemy.TakeDamage(damage);
            }
        }
    }

    void Lunge(){
        Vector2 lungeDirection = transform.right;
        rb.MovePosition(rb.position + lungeDirection * lungeDistance);
    }

    IEnumerator ResetComboTimer(){
        yield return new WaitForSeconds(comboResetTime);

        attackCombo = 0;
        canAttack = true;
    }

}
