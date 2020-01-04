/*
Copyright 2020 Olga Miller <olga.rgb@gmail.com>
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System.Linq;

namespace MathExtension.LinearAlgebra
{
    public class Matrix
    {
        private double[,] _a;

        public int Rows { get; }
        public int Columns { get; }
        public double this[int i, int j]
        {
            get => _a[i, j];
            set => _a[i, j] = value;
        }

        public Matrix(int rows, int columns)
        {
            _a = new double[rows, columns];
            Rows = rows;
            Columns = columns;
        }

        public Matrix(double[,] a)
        {
            _a = a;
            Rows = a.GetLength(0);
            Columns = a.GetLength(1);
        }

        public static bool operator ==(Matrix left, Matrix right)
        {
            if (left.Rows != right.Rows || left.Columns != right.Columns)
                return false;

            return left._a.Cast<double>().SequenceEqual(left._a.Cast<double>());
        }

        public static bool operator !=(Matrix left, Matrix right)
        {
            return !(left == right);
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            Matrix matrix = new Matrix(left.Rows, right.Columns);

            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Columns; j++)
                {
                    double d = 0.0;
                    for (int k = 0; k < right.Rows; k++)
                    {
                        d += left[i, k] * right[k, j];
                    }
                    matrix[i, j] = d;
                }
            }

            return matrix;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix))
                return false;

            return this == (Matrix)obj;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}