using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DroneManager : MonoBehaviour
{
    public GameObject dronePrefab;
    public int populationSize = 20;
    public float generationTime = 15f;

    private List<GameObject> drones = new List<GameObject>();
    private float timer = 0f;

    void Start()
    {
        SpawnInitialPopulation();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= generationTime)
        {
            timer = 0f;
            EvolvePopulation();
        }
    }

    void SpawnInitialPopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f));
            GameObject droneObj = Instantiate(dronePrefab, pos, Quaternion.identity);
            drones.Add(droneObj);
        }
    }

    void EvolvePopulation()
    {
        List<Drone> allDrones = drones.Select(d => d.GetComponent<Drone>()).ToList();

        List<Drone> sortedDrones = allDrones.OrderByDescending(d => d.Getfitness()).ToList();

        int survivors = populationSize / 2;
        List<NeuralNetwork> parentBrains = new List<NeuralNetwork>();

        for (int i = 0; i < survivors; i++)
        {
            parentBrains.Add(sortedDrones[i].brain);
        }

        foreach (GameObject drone in drones)
        {
            Destroy(drone);
        }

        drones.Clear();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork parentA = parentBrains[Random.Range(0, parentBrains.Count)];
            NeuralNetwork parentB = parentBrains[Random.Range(0, parentBrains.Count)];

            NeuralNetwork childBrain = NeuralNetwork.Crossover(parentA, parentB);
            childBrain.Mutate(0.1f);

            Vector3 pos = transform.position + new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f));
            GameObject newDrone = Instantiate(dronePrefab, pos, Quaternion.identity);
            newDrone.GetComponent<Drone>().brain = childBrain;

            drones.Add(newDrone);
        }
    }
}
