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
using Xunit;
using MathExtension.Polynomial.Internal;

namespace MathExtension.Polynomial.Tests
{
    public class PolynomialRegressionTest
    {
        [Fact]
        public void Test_PolynomialRegression()
        {
            double[] data = new double[] { -2, -39, 0, 3, 1, 6, 3, 36 };
            Assert.Equal("2*x^3-4*x^2+5*x+3", Execute(data));
        }

        private string Execute(double[] data)
        {
            PolynomialRegression regression = new PolynomialRegression();
            Polynomial actualPolynomial = regression.Fit(data);
            string actual = actualPolynomial.ToString();
            return actual;
        }
    }
}