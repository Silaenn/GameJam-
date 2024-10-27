using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStamina : MonoBehaviour
{
    
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 7f;  
    public float staminaRunCost = 20f;   
    public RectTransform staminaBarFill;       

    public Image BackgorundBar;
    public static PlayerStamina singleton;

    private void Awake() {
        singleton = this;
    } 

    private void Start()
    {
        currentStamina = maxStamina;  
        UpdateStaminaUI();  
    }

    
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
        UpdateStaminaUI();  
    }

    
    public void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
        UpdateStaminaUI();  
    }

    
    void UpdateStaminaUI()
    {
        if (staminaBarFill != null)
        {
            float widthPercentage = currentStamina / maxStamina;

              staminaBarFill.sizeDelta = new Vector2(widthPercentage * BackgorundBar.rectTransform.sizeDelta.x, staminaBarFill.sizeDelta.y);
        }
    }
}
