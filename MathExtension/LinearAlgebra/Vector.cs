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
    public class Vector
    {
        private double[] _v;

        public int Length { get; }
        public double this[int i]
        {
            get => _v[i];
            set => _v[i] = value;
        }

        public Vector(int length)
        {
            _v = new double[length];
            Length = length;
        }

        public Vector(double[] v)
        {
            _v = v;
            Length = v.Length;
        }

        public static Vector operator *(Matrix m, Vector x)
        {
            Vector y = new Vector(x.Length);

            for (int i = 0; i < m.Rows; i++)
            {
                y[i] = 0;

                for (int j = 0; j < m.Columns; j++)
                {
                    y[i] += m[i, j] * x[j];
                }
            }

            return y;
        }
    }
}