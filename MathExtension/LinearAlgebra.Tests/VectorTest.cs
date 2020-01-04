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
    public class VectorTest
    {
        [Fact]
        public void Test_Vector_4_MultMatrix()
        {
            Matrix m = new Matrix(new double[,] { { 1, 3, 5, 7 }, { 2, 4, 6, 8 }, { 9, 11, 13, 15 }, { 10, 12, 14, 16 } });
            Vector v = new Vector(new double[] { 2, 3, 5, 7 });

            Vector actual = m * v;

            Assert.Equal(85, actual[0]); // 2 * 1 + 3 * 3 + 5 * 5 + 7 * 7
            Assert.Equal(102, actual[1]); // 2 * 2 + 3 * 4 + 5 * 6 + 7 * 8
            Assert.Equal(221, actual[2]); // 2 * 9 + 3 * 11 + 5 * 13 + 7 * 15
            Assert.Equal(238, actual[3]); // 2 * 10 + 3 * 12 + 5 * 14 + 7 * 16
        }
    }
}