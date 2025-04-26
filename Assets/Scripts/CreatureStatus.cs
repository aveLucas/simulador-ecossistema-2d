using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CreatureStatus : MonoBehaviour
{
    [Header("Status")]
    public float maxHealth;
    public float currentHealth;

    public bool isAlive;

    public float maxFoodLevel;
    public float foodLevel;

    public float hungerLimit;

    public bool isEating;

    public bool satisfied;

    private bool isRegenerating = false;

    private Coroutine hungerRoutine;


    private void Awake()
    {
        currentHealth = maxHealth;
        foodLevel = maxFoodLevel;
        isAlive = true; 

    }

   

    private void Update()
    {
        if (currentHealth == 0)
        {
            isAlive = false;
        }
        if (foodLevel >= hungerLimit && !isRegenerating)
        {
            satisfied = true;
            StartCoroutine(RegenHealth());
        }


    }
   
    public IEnumerator HungerRoutine() 
    {
        while (foodLevel > 0 && !isEating)
        {
            foodLevel--;
            //Debug.Log($"Com fome {foodLevel}");
            yield return new WaitForSeconds(5f);
        }
        if (foodLevel < hungerLimit)
        {
            satisfied = false;
        }
        if (foodLevel == 0)
        {
            StartCoroutine(DieOfHunger());
        }
    }
    
    public IEnumerator Eat(System.Action onFinished)
    {
        StopCoroutine(DieOfHunger());
        isEating = true;
        while (foodLevel < maxFoodLevel)
        {
            foodLevel++;
            //Debug.Log($"Comendo {foodLevel}");
            yield return new WaitForSeconds(2f);
        }
        
        isEating = false;
        onFinished?.Invoke(); 
    }

    public IEnumerator RegenHealth()
    {
        isRegenerating = true;
        if (satisfied)
        {
            while (currentHealth < maxHealth)
            {
                currentHealth++;
                yield return new WaitForSeconds(2f);
            }
        }
        isRegenerating = false;
    }

    public IEnumerator DieOfHunger()
    {
        while (foodLevel == 0 && isAlive)
        {

            currentHealth--;
            yield return new WaitForSeconds(5f);
        }

    }

    public void StartHungerRoutine()
    {
        if (hungerRoutine != null)
            StopCoroutine(hungerRoutine);

        hungerRoutine = StartCoroutine(HungerRoutine());
    }

    public void StopHungerRoutine()
    {
        if (hungerRoutine != null)
            StopCoroutine(hungerRoutine);
    }
}
