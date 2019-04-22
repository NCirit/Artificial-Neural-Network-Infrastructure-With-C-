using System;

namespace Matrix
{
    class Matrix
    {
        public int row;
        public int column;

        public float[,] DataMatrix;
        
        public Matrix(float data)
        {
            this.row = 1;
            this.column = 1;
            this.DataMatrix = new float[1, 1];
            DataMatrix[0, 0] = data;
        }
        public Matrix(float[,] data_matrix)
        {
            this.row = data_matrix.GetLength(0);
            this.column = data_matrix.GetLength(1);
            this.DataMatrix = data_matrix;
        }
        public Matrix(float[] data_matrix)
        {
            this.row = 1;
            this.column = data_matrix.Length;
            this.DataMatrix = new float[1,this.column];
            for (int i = 0; i < this.column; i++)
            {
                this.DataMatrix[0,i] = data_matrix[i];
            }
            
        }
        public Matrix(int row, int column)
        {
            this.row = row;
            this.column = column;
            init();
        }
        private void init()
        {
            this.DataMatrix = new float[row,column];
        }

        public void Transpose()
        {
            float[,] temp = new float[column, row];
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.column; j++)
                {
                    temp[j, i] = DataMatrix[i, j];

                }
            }
            int tmp = row;
            row = column;
            column = tmp;

            DataMatrix = temp;
        }
        public Matrix GetTranspose()
        {
            Matrix temp = new Matrix(column, row);
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.column; j++)
                {
                    temp.DataMatrix[j, i] = DataMatrix[i, j];

                }
            }
            return temp;
        }

        public Matrix getRow(int i)
        {
            float[] rw = new float[this.column];

            for (int j = 0; j < this.column; j++)
            {
                rw[j] = this.DataMatrix[i, j];
            }

            return (new Matrix(rw));
        }
        public Matrix getColumn(int i)
        {
            float[] cl = new float[this.row];

            for (int j = 0; j < this.row; j++)
            {
                cl[j] = this.DataMatrix[j, i];
            }

            return (new Matrix(cl).GetTranspose());
        }
        public void RandomFill()
        {
            Random rnd = new Random();
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.column; j++)
                {
                    DataMatrix[i, j] = (float)Math.Round(2*rnd.NextDouble()-1,5);
                }
            }
        }

        public void RandomFill(int upTo)
        {
            Random rnd = new Random();
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.column; j++)
                {
                    DataMatrix[i, j] = (float)rnd.Next(upTo);
                }
            }
        }

        public void PrintMatrix()
        {
            Console.Write("[");
            for (int i = 0; i < row; i++)
            {
                
                for (int j = 0; j < column; j++)
                {
                    Console.Write(DataMatrix[i,j]);
                    if (j != column - 1) Console.Write(",");
                }
                
                if (i != row - 1) 
                    Console.WriteLine(" ");
                Console.Write(" ");
            }
            Console.WriteLine("]");
            Console.WriteLine();
        }

        public static Matrix operator* (Matrix A , Matrix B)
        {
            if (B.row != A.column) throw new Exception("Matrix boyutları eşit değil.");

            Matrix c = new Matrix(A.row,B.column);
            float toplam = 0;
            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < B.column; j++)
                {
                    toplam = 0;
                    for (int t = 0; t < B.row; t++)
                    {
                        toplam += A.DataMatrix[i, t] * B.DataMatrix[t,j];
                    }
                    c.DataMatrix[i, j] = toplam;
                }
            }
            return c;
        }

        public static Matrix operator* (Matrix A, int x)
        {
            
            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    A.DataMatrix[i, j] *= x;
                }
            }

            return A;
        }

        public static Matrix operator *(float x ,Matrix A)
        {

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    A.DataMatrix[i, j] *= x;
                }
            }

            return A;
        }

        public static Matrix operator *(Matrix A, float x)
        {

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    A.DataMatrix[i, j] *= x;
                }
            }

            return A;
        }
        public static Matrix operator +(float x, Matrix A)
        {

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    A.DataMatrix[i, j] += x;
                }
            }

            return A;
        }
        public static Matrix operator +(Matrix A ,float x)
        {

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    A.DataMatrix[i, j] += x;
                }
            }

            return A;
        }

        public static Matrix operator *(int x, Matrix A)
        {

            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    A.DataMatrix[i, j] *= x;
                }
            }

            return A;
        }

        public static Matrix operator+ (Matrix A, Matrix B)
        {
            if (A.row != B.row || A.column != B.column) throw new Exception("Matrix boyutları eşit değil.");
            Matrix C = new Matrix(A.row,A.column);
            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    C.DataMatrix[i,j] = A.DataMatrix[i, j] + B.DataMatrix[i, j];
                }
            }

            return C;
        }

        public static Matrix operator- (Matrix A, Matrix B)
        {
            if ((A.row != B.row) || (A.column != B.column)) throw new Exception("Matrix boyutları eşit değil.");
            Matrix C = new Matrix(A.row,A.column);
            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    C.DataMatrix[i,j] = A.DataMatrix[i, j] - B.DataMatrix[i, j];
                }
            }

            return C;
        }

        public static Matrix operator -(Matrix A)
        {
            Matrix C = new Matrix(A.row, A.column);
            for (int i = 0; i < A.row; i++)
            {
                for (int j = 0; j < A.column; j++)
                {
                    C.DataMatrix[i, j] = -A.DataMatrix[i, j];
                }
            }

            return C;
        }


        // point to point product
        public Matrix P2PProduct(Matrix B)
        {
            if ((this.row != B.row) || (this.column != B.column)) throw new Exception("Row or column count not equal.");
            Matrix C = new Matrix(B.row, B.column);
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.column; j++)
                {
                    C.DataMatrix[i, j] = this.DataMatrix[i, j] * B.DataMatrix[i, j];
                }
            }

            return C;
        }
    }
}
