using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CreatureStatus : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] private float life = 10f;
    private float _foodLevel = 20f;
    private bool _isEating;
    public float foodLevel;
    public bool isEating;

    private Coroutine hungerRoutine;

    [Header("References")]
    [SerializeField] private CreatureActions creatureActions;

    private void Awake()
    {
        creatureActions = GetComponent<CreatureActions>();

        isEating = _isEating;
        foodLevel = _foodLevel;
    }

    public IEnumerator HungerRoutine()
    {
        while (foodLevel > 0 && !isEating)
        {
            foodLevel--;
            Debug.Log($"Com fome {foodLevel}");
            yield return new WaitForSeconds(5f);
        }
    }
    
    /*public IEnumerator Eat()
    {
        isEating = true;
        while (foodLevel < 20)
        {

            //speed = 0f;
            foodLevel++;
            Debug.Log($"Comendo {foodLevel}");
            yield return new WaitForSeconds(2f);
        }
        if (foodLevel == 20)
        {
            //speed = 0.002;
            isEating = false;
            
        }

    }*/
    public IEnumerator Eat(System.Action onFinished)
    {
        isEating = true;
        while (foodLevel < 20)
        {
            foodLevel++;
            Debug.Log($"Comendo {foodLevel}");
            yield return new WaitForSeconds(2f);
        }

        isEating = false;
        onFinished?.Invoke(); 
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
