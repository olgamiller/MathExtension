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
    public class MatrixTest
    {
        [Fact]
        public void Test_Matrix_2_Mult_MatrixMatrix()
        {
            Matrix left = new Matrix(new double[,] {
                { 1, 3 },
                { 2, 4 } });
            Matrix right = new Matrix(new double[,] {
                { 5, 7 },
                { 6, 8 } });

            Matrix expected = new Matrix(new double[,] {
                { 23, 31 },
                { 34,46 } });
            Matrix actual = left * right;

            Assert.Equal(expected, actual);
        }
    }
}