using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEditor.PlayerSettings;

public class CreatureActions : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform centerPoint;
    [SerializeField] public Vector3 walkPoint;

    //private Vector3 distance;
    [Header("Control Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float changeRate;
    [SerializeField] private float rotationSpeed;
    private float minPos = -10f, maxPos = 10f;
    public float walkPointRadius;
    private bool changingPoint = false;
    

    [Header("References")]
    [SerializeField] private CreatureStatus creatureStatus;
    [SerializeField] private HerbivoreSenses herbivoreSenses;

    

    private void Awake()
    {
        herbivoreSenses = GetComponent<HerbivoreSenses>();
        creatureStatus = GetComponent<CreatureStatus>();

    }
    void Start()
    {
        

        walkPoint = transform.position + new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos), 0);

        //distance = transform.position;


        StartCoroutine(creatureStatus.HungerRoutine());
        

    }

    void Update()
    {
        
        if (!creatureStatus.isEating)
        {
            if (creatureStatus.foodLevel < creatureStatus.hungerLimit)
            {
                bool foundFood = herbivoreSenses.LookForFood();

                if (!foundFood)
                {
                    Move(walkPoint);
                }
            }
            else
            {
                Move(walkPoint);
            }

            if (Vector3.Distance(transform.position, walkPoint) < 1 && !changingPoint)
            {
              StartCoroutine(ChangePoint());

            }
        }
    }

    public void Move( Vector3 targetPosition )
    {
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

        
        Vector3 direction = targetPosition - transform.position;
        direction.z = 0; 
        herbivoreSenses.lookDirection = direction;

        if (direction.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public IEnumerator ChangePoint()
    {
        changingPoint = true;

        yield return new WaitForSeconds(changeRate);

        walkPoint = transform.position + new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos), 0);

        changingPoint = false;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(walkPoint, walkPointRadius);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("FoodBush") && creatureStatus.foodLevel < 20 && creatureStatus.foodLevel < 10)
        {
            Debug.Log("entrou");
            StopCoroutine(ChangePoint());
            creatureStatus.StopHungerRoutine();
            StartCoroutine(creatureStatus.Eat(OnFinishedEating));

        }
    }
    private void OnFinishedEating()
    {
        Debug.Log("Terminou de comer");
        StartCoroutine(ChangePoint());
        creatureStatus.StartHungerRoutine();
    }

}
