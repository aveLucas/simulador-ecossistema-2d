using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateCreatures : MonoBehaviour
{
    [SerializeField] private GameObject[] creatures;
    private GameObject creature;
    [SerializeField] private int quantityOfCreatures;
    [SerializeField] private List<Vector3> positions;


    private float minPos = -20f, maxPos = 20f;

    void Start()
    {
        SpawnCreatures();
    }

    // Update is called once per frame

    void Update()
    {
        
    }
    private void CreatePos()
    {
        for (int i = 0; i < quantityOfCreatures; i++)
        {
            positions.Add(new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos), 0));
        }
    }
    private void SpawnCreatures()
    {
        
        CreatePos();
        
        
        foreach (Vector3 position in positions)
        {
            creature = creatures[Random.Range(0, creatures.Length)];
            Instantiate(creature, position, Quaternion.identity);
        }

    }
}

// Randomizar que criatura vai spanar em cada posição
//