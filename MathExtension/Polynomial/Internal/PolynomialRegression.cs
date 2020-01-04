/*
Copyright 2018 Olga Miller <olga.rgb@gmail.com>
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
using MathExtension.LinearAlgebra;

namespace MathExtension.Polynomial.Internal
{
    internal class PolynomialRegression
    {
        public Polynomial Fit(double[] data)
        {
            Polynomial polynomial = new Polynomial();
            Vector coeffs = GetCoeffs(data);

            for (int i = 0; i < coeffs.Length; i++)
            {
                polynomial.Add((uint)i, coeffs[i]);
            }

            return polynomial;
        }

        private Vector GetCoeffs(double[] data)
        {
            Polynomial polynomial = new Polynomial();
            int length = data.Length / 2;
            double[] x = new double[length];
            double[] y = new double[length];

            for (int i = 0; i < length; i++)
            {
                x[i] = data[i * 2];
                y[i] = data[i * 2 + 1];
            }

            VandermondeMatrix vm = new VandermondeMatrix(x);
            Vector v = new Vector(y);
            return vm.GetInverse() * v;
        }
    }
}