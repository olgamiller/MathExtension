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
using System;
using Xunit;
using MathExtension.Common;
using MathExtension.Polynomial.Internal;

namespace MathExtension.Polynomial.Tests
{
    public class PolynomialParserTest
    {
        [Theory]
        [InlineData("-5", "-5")]
        [InlineData(" +57 ", "57")]
        [InlineData(" +53.75 ", "53.75")]
        [InlineData(".5 ", "0.5")]
        [InlineData("5e3 ", "5000")]
        [InlineData("5E+3 ", "5000")]
        [InlineData("-5E-3 ", "-0.005")]

        [InlineData("x", "x")]
        [InlineData("-x", "-x")]
        [InlineData("x^3", "x^3")]
        [InlineData("-x^3", "-x^3")]

        [InlineData("1*x", "x")]
        [InlineData("2x", "2*x")]
        [InlineData("2*x", "2*x")]
        [InlineData("23.5x^3", "23.5*x^3")]
        [InlineData("23.5*x^3", "23.5*x^3")]

        [InlineData("-2*x^3+5", "-2*x^3+5")]
        [InlineData("-5 + 2 x", "2*x-5")]
        [InlineData("-5*x - 3x + 1345", "-8*x+1345")]
        [InlineData("-5*x^34 + 13.45", "-5*x^34+13.45")]
        [InlineData("-5*x^34", "-5*x^34")]
        [InlineData("1*x^3+13.45x^2-.5", "x^3+13.45*x^2-0.5")]

        [InlineData("0*x^3", "0")]
        [InlineData("5+x", "x+5")]
        [InlineData("x+x", "2*x")]
        [InlineData("3*x+x", "4*x")]
        [InlineData("3*x^+45", "3*x^45")]
        public void Test_Valid(string input, string expected)
        {
            Polynomial actualPolynomial = PolynomialParser.TryParse(input);
            string actual = actualPolynomial.ToString();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("52*-", "Syntax error at 3.")]
        [InlineData("3*-4", "Syntax error at 2.")]
        [InlineData("3*    x4", "Syntax error at 3.")]
        [InlineData("x*4", "Syntax error at 1.")]
        public void Test_SyntaxException(string input, string expectedError)
        {
            Exception ex = Assert.Throws<SyntaxException>(() => PolynomialParser.TryParse(input));
            Assert.Equal(expectedError, ex.Message);
        }

        [Theory]
        [InlineData("-5^5", "Coefficient syntax error at 0.")]
        [InlineData("--5", "Coefficient syntax error at 0.")]
        [InlineData("*4", "Coefficient syntax error at 0.")]
        [InlineData("4+*x", "Coefficient syntax error at 1.")]
        [InlineData("*x", "Coefficient syntax error at 0.")]
        public void Test_CoefficientSyntaxException(string input, string expectedError)
        {
            Exception ex = Assert.Throws<CoefficientSyntaxException>(() => PolynomialParser.TryParse(input));
            Assert.Equal(expectedError, ex.Message);
        }

        [Theory]
        [InlineData("x^-5", "Exponent syntax error at 2.")]
        [InlineData("3*x^*4", "Exponent syntax error at 4.")]
        [InlineData("x^+m", "Exponent syntax error at 2.")]
        [InlineData("x^4m", "Exponent syntax error at 2.")]
        public void Test_ExponentSyntaxException(string input, string expectedError)
        {
            Exception ex = Assert.Throws<ExponentSyntaxException>(() => PolynomialParser.TryParse(input));
            Assert.Equal(expectedError, ex.Message);
        }
    }
}