using System;
using System.Collections.Generic;
using System.Text;
using Matrix;
namespace Matrix
{
    class ANN
    {
        public int inputCount;
        public int outputCount;
        public int hLayerCount;
        public int[] neuronCount;

        public float[] bias;
        public float learningRate;

        public Matrix inputs;
        public Matrix targets;
        public Matrix outputs;


        public Matrix[] hiddenLayers;
        public Matrix[] weights;

        Random rnd; // for selecting a random tranning example in training set

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs">it contains row by row each training-set elements</param>
        /// <param name="targets">it contains row by row each actual value of inputs</param>
        /// <param name="outputCount">it specify neuron count of outpu layer</param>
        /// <param name="hLayerCount">it specify hidden layer count</param>
        /// <param name="hLayerNeuronCount">it specify neuron count for each hidden layer</param>
        public ANN(float[,] inputs, float[,] targets, int outputCount, int hLayerCount, int[] hLayerNeuronCount)
        {

            this.inputCount = inputs.GetLength(1);
            this.outputCount = outputCount;
            this.inputs = new Matrix(inputs);
            this.targets = new Matrix(targets);
            this.hLayerCount = hLayerCount;
            this.neuronCount = hLayerNeuronCount;
            init();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs">it contains row by row each training-set elements</param>
        /// <param name="targets">it contains row by row each actual value of inputs</param>
        /// <param name="outputCount">it specify neuron count of outpu layer</param>
        /// <param name="hLayerCount">it specify hidden layer count</param>
        /// <param name="hLayerNeuronCount">it specify neuron count for each hidden layer</param>
        public ANN(Matrix inputs, Matrix targets, int outputCount, int hLayerCount, int[] hLayerNeuronCount)
        {
            this.inputs = inputs;
            this.inputCount = inputs.column;
            this.targets = targets.GetTranspose();
            this.hLayerCount = hLayerCount;
            this.neuronCount = hLayerNeuronCount;
            this.outputCount = outputCount;

            init();
        }
        /// <summary>
        /// This method initialize the bias , hidden layers and weights.
        /// The variable weights is a multi-dimensional Matrix, that contain weight between layers. w[0] is between input layer and if exist hidden layer else outputlayer.
        /// The weights[i] is weight between hidden layer [i-1] and hidden layer [i], each column of weights[i] does specify weight between inputs and a hidden layer neuron .
        /// </summary>
        private void init()
        {
            rnd = new Random();
            this.bias = new float[this.hLayerCount+1];
            hiddenLayers = new Matrix[this.hLayerCount];
            for (int i = 0; i < this.hLayerCount; i++)
            {
                hiddenLayers[i] = new Matrix(1, this.neuronCount[i]);
            }
            weights = new Matrix[this.hLayerCount + 1];
            for (int i = 0; i < weights.Length; i++)
            {
                if (i == 0)
                {
                    if (this.hLayerCount != 0)
                        weights[i] = new Matrix(this.inputCount, this.hiddenLayers[i].column);
                    else
                    {
                        weights[i] = new Matrix(this.inputCount, this.outputCount);
                    }
                }
                else if (i == weights.Length - 1)
                {
                    weights[i] = new Matrix(this.hiddenLayers[i - 1].column, this.outputCount);
                }
                else
                {
                    weights[i] = new Matrix(this.hiddenLayers[i - 1].column, this.hiddenLayers[i].column);
                }

            }

            FillWeights();
        }

        /// <summary>
        /// Filling weights with random numbers between -1 and 1.
        /// </summary>
        public void FillWeights()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i].RandomFill();
            }
        }



        public void Train(int iterationCount)
        {
            for (int i = 0; i < iterationCount; i++)
            {
                CalculateH();
            }
            Console.WriteLine("Weights:");
            this.PrintWeights();
            Console.WriteLine("Outputs:");
            this.outputs.PrintMatrix();
        }

        /// <summary>
        /// Calculating for neurons of hidden layers and Backpropagation
        /// </summary>
        public void CalculateH()
        {
            int input_row = rnd.Next(this.inputs.row);
            if (this.hLayerCount == 0)
            {

                Matrix error, derivative , inp;

                inp = this.inputs.getRow(input_row);
                outputs = inp * this.weights[0];
                outputs = ANN.sigmoid(outputs + bias[0]);


                error = -(targets.getRow(input_row) - outputs);

                derivative = sigmoidDerivative(outputs);

                error = error.P2PProduct(derivative);


                derivative = inp.GetTranspose() * error;

                weights[0] = weights[0] - learningRate * derivative;

                return;

            }       

            
            Matrix c;
            c = this.inputs.getRow(input_row) * this.weights[0];

            this.hiddenLayers[0] = sigmoid(c + bias[0]) ;
            for (int i = 1; i < this.hLayerCount; i++)
            {
                c = this.hiddenLayers[i - 1] * weights[i];
                this.hiddenLayers[i] = sigmoid(c + bias[i]);
            }
            c = this.hiddenLayers[this.hLayerCount - 1] * weights[this.hLayerCount];
            this.outputs = sigmoid(c + bias[this.hLayerCount]);
            this.Backpropagate(input_row);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="input_row">This parameter specifies example in the traning set </param>
        public void Backpropagate(int input_row)
        {
            Matrix E = -(this.targets.getRow(input_row) - this.outputs).P2PProduct(sigmoidDerivative(outputs));
            Matrix[] temp_weights = weights;
            for (int i = 0; i < weights[this.hLayerCount].row; i++)
            {
                for (int j = 0; j < weights[this.hLayerCount].column; j++)
                {
                    temp_weights[this.hLayerCount].DataMatrix[i, j] = weights[this.hLayerCount].DataMatrix[i, j] - learningRate * hiddenLayers[this.hLayerCount - 1].DataMatrix[0,j]*E.DataMatrix[0,j]; 
                }
            }

            for (int t = this.hLayerCount - 1; t > 0; t--)
            {
                E = E * weights[t];
                E = E.P2PProduct(sigmoidDerivative(hiddenLayers[t]));
                for (int i = 0; i < weights[t].row; i++)
                {
                    for (int j = 0; j < weights[t].column; j++)
                    {
                        temp_weights[t].DataMatrix[i, j] = weights[t].DataMatrix[i, j] -learningRate*hiddenLayers[t].DataMatrix[0, j] * E.DataMatrix[0, j];
                    }
                }

            }

            E = E*weights[1];
            Matrix inp = inputs.getRow(input_row);
            E = E.P2PProduct(inp);


            for (int i = 0; i < weights[0].row; i++)
            {
                for (int j = 0; j < weights[0].column; j++)
                {
                    temp_weights[0].DataMatrix[i, j] = weights[0].DataMatrix[i, j] - learningRate * inp.DataMatrix[0,j]* E.DataMatrix[0, j];
                }
            }

            weights = temp_weights;

        }

        public void PrintWeights()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                if (i == 0)
                {
                    if(weights.Length == 1)
                    {
                        Console.WriteLine("Weights between input layer and output layer:");
                    }
                    else
                        Console.WriteLine("Weights between input layer and Layer 0:");
                }
                else
                {
                    if(i == weights.Length -1)
                        Console.WriteLine("Weights between Layer {0} and output layer:", i - 1, i + 1);
                    else
                        Console.WriteLine("Weights between Layer {0} and Layer {1}:", i - 1, i);
                }
                weights[i].PrintMatrix();
            }
        }

        public static Matrix sigmoid(Matrix x)
        {
            Matrix C = new Matrix(x.row,x.column);
            for (int i = 0; i < x.row; i++)
            {
                for (int j = 0; j < x.column; j++)
                {
                    C.DataMatrix[i, j] = (float)(1 / (1 + Math.Exp(-x.DataMatrix[i, j])));
                }
            }
            return C;
        }

        public static Matrix sigmoidDerivative(Matrix x)
        {
            Matrix C = new Matrix(x.row,x.column);
            for (int i = 0; i < x.row; i++)
            {
                for (int j = 0; j < x.column; j++)
                {
                    C.DataMatrix[i, j] = x.DataMatrix[i, j] * (1 - x.DataMatrix[i, j]);
                }
            }
            return C;
        }
    }
}
