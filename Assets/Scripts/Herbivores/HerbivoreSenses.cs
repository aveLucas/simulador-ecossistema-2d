using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HerbivoreSenses : MonoBehaviour
{

    public Vector3 lookDirection;
    public float smellRadius;
    public float visionDistance;
    public List<Vector3> _smellDirections;
    public List<Vector3> _visionDirections;

    public LayerMask foodLayer;

    CreatureActions creatureActions;
    CreatureStatus creatureStatus;

    void Awake()
    {
        creatureActions = GetComponent<CreatureActions>();
        creatureStatus = GetComponent<CreatureStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vision();
    }
    public void Vision()
    {
        Vector3 origin = transform.position;
        Vector3 forward = lookDirection.normalized;
        float angle = 45f;
        int rayCount = 6;
        List<Vector3> directions = new List<Vector3>();
        directions.Add(forward);
        _visionDirections = directions;

        for (int i = 0; i <= rayCount; i++)
        {

            float t = (float)i / rayCount;
            float currentAngle = Mathf.Lerp(-angle / 2, angle / 2, t);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Vector3 direction = rotation * forward;
            directions.Add(direction);

        }
        foreach (Vector3 dir in directions)
        {
            Debug.DrawRay(transform.position, dir * visionDistance, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, visionDistance, foodLayer);
            if (hit.collider != null)
            {
                Debug.Log($"achei comida {hit.collider.name}");
            }
        }
    }


    public bool LookForFood()
    {
        if (creatureStatus.isEating)
            return false;



        Vector3 origin = transform.position;
        int rayCount = 36;
        float angleStep = 360f / rayCount;

        List<Vector3> directions = new List<Vector3>();

        _smellDirections = directions;

        for (int i = 0; i <= rayCount; i++)
        {

            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * Vector3.up; // Vector3.up aponta pro topo do objeto
            directions.Add(direction);

            
        }
        

        Transform closestFood = null;
        float closestDistance = Mathf.Infinity;

        foreach (Vector3 direction in directions)
        {
            Debug.DrawRay(origin, direction * smellRadius);
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
            creatureActions.Move(closestFood.position);
            return true;
        }

        return false;
    }

   /* public bool LookForFood()
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
            creatureActions.Move(closestFood.position);
            return true;
        }

        return false;
    }
*/
    private void OnDrawGizmos()
    {
        /*Vector3 origin = transform.position;
        Vector3 forward = lookDirection.normalized;
        float angle = 45f;
        int rayCount = 6;


        // Raio central do cone de visão
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(origin, forward * visionDistance);

        for (int i = 0; i <= rayCount; i++)
        {
            float t = (float)i / rayCount;
            float currentAngle = Mathf.Lerp(-angle / 2, angle / 2, t);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Vector3 direction = rotation * forward;

            Gizmos.DrawRay(origin, direction * visionDistance);
        }
*/

        /*// Area do olfato
        if (_directions != null)
        {
            foreach (Vector3 direction in _directions)
            {
                Vector3 target = transform.position + direction.normalized * smellRadius;
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, direction.normalized * smellRadius);
                Gizmos.DrawSphere(target, 0.1f);
            }
        }*/
    }

}
