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
using System.Collections.Generic;
using Xunit;
using MathExtension;

namespace MathExtensionTests
{
    public class PolynomialTest
    {
        [Fact]
        public void Test_ToString_Exponent0()
        {
            Polynomial p;

            p = new Polynomial();
            Assert.Equal("0", p.ToString());

            p = new Polynomial(1, 0);
            Assert.Equal("0", p.ToString());

            p = new Polynomial(0, 0);
            Assert.Equal("0", p.ToString());

            p = new Polynomial(0, 1);
            Assert.Equal("1", p.ToString());

            p = new Polynomial(0, -1);
            Assert.Equal("-1", p.ToString());

            p = new Polynomial(0, 2.3);
            Assert.Equal("2.3", p.ToString());

            p = new Polynomial(0, -5.13);
            Assert.Equal("-5.13", p.ToString());
        }

        [Fact]
        public void Test_ToString_Exponent1()
        {
            Polynomial p;

            p = new Polynomial(1, 1);
            Assert.Equal("x", p.ToString());

            p = new Polynomial(1, -1);
            Assert.Equal("-x", p.ToString());

            p = new Polynomial(1, 0.5);
            Assert.Equal("0.5*x", p.ToString());

            p = new Polynomial(1, -2);
            Assert.Equal("-2*x", p.ToString());
        }

        [Fact]
        public void Test_ToString_Exponent2()
        {
            Polynomial p;

            p = new Polynomial(2, 1);
            Assert.Equal("x^2", p.ToString());

            p = new Polynomial(2, -1);
            Assert.Equal("-x^2", p.ToString());

            p = new Polynomial(2, 0.5);
            Assert.Equal("0.5*x^2", p.ToString());

            p = new Polynomial(2, -3);
            Assert.Equal("-3*x^2", p.ToString());
        }

        [Fact]
        public void Test_ToString_ExponentCombined()
        {
            Polynomial p;

            p = new Polynomial { { 0, -5 }, { 3, -1 } };
            Assert.Equal("-x^3-5", p.ToString());

            p = new Polynomial { { 1, 1 }, { 0, 1 } };
            Assert.Equal("x+1", p.ToString());

            p = new Polynomial { { 1, -1 }, { 0, -1 } };
            Assert.Equal("-x-1", p.ToString());

            p = new Polynomial { { 4, -1 }, { 0, -1 } };
            Assert.Equal("-x^4-1", p.ToString());

            p = new Polynomial { { 1, 1 }, { 3, -1 }, { 6, 9 }, { 2, 7 } };
            Assert.Equal("9*x^6-x^3+7*x^2+x", p.ToString());
        }

        [Fact]
        public void Test_Multiply()
        {
            Polynomial p, p2;

            p = new Polynomial(0, -5);
            p2 = new Polynomial(1, 2);
            Assert.Equal("-10*x", p.Multiply(p2).ToString());

            p = new Polynomial { { 0, -5 }, { 3, -1 } };
            p2 = new Polynomial { { 1, 2 }, { 3, 2 }, { 4, -7 } };
            Assert.Equal("7*x^7-2*x^6+33*x^4-10*x^3-10*x", p.Multiply(p2).ToString());
        }

        [Fact]
        public void Test_Divide1()
        {
            Polynomial p, p2;
            Polynomial quotient, remainder;

            p = new Polynomial(0, 30);
            p2 = new Polynomial(0, 5);
            (quotient, remainder) = p / p2;
            Assert.Equal("6", quotient.ToString());
            Assert.Equal("0", remainder.ToString());

            p = new Polynomial(3, 1);
            p2 = new Polynomial(1, 1);
            (quotient, remainder) = p.Divide(p2);
            Assert.Equal("x^2", quotient.ToString());
            Assert.Equal("0", remainder.ToString());

            p = new Polynomial(3, 1);
            p2 = new Polynomial(2, 1);
            (quotient, remainder) = p / p2;
            Assert.Equal("x", quotient.ToString());
            Assert.Equal("0", remainder.ToString());
        }

        [Fact]
        public void Test_Divide2()
        {
            Polynomial p, p2;
            Polynomial quotient, remainder;

            p = new Polynomial { { 3, 1 }, { 0, -4 } };
            p2 = new Polynomial(2, 1);
            (quotient, remainder) = p / p2;
            Assert.Equal("x", quotient.ToString());
            Assert.Equal("-4", remainder.ToString());
        }

        [Fact]
        public void Test_Divide3()
        {
            Polynomial p, p2;
            Polynomial quotient, remainder;

            p = new Polynomial { { 1, 3 }, { 0, -4 } };
            p2 = new Polynomial { { 1, 1 }, { 0, -3 } };
            (quotient, remainder) = p / p2;
            Assert.Equal("3", quotient.ToString());
            Assert.Equal("5", remainder.ToString());

            p = new Polynomial { { 3, 1 }, { 2, -2 }, { 0, -4 } };
            p2 = new Polynomial { { 1, 1 }, { 0, -3 } };
            (quotient, remainder) = p / p2;
            Assert.Equal("x^2+x+3", quotient.ToString());
            Assert.Equal("5", remainder.ToString());
        }

        [Theory]
        [InlineData("0", 5, 0)]
        [InlineData("5", -100, 5)]
        [InlineData("-x", 5, -5)]
        [InlineData("x^2", 5, 25)]
        [InlineData("x^2 + 1", 5, 26)]
        [InlineData("2*x^2 + x + 0.5", 3, 21.5)]
        public void Test_GetY(string polynomial, double x, double expected)
        {
            Polynomial actualPolynomial = Polynomial.FromString(polynomial);
            double actual = actualPolynomial.GetY(x);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("5", 0)]
        [InlineData("-x+1", 1)]
        [InlineData("x^2", 2)]
        [InlineData("x^3 + x^2", 3)]
        public void Test_HighestExponent(string polynomial, uint expected)
        {
            Polynomial actualPolynomial = Polynomial.FromString(polynomial);
            uint actual = actualPolynomial.HighestExponent;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_FromPolynomial()
        {
            string expected = "5*x^2+x+1";
            Polynomial polynomial = Polynomial.FromString(expected);
            string actual = Polynomial.FromPolynomial(polynomial).ToString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_FromPolynomial_Null()
        {
            Assert.Null(Polynomial.FromPolynomial(null));
        }

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
        public void Test_FromString(string input, string expected)
        {
            Polynomial actualPolynomial = Polynomial.FromString(input);
            string actual = actualPolynomial.ToString();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("52*-", "Syntax error at 3.")]
        [InlineData("3*-4", "Syntax error at 2.")]
        [InlineData("3*    x4", "Syntax error at 3.")]
        [InlineData("x*4", "Syntax error at 1.")]
        public void Test_FromString_SyntaxException(string input, string expectedError)
        {
            Exception ex = Assert.Throws<SyntaxException>(() => Polynomial.FromString(input));
            Assert.Equal(expectedError, ex.Message);
        }

        [Theory]
        [InlineData("-5^5", "Coefficient syntax error at 0.")]
        [InlineData("--5", "Coefficient syntax error at 0.")]
        [InlineData("*4", "Coefficient syntax error at 0.")]
        [InlineData("4+*x", "Coefficient syntax error at 1.")]
        [InlineData("*x", "Coefficient syntax error at 0.")]
        public void Test_FromString_CoefficientSyntaxException(string input, string expectedError)
        {
            Exception ex = Assert.Throws<CoefficientSyntaxException>(() => Polynomial.FromString(input));
            Assert.Equal(expectedError, ex.Message);
        }

        [Theory]
        [InlineData("x^-5", "Exponent syntax error at 2.")]
        [InlineData("3*x^*4", "Exponent syntax error at 4.")]
        [InlineData("x^+m", "Exponent syntax error at 2.")]
        [InlineData("x^4m", "Exponent syntax error at 2.")]
        public void Test_FromString_ExponentSyntaxException(string input, string expectedError)
        {
            Exception ex = Assert.Throws<ExponentSyntaxException>(() => Polynomial.FromString(input));
            Assert.Equal(expectedError, ex.Message);
        }
    }
}
