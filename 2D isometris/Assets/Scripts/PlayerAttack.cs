using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private GameObject attcakArea;
    private bool attacking = false;
    private int comboStep = 0;
    private float comboTimer = 0f;
    private float maxComboTime = 1.5f;
    private bool canAttack = true;
    public float lungeDistance = 2f;
    
    private Vector2 lastDirection; 
    public static PlayerAttack singleton;

    private bool isComboWindow = false;

    public void Awake() {
        singleton = this;
    }

    private void Start() {
        animator = GetComponent<Animator>();
        attcakArea = transform.GetChild(0).gameObject;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return) && canAttack){
            Attack();
        }

        if(attacking){
            comboTimer += Time.deltaTime;
            if(comboTimer >= maxComboTime){
                ResetCombo();
            }
        }
    }

    void Attack(){
        if(!attacking){
            attacking = true;
            comboTimer = 0f;
            comboStep = 1;
        }
        else if(isComboWindow){
            comboStep++;
            if(comboStep > 3) {
                ResetCombo();
                return;
            }
            comboTimer = 0f;
        }

        PlayAttackAnimation();
        StartCoroutine(EnableComboWindow());
    }

    void PlayAttackAnimation(){
        switch(comboStep){
            case 1:
                animator.SetTrigger("Attack11");
                break;
            case 2:
                animator.SetTrigger("Attack22");
                break;
            case 3:
                animator.SetTrigger("Attack33");
                break;
        }

        attcakArea.SetActive(true);
        AlignAttackArea();
        StartCoroutine(DisableAttackAreaAfterDelay(0.25f));
    }

    // Fungsi ini akan dipanggil dari Animation Event di akhir setiap animasi attack
    public void OnAttackAnimationComplete()
    {
        if(!isComboWindow) // Jika tidak dalam window combo, reset
        {
            ResetCombo();
        }
        else 
        { 
            // Set idle animation berdasarkan arah
            SetIdleAnimation();
        }
    }

    // membuat supaya kembali ke animasi awal setelah attack
    void SetIdleAnimation()
    {
    animator.ResetTrigger("Attack11");
    animator.ResetTrigger("Attack22");
    animator.ResetTrigger("Attack33");

        if (lastDirection.y > 0) {
            animator.Play("idleUp");
        } 
        else if (lastDirection.y < 0) {
            animator.Play("idleDown");
        } 
        else if (lastDirection.x > 0) {
            animator.Play("idleRight");
        } 
        else if (lastDirection.x < 0) {
            animator.Play("idleLeft");
        }
    }

    IEnumerator EnableComboWindow(){
        isComboWindow = true;
        yield return new WaitForSeconds(0.7f);
        isComboWindow = false;
    }

    // kontrol arah attackArea
    void AlignAttackArea(){
        attcakArea.transform.position = transform.position; 
        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg + 180f; 
        attcakArea.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // menonaktifkan attackArea setelah attack
    IEnumerator DisableAttackAreaAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        attcakArea.SetActive(false);
    }

    // mengambil arah terakhir player
    public void SetLastDirection(Vector2 direction){
        lastDirection = direction;
    }

    void ResetCombo(){
        comboStep = 0;
        attacking = false;
        comboTimer = 0f;
        isComboWindow = false;
        canAttack = true;
        Debug.Log(comboStep);
        SetIdleAnimation();
    }
}