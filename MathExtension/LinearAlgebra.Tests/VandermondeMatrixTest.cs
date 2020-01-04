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
using Xunit;

namespace MathExtension.LinearAlgebra.Tests
{
    public class VandermondeMatrixTest
    {
        [Fact]
        public void Test_VandermondeMatrix_4_ToMatrix()
        {
            VandermondeMatrix vm = new VandermondeMatrix(2, 3, 5, 7);
            Matrix m = (Matrix)vm;

            Assert.Equal(1, m[0, 0]);
            Assert.Equal(1, m[1, 0]);
            Assert.Equal(1, m[2, 0]);
            Assert.Equal(1, m[3, 0]);

            Assert.Equal(2, m[0, 1]);
            Assert.Equal(3, m[1, 1]);
            Assert.Equal(5, m[2, 1]);
            Assert.Equal(7, m[3, 1]);

            Assert.Equal(4, m[0, 2]);
            Assert.Equal(9, m[1, 2]);
            Assert.Equal(25, m[2, 2]);
            Assert.Equal(49, m[3, 2]);

            Assert.Equal(8, m[0, 3]);
            Assert.Equal(27, m[1, 3]);
            Assert.Equal(125, m[2, 3]);
            Assert.Equal(343, m[3, 3]);
        }

        [Fact]
        public void Test_VandermondeMatrix_4_Determinant()
        {
            VandermondeMatrix vm = new VandermondeMatrix(2, 3, 5, 7);
            double actual = vm.GetDeterminant();
            double expected = (7 - 5) * (7 - 3) * (7 - 2) * (5 - 3) * (5 - 2) * (3 - 2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_VandermondeMatrix_3_LowerInverse()
        {
            VandermondeMatrix vm = new VandermondeMatrix(new double[] { 0, 2, 4 });
            Matrix expected = new Matrix(new double[,] {
                { 1, 0, 0} ,
                { -0.5, 0.5, 0 },
                { 0.125, -0.25, 0.5 }
            });
            Matrix actual = vm.GetLowerInverse();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_VandermondeMatrix_4_UpperInverse()
        {
            VandermondeMatrix vm = new VandermondeMatrix(2, 3, 5, 7);
            Matrix expected = new Matrix(new double[,] {
                { 1, -2, 6, -30 },
                { 0, 1, -5, 31 },
                { 0, 0, 1, -10 },
                { 0, 0, 0, 1 }
            });
            Matrix actual = vm.GetUpperInverse();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_VandermondeMatrix_4_Inverse()
        {
            // (-2, -39), (0, 3), (1, 6), (3, 36)
            VandermondeMatrix vm = new VandermondeMatrix(-2, 0, 1, 3);
            Vector y = new Vector(new double[] { -39, 3, 6, 36 });

            double determinant = vm.GetDeterminant();
            Assert.NotEqual(0, determinant);

            Matrix m = vm.GetInverse();
            Vector actual = m * y;

            // p(x) = 2x^3-4x^2+5x+3
            Assert.Equal(3, actual[0]);
            Assert.Equal(5, actual[1]);
            Assert.Equal(-4, actual[2]);
            Assert.Equal(2, actual[3]);
        }

        [Fact]
        public void Test_VandermondeMatrix_4_Inverse2()
        {
            // (0, 3), (1, 6), (3, 36), (4, 9)
            VandermondeMatrix vm = new VandermondeMatrix(0, 1, 3, 4);
            Vector y = new Vector(new double[] { 3, 6, 36, 9 });

            double determinant = vm.GetDeterminant();
            Assert.NotEqual(0, determinant);

            Matrix m = vm.GetInverse();
            Vector actual = m * y;

            // p(x) = -4.5x^3 + 22x^2 -14.5x + 3
            Assert.Equal(3, actual[0]);
            Assert.Equal(-14.5, actual[1]);
            Assert.Equal(0, 22 - actual[2], 14);
            Assert.Equal(-4.5, actual[3]);
        }
    }
}