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

namespace MathExtension.LinearAlgebra
{
    public class VandermondeMatrix
    {
        private double[] _x;

        public int N { get; }

        public VandermondeMatrix(double x0, double x1, double x2, double x3)
        {
            _x = new double[] { x0, x1, x2, x3 };
            N = 4;
        }

        public VandermondeMatrix(double[] x)
        {
            _x = x;
            N = x.Length;
        }

        public static explicit operator Matrix(VandermondeMatrix vm)
        {
            Matrix m = new Matrix(vm.N, vm.N);

            for (int i = 0; i < vm.N; i++)
            {
                m[i, 0] = 1;

                for (int j = 1; j < vm.N; j++)
                {
                    m[i, j] = m[i, j - 1] * vm._x[i];
                }
            }

            return m;
        }

        public bool IsInvertable() => GetDeterminant() != 0.0;

        public double GetDeterminant()
        {
            double d = 1.0;

            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    d *= _x[j] - _x[i];
                }
            }

            return d;
        }

        // A^-1 = U^-1 * L^-1
        public Matrix GetInverse()
        {
            Matrix lowerInverse = GetLowerInverse();
            Matrix upperInverse = GetUpperInverse();
            return upperInverse * lowerInverse;
        }

        // L^-1
        public Matrix GetLowerInverse()
        {
            Matrix matrix = new Matrix(N, N);
            matrix[0, 0] = 1.0;

            for (int i = 1; i < N; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    double prod = 1.0;

                    for (int k = 0; k <= i; k++)
                    {
                        if (k != j)
                            prod *= (_x[j] - _x[k]);
                    }
                    matrix[i, j] = 1.0 / prod;
                }
            }

            return matrix;
        }

        // U^-1
        public Matrix GetUpperInverse()
        {
            Matrix matrix = new Matrix(N, N);
            matrix[0, 0] = 1.0;

            for (int j = 1; j < N; j++)
            {
                matrix[j, j] = 1.0;
                matrix[0, j] = -matrix[0, j - 1] * _x[j - 1];
            }

            for (int i = 1; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    matrix[i, j] = matrix[i - 1, j - 1] - matrix[i, j - 1] * _x[j - 1];
                }
            }

            return matrix;
        }
    }
}