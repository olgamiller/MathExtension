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

namespace MathExtension
{
    public static class PolynomialParser
    {
        public class ParserException : Exception
        {
            public ParserException(int position)
                : base($"Error at {position}.")
            {
            }
        }

        /// <summary>
        /// The following tokens are supported:
        ///     Variables: x
        ///     Operators: +, -, *, ^
        ///
        /// Exponent type: uint
        /// Coefficient type: double
        ///
        /// Example: 2.5x^3+x+1, or 2.5*x^3+x+1
        /// </summary>
        /// <exception cref="ParserException"/>
        public static Polynomial TryParse(string polynomialString)
        {
            string s = PrepareString(polynomialString);
            Polynomial polynomial = new Polynomial();

            int pos = 0;
            polynomial.Add(TryParseItem(s, ref pos));

            while (pos < s.Length)
            {
                if (s[pos] == '+' || s[pos] == '-')
                    polynomial.Add(TryParseItem(s, ref pos));
                else
                    throw new ParserException(pos);
            }

            return polynomial;
        }

        private static string PrepareString(string polynomialString)
        {
            if (polynomialString == null)
                throw new ArgumentNullException();

            string s = polynomialString.Replace(" ", "").ToLower();
            return s;
        }

        private static (uint, double) TryParseItem(string s, ref int pos)
        {
            (uint exponent, double coefficient) item = (0, 1.0);

            if (pos < s.Length && s[pos] != 'x')
            {
                item.coefficient = TryParseCoefficient(s, ref pos);

                if (pos < s.Length && s[pos] == '*')
                    pos++;
            }

            if (pos < s.Length && s[pos] == 'x')
            {
                pos++;
                item.exponent = TryParseExponent(s, ref pos);
            }

            return item;
        }

        private static double TryParseCoefficient(string s, ref int pos)
        {
            int startPos = pos;
            GoToTokenAfterCoefficient(s, startPos, ref pos);
            double coefficient = TryParseCoefficient(s, startPos, pos);
            return coefficient;
        }

        private static void GoToTokenAfterCoefficient(string s, int startPos, ref int pos)
        {
            pos++;

            while (pos < s.Length && s[pos] != '+' && s[pos] != '-' && s[pos] != '*' && s[pos] != 'x')
            {
                switch (s[pos])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                        pos++;
                        break;
                    case 'e':
                        pos++;
                        if (s[pos] == '+' || s[pos] == '-')
                            pos++;
                        break;
                    default:
                        throw new ParserException(startPos);
                }
            }
        }

        private static double TryParseCoefficient(string s, int startPos, int pos)
        {
            string coefficientString = s.Substring(startPos, pos - startPos);
            double coefficient;

            if ((coefficientString == string.Empty || coefficientString == "+") &&
                (pos < s.Length && s[pos] == 'x'))
                coefficient = 1;
            else if (coefficientString == "-" && (pos < s.Length && s[pos] == 'x'))
                coefficient = -1;
            else if (!double.TryParse(coefficientString, out coefficient))
                throw new ParserException(startPos);

            return coefficient;
        }

        private static uint TryParseExponent(string s, ref int pos)
        {
            uint exponent;

            if (pos >= s.Length || s[pos] != '^')
                exponent = 1;
            else
            {
                pos++;
                int startPos = pos;
                GoToTokenAfterExponent(s, startPos, ref pos);
                exponent = TryParseExponent(s, startPos, pos);
            }

            return exponent;
        }

        private static void GoToTokenAfterExponent(string s, int startPos, ref int pos)
        {
            pos++;

            while (pos < s.Length && s[pos] != '+' && s[pos] != '-')
            {
                switch (s[pos])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        pos++;
                        break;
                    default:
                        throw new ParserException(startPos);
                }
            }
        }

        private static uint TryParseExponent(string s, int startPos, int pos)
        {
            string exponentString = s.Substring(startPos, pos - startPos);
            uint exponent;

            if (!uint.TryParse(exponentString, out exponent))
                throw new ParserException(startPos);

            return exponent;
        }
    }
}