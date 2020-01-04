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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using MathExtension.Polynomial.Internal;

[assembly: InternalsVisibleTo("Polynomial.Tests")]
namespace MathExtension.Polynomial
{
    // Univariate Polynomial
    public class Polynomial : IEnumerable
    {
        private SortedDictionary<uint, double> _Items;

        public uint HighestExponent { get => _Items.Count == 0 ? 0 : _Items.Last<KeyValuePair<uint, double>>().Key; }
        public static Polynomial Zero { get => new Polynomial(); }

        /// <summary>
        /// <see cref="PolynomialParser"/> 
        /// </summary>
        /// <exception cref="SyntaxException"/>
        public static Polynomial FromString(string polynomial)
        {
            Polynomial result = PolynomialParser.TryParse(polynomial);
            return result;
        }

        public static Polynomial FromPolynomial(Polynomial polynomial)
        {
            return (Polynomial)polynomial?.MemberwiseClone();
        }

        public static Polynomial FromDataPoints(double[] data)
        {
            PolynomialRegression regression = new PolynomialRegression();
            Polynomial polynomial = regression.Fit(data);
            return polynomial;
        }

        public Polynomial()
        {
            _Items = new SortedDictionary<uint, double>();
        }

        public Polynomial(uint exponent, double coefficient)
            : this()
        {
            Merge(exponent, coefficient);
        }

        public void Add((uint exponent, double coefficient) item) => Merge(item.exponent, item.coefficient);
        public void Add(uint exponent, double coefficient) => Merge(exponent, coefficient);
        public IEnumerator GetEnumerator() => (IEnumerator)_Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)GetEnumerator();

        private void Merge(uint exponent, double coefficient)
        {
            if (coefficient == 0.0)
                return;

            if (!_Items.ContainsKey(exponent))
                _Items.Add(exponent, coefficient);
            else
            {
                double newCoefficient = _Items[exponent] + coefficient;

                if (newCoefficient == 0.0)
                    _Items.Remove(exponent);
                else
                    _Items[exponent] = newCoefficient;
            }
        }

        public Polynomial Add(Polynomial p)
        {
            return Add(this, p);
        }

        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            return Add(p1, p2);
        }

        private static Polynomial Add(Polynomial p1, Polynomial p2)
        {
            Polynomial result = (Polynomial)p1.MemberwiseClone();

            foreach (uint key in p2._Items.Keys)
                result.Merge(key, p2._Items[key]);

            return result;
        }

        public Polynomial Subtract(Polynomial p)
        {
            return Subtract(this, p);
        }

        public static Polynomial operator -(Polynomial p1, Polynomial p2)
        {
            return Subtract(p1, p2);
        }

        private static Polynomial Subtract(Polynomial p1, Polynomial p2)
        {
            Polynomial result = (Polynomial)p1.MemberwiseClone();

            foreach (uint key in p2._Items.Keys)
                result.Merge(key, -p2._Items[key]);

            return result;
        }

        public Polynomial Multiply(Polynomial p)
        {
            return Multiply(this, p);
        }

        public static Polynomial operator *(Polynomial p1, Polynomial p2)
        {
            return Multiply(p1, p2);
        }

        private static Polynomial Multiply(Polynomial p1, Polynomial p2)
        {
            Polynomial result = new Polynomial();

            foreach (uint key2 in p2._Items.Keys)
            {
                double coefficient2 = p2._Items[key2];

                foreach (uint key1 in p1._Items.Keys)
                    result.Merge(key2 + key1, coefficient2 * p1._Items[key1]);
            }

            return result;
        }

        public (Polynomial quotient, Polynomial remainder) Divide(Polynomial p)
        {
            return Divide(this, p);
        }

        public static (Polynomial quotient, Polynomial remainder) operator /(Polynomial p1, Polynomial p2)
        {
            return Divide(p1, p2);
        }

        private static (Polynomial quotient, Polynomial remainder) Divide(Polynomial p1, Polynomial p2)
        {
            if (p2._Items.Count == 0)
                throw new DivideByZeroException();

            Polynomial quotient = Zero;
            Polynomial remainder = (Polynomial)p1.MemberwiseClone();
            KeyValuePair<uint, double> p2Last = p2._Items.Last();

            do
            {
                if (remainder._Items.Count == 0)
                    return (quotient, remainder);

                KeyValuePair<uint, double> p1Last = remainder._Items.Last();

                if (p1Last.Key < p2Last.Key)
                    return (quotient, remainder);

                Polynomial quotientItem = new Polynomial(p1Last.Key - p2Last.Key, p1Last.Value / p2Last.Value);
                quotient += quotientItem;
                remainder -= p2 * quotientItem;

            } while (true);
        }

        public double GetY(double x)
        {
            double result = 0;

            foreach (uint exponent in _Items.Keys)
            {
                double coefficent = _Items[exponent];
                result += coefficent * Math.Pow(x, exponent);
            }

            return result;
        }

        public Polynomial Derive()
        {
            Polynomial polynomial = Zero;

            foreach (uint exponent in _Items.Keys)
            {
                if (exponent != 0)
                {
                    double coefficent = _Items[exponent];
                    polynomial.Merge(exponent - 1, coefficent * exponent);
                }
            }

            return polynomial;
        }

        public override string ToString()
        {
            if (_Items.Count <= 0)
                return "0";

            List<uint> keys = new List<uint>();
            keys.AddRange(_Items.Keys);

            StringBuilder s = new StringBuilder();

            for (int i = keys.Count - 1; i >= 0; --i)
            {
                uint exponent = keys[i];
                double coefficient = _Items[exponent];

                if (coefficient < 0.0)
                    s.Append("-");
                else if (i != keys.Count - 1)
                    s.Append("+");

                double coefficientAbs = Math.Abs(coefficient);
                if (coefficientAbs != 1.0)
                {
                    s.Append(coefficientAbs);
                    if (exponent != 0)
                        s.Append("*");
                }
                else if (exponent == 0)
                    s.Append("1");

                if (exponent != 0)
                {
                    if (exponent == 1)
                        s.Append("x");
                    else
                    {
                        s.Append("x^");
                        s.Append(exponent);
                    }
                }
            }

            return s.ToString();
        }
    }
}
