using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player;
    private float transparencyLevel = 0.5f;
    private float detectionRadius = 0.7f;
    public LayerMask obstacleLayer;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isTransparent = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
      Collider2D hit = Physics2D.OverlapCircle(player.position, detectionRadius, obstacleLayer);

        if(hit != null && hit.transform == transform){
           SetTransparency(true);
        }
        else{
           SetTransparency(false);
        }
    }

    void SetTransparency(bool makeTransparent){
        if(makeTransparent && !isTransparent){
            Color newColor = originalColor;
            newColor.a = transparencyLevel;
            spriteRenderer.color = newColor;
            isTransparent = true;
        } else if(!makeTransparent && isTransparent){
            spriteRenderer.color = originalColor;
            isTransparent = false;
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, detectionRadius);
    }
}
