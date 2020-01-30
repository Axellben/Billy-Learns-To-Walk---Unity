using System;
public class NeuronalNetwork {

    public int inputSize, hiddenSize, outputSize;
    public Matrix W1, W2, b;
    public float lr;

    public NeuronalNetwork (int inputSize, int hiddenSize, int outputSize) {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;

        this.lr = 0.01f;
        this.W1 = new Matrix (this.hiddenSize, this.inputSize);
        this.W2 = new Matrix (this.outputSize, this.hiddenSize);
        this.b = new Matrix (1, 2);

        this.W1.Randomize ();
        this.W2.Randomize ();
        this.b.Randomize ();

    }

    public void Train (Matrix X, Matrix y, int epochs) {
        for (int i = 0; i < epochs; ++i) {
            Matrix z1 = Matrix.dot (this.W1, X) + this.b.data[0, 0];
            Matrix a1 = Matrix.Tanh (z1);
            Matrix z2 = Matrix.dot (this.W2, a1) + this.b.data[0, 1];
            Matrix a2 = Matrix.Tanh (z2);

            Matrix error_O = y - a2;

            if (i % 10000 == 0) {
                float error = Matrix.Sum (error_O);
                Console.WriteLine (Math.Pow (error, 2) / 2);
            }

            Matrix delta_O = error_O * Matrix.Tanh (a2, true);

            Matrix error_H = Matrix.dot (this.W2.T (), error_O);
            Matrix delta_H = error_H * Matrix.Tanh (a1, true);

            this.W2 += Matrix.dot (delta_O, a1.T ()) * this.lr;
            this.W1 += Matrix.dot (delta_H, X.T ()) * this.lr;

            this.b.data[0, 0] += this.lr * Matrix.Sum (delta_H);
            this.b.data[0, 1] += this.lr * Matrix.Sum (delta_O);
        }
    }

    public Matrix FeedForward (Matrix X) {
        Matrix z1 = Matrix.dot (this.W1, X) + this.b.data[0, 0];
        Matrix a1 = Matrix.Tanh (z1);
        Matrix z2 = Matrix.dot (this.W2, a1) + this.b.data[0, 1];
        Matrix a2 = Matrix.Tanh (z2);
        return a2;
    }

    public void Mutate (float rate) {
        this.W1.Mutate (rate);
        this.W2.Mutate (rate);
        this.b.Mutate (rate);
    }

    public void Copy (NeuronalNetwork b) {
        this.W1.Copy (b.W1);
        this.W2.Copy (b.W2);
        this.b.Copy (b.b);
    }

}