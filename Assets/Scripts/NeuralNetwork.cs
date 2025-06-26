using UnityEngine;

public class NeuralNetwork
{
    private float[,] w1, w2;
    private float[] b1, b2;

    public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
    {
        w1 = new float[hiddenSize, inputSize];
        b1 = new float[hiddenSize];
        w2 = new float[outputSize, hiddenSize];
        b2 = new float[outputSize];

        Randomize(w1);
        Randomize(w2);
        Randomize(b1);
        Randomize(b2);
    }

    private void Randomize(float[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
                matrix[i, j] = Random.Range(-1f, 1f);
    }

    private void Randomize(float[] array)
    {
        for (int i = 0; i < array.Length; i++)
            array[i] = Random.Range(-1f, 1f);
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    public float[] Forward(float[] input)
    {
        int hiddenSize = b1.Length;
        float[] hidden = new float[hiddenSize];

        for (int i = 0; i < hiddenSize; i++)
        {
            float sum = b1[i];
            for (int j = 0; j < input.Length; j++)
                sum += w1[i, j] * input[j];
            hidden[i] = Sigmoid(sum);
        }

        int outputSize = b2.Length;
        float[] output = new float[outputSize];

        for (int i = 0; i < outputSize; i++)
        {
            float sum = b2[i];
            for (int j = 0; j < hiddenSize; j++)
                sum += w2[i, j] * hidden[j];
            output[i] = Sigmoid(sum);
        }

        return output;
    }

    // Método para mutar pesos y biases con cierta tasa de mutación
    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < w1.GetLength(0); i++)
            for (int j = 0; j < w1.GetLength(1); j++)
                if (Random.value < mutationRate)
                    w1[i, j] += Random.Range(-0.1f, 0.1f);

        for (int i = 0; i < b1.Length; i++)
            if (Random.value < mutationRate)
                b1[i] += Random.Range(-0.1f, 0.1f);

        for (int i = 0; i < w2.GetLength(0); i++)
            for (int j = 0; j < w2.GetLength(1); j++)
                if (Random.value < mutationRate)
                    w2[i, j] += Random.Range(-0.1f, 0.1f);

        for (int i = 0; i < b2.Length; i++)
            if (Random.value < mutationRate)
                b2[i] += Random.Range(-0.1f, 0.1f);
    }

    public static NeuralNetwork Crossover(NeuralNetwork parentA, NeuralNetwork parentB)
    {
        int inputSize = parentA.w1.GetLength(1);
        int hiddenSize = parentA.w1.GetLength(0);
        int outputSize = parentA.w2.GetLength(0);

        NeuralNetwork child = new NeuralNetwork(inputSize, hiddenSize, outputSize);

        // Cruza w1
        for (int i = 0; i < hiddenSize; i++)
            for (int j = 0; j < inputSize; j++)
                child.w1[i, j] = (Random.value < 0.5f) ? parentA.w1[i, j] : parentB.w1[i, j];

        // Cruza b1
        for (int i = 0; i < hiddenSize; i++)
            child.b1[i] = (Random.value < 0.5f) ? parentA.b1[i] : parentB.b1[i];

        // Cruza w2
        for (int i = 0; i < outputSize; i++)
            for (int j = 0; j < hiddenSize; j++)
                child.w2[i, j] = (Random.value < 0.5f) ? parentA.w2[i, j] : parentB.w2[i, j];

        // Cruza b2
        for (int i = 0; i < outputSize; i++)
            child.b2[i] = (Random.value < 0.5f) ? parentA.b2[i] : parentB.b2[i];

        return child;
    }
}
