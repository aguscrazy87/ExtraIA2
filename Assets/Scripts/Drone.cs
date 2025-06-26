using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Drone : MonoBehaviour
{
    public NeuralNetwork brain;

    public float speed = 5f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;
    private bool hasCrashed = false;
    private float aliveTime = 0f;

    private int numSensors = 6;
    private float sensorRange = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (brain == null) brain = new NeuralNetwork(7, 6, 4);
    }

    void FixedUpdate()
    {
        if (hasCrashed) return;

        aliveTime += Time.deltaTime;
        float[] inputs = GetSensorInputs();
        float[] outputs = brain.Forward(inputs);

        float yaw = (outputs[0] - 0.5f) * 2f;
        float forward = outputs[1];
        float lift = (outputs[2] - 0.5f) * 2f;

        Quaternion rotation = Quaternion.Euler(0f, yaw * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * rotation);

        Vector3 moveDir = transform.forward * forward * speed + Vector3.up * lift * speed;
        rb.linearVelocity = moveDir; 
    }

    float[] GetSensorInputs()
    {
        float[] sensors = new float[numSensors];

        Vector3[] directions = new Vector3[]
        {
        transform.forward,         //adelante
        -transform.forward,        //atrás
        transform.right,           //derecha
        -transform.right,          //izquierda
        transform.up,              //arriba
        -transform.up              //abajo
        };

        for (int i = 0; i < numSensors; i++)
        {
            Ray ray = new Ray(transform.position, directions[i]);
            if (Physics.Raycast(ray, out RaycastHit hit, sensorRange))
            {
                sensors[i] = 1f - (hit.distance / sensorRange);
                Debug.DrawRay(transform.position, directions[i] * hit.distance, Color.red);
            }
            else
            {
                sensors[i] = 0f;
                Debug.DrawRay(transform.position, directions[i] * sensorRange, Color.green);
            }
        }

        float[] inputs = new float[numSensors + 1];
        sensors.CopyTo(inputs, 0);
        inputs[inputs.Length - 1] = aliveTime / 10f;

        return inputs;
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasCrashed = true;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    public float Getfitness()
    {
        return aliveTime;
    }

    public bool HasCrashed() => hasCrashed;
}