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

namespace MathExtension.Polynomial.Tests
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
        [InlineData("0", "0")]
        [InlineData("11", "0")]
        [InlineData("11x", "11")]
        [InlineData("-11x+5", "-11")]
        [InlineData("-x^2", "-2*x")]
        [InlineData("3x^7", "21*x^6")]
        [InlineData("x^3+5x", "3*x^2+5")]
        public void Test_Derive(string polynomial, string expected)
        {
            Polynomial actualPolynomial = Polynomial.FromString(polynomial);
            string actual = actualPolynomial.Derive().ToString();
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
        public void Test_FromString()
        {
            string expected = "5*x^2+x+1";
            Polynomial actualPolynomial = Polynomial.FromString(expected);
            string actual = actualPolynomial.ToString();
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
    }
}
