using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using Unity.Burst.CompilerServices;
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
    private float minPos = -10f, maxPos = 10f;
    public float radius;
    [SerializeField] private float changeRate;
    [SerializeField] private float smellRadius;
    [SerializeField] private Vector3[] _directions;
    private bool changingPoint = false;

    [Header("References")]
    [SerializeField] private CreatureStatus creatureStatus;
    [SerializeField] private LayerMask foodLayer;

    void Start()
    {
        creatureStatus = GetComponent<CreatureStatus>();

        walkPoint = transform.position + new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos), 0);

        //distance = transform.position;


        StartCoroutine(creatureStatus.HungerRoutine());
        

    }

    void Update()
    {
        if (!creatureStatus.isEating)
        {
            if (creatureStatus.foodLevel < 10)
            {
                bool foundFood = LookForFood();

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

    private bool LookForFood()
    {
        if (creatureStatus.isEating)
            return false;



        Vector3[] directions = {
            Vector3.right,
            Vector3.left,
            Vector3.up,
            Vector3.down,
            new Vector3(1, 1, 0),   // cima-direita
            new Vector3(-1, 1, 0),  // cima-esquerda
            new Vector3(1, -1, 0),  // baixo-direita
            new Vector3(-1, -1, 0)  // baixo-esquerda
        };
        _directions = directions;

        Transform closestFood = null;
        float closestDistance = Mathf.Infinity;

        foreach (Vector3 direction in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, smellRadius, foodLayer);
            if (hit.collider != null)
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFood = hit.transform;
                }

                Debug.DrawLine(transform.position, hit.point, Color.yellow, 0.5f);
            }
        }

        if (closestFood != null)
        {
            Move(closestFood.position);
            return true;
        }

        return false;
    }
    

    public void Move( Vector3 position )
    {
        transform.position = Vector3.Lerp(transform.position, position, speed);
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
        Gizmos.DrawWireSphere(walkPoint, radius);


        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, smellRadius);  
        if(_directions != null)
        {
            foreach (Vector3 direction in _directions)
            {
                Vector3 target = transform.position + direction.normalized * smellRadius;
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, direction.normalized * smellRadius);
                Gizmos.DrawSphere(target, 0.1f);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay");
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
