using System;

public class Matrix {

    public float[, ] data;
    public int rows, cols;

    public Matrix (int rows, int cols) {
        this.rows = rows;
        this.cols = cols;
        this.data = new float[rows, cols];
        for (int i = 0; i < rows; ++i) {
            for (int j = 0; j < cols; ++j) {
                this.data[i, j] = 0;
            }
        }
    }

    public void Randomize () {
        Random rnd = new Random ((int) DateTime.Now.Ticks);
        for (int i = 0; i < rows; ++i) {
            for (int j = 0; j < cols; ++j) {
                this.data[i, j] = (float) rnd.NextDouble () * 2 - 1;
            }
        }

    }
    // Multiply a scalar to a data
    public static Matrix operator * (Matrix a, float b) {
        Matrix c = new Matrix (a.rows, a.cols);
        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < a.cols; ++j) {
                c.data[i, j] = a.data[i, j] * b;
            }
        }
        return c;
    }

    // Multiply a data to a data (Element-Wise)
    public static Matrix operator * (Matrix a, Matrix b) {
        if (a.rows != b.rows || a.cols != b.cols) {
            Console.WriteLine ("\n Multiply matrices ERROR");
            Environment.Exit (-1);
        }
        Matrix c = new Matrix (a.rows, b.cols);
        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < a.cols; ++j) {
                c.data[i, j] = a.data[i, j] * b.data[i, j];
            }
        }
        return c;
    }

    // Add a scalar to a data
    public static Matrix operator + (Matrix a, float b) {
        Matrix c = new Matrix (a.rows, a.cols);
        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < a.cols; ++j) {
                c.data[i, j] = a.data[i, j] + b;
            }
        }
        return c;
    }

    // Subtract two matrices
    public static Matrix operator - (Matrix a, Matrix b) {
        if (a.rows != b.rows && a.cols != b.cols) {
            Console.WriteLine ("\n Adding matrices ERROR");
            Environment.Exit (-1);
        }
        Matrix c = new Matrix (a.rows, a.cols);
        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < a.cols; ++j) {
                c.data[i, j] = a.data[i, j] - b.data[i, j];
            }
        }
        return c;
    }

    // Add two matrices
    public static Matrix operator + (Matrix a, Matrix b) {
        if (a.rows != b.rows && a.cols != b.cols) {
            Console.WriteLine ("\n Adding matrices ERROR");
            Environment.Exit (-1);
        }
        Matrix c = new Matrix (a.rows, a.cols);
        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < a.cols; ++j) {
                c.data[i, j] = a.data[i, j] + b.data[i, j];
            }
        }
        return c;
    }

    // Do the dot product between two matrices
    public static Matrix dot (Matrix a, Matrix b) {
        if (a.cols != b.rows) {
            Console.WriteLine ("\n Matrix dot ERROR");
            Environment.Exit (-1);
        }
        Matrix c = new Matrix (a.rows, b.cols);

        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < b.cols; ++j) {
                for (int k = 0; k < a.cols; ++k) {
                    c.data[i, j] += a.data[i, k] * b.data[k, j];
                }
            }
        }

        return c;
    }

    // Transpose a data
    public Matrix T () {
        Matrix c = new Matrix (this.cols, this.rows);

        for (int i = 0; i < this.rows; ++i) {
            for (int j = 0; j < this.cols; ++j) {
                c.data[j, i] = this.data[i, j];
            }
        }

        return c;
    }

    public static Matrix Tanh (Matrix a, bool deriv = false) {
        Matrix b = new Matrix (a.rows, a.cols);
        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < a.cols; ++j) {
                float x = a.data[i, j];
                if (deriv == false) {
                    b.data[i, j] = (float) ((2.0f / (1 + Math.Exp (-2 * x))) - 1);
                } else {
                    b.data[i, j] = (float) (1 - (x * x));
                }

            }
        }
        return b;
    }

    public static Matrix Sigmoid (Matrix a, bool deriv = false) {
        Matrix b = new Matrix (a.rows, a.cols);
        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < a.cols; ++j) {
                if (deriv) {
                    b.data[i, j] = a.data[i, j] * (1 - a.data[i, j]);
                } else {
                    b.data[i, j] = (float) (1 / (1 + Math.Exp (-a.data[i, j])));
                }

            }
        }
        return b;
    }

    public static float Sum (Matrix a) {
        float S = 0;
        for (int i = 0; i < a.rows; ++i) {
            for (int j = 0; j < a.cols; ++j) {
                S += a.data[i, j];
            }
        }
        return S;
    }

    public void PrintMatrix () {
        Console.WriteLine ("***********************");
        for (int i = 0; i < this.rows; ++i) {
            for (int j = 0; j < this.cols; ++j) {
                Console.Write (this.data[i, j] + " ");
            }
            Console.WriteLine ();
        }
        Console.WriteLine ("***********************");
    }

    public void Mutate (float mutationRate) {
        Random rnd = new Random ((int) DateTime.Now.Ticks);
        for (int i = 0; i < this.rows; i++) {
            for (int j = 0; j < this.cols; j++) {
                if (rnd.NextDouble () < mutationRate) {
                    this.data[i, j] += (float) Matrix.SampleGaussian (rnd, 0, 0.1f);
                }
            }
        }
    }

    public static double SampleGaussian (Random random, double mean, double stddev) {
        // The method requires sampling from a uniform random of (0,1]
        // but Random.NextDouble() returns a sample of [0,1).
        double x1 = 1 - random.NextDouble ();
        double x2 = 1 - random.NextDouble ();

        double y1 = Math.Sqrt (-2.0 * Math.Log (x1)) * Math.Cos (2.0 * Math.PI * x2);
        return y1 * stddev + mean;
    }

    public void Copy (Matrix b) {
        this.data = (float[, ]) b.data.Clone ();
    }

}