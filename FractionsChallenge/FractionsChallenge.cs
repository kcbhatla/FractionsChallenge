using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace FractionsChallengeNS
{
    class FractionsChallenge
    {

        public const string IncorrectInputString = "The input string/parameter passed is not in the correct format";

        private readonly string m_parameters;
        private bool m_hasFractions = false;

        private FractionsChallenge() { }

        public FractionsChallenge(string parameters)
        {
            m_parameters = parameters;
        }

        public void Compute()
        {
            if (!m_parameters.StartsWith("?"))
            {
                Console.WriteLine("Please input the string in the following format ? followed by numbers and operands.");
                return;
            }
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            String parameters = m_parameters.Replace("?", String.Empty);
            parameters = parameters.Trim();
            string computeValues = regex.Replace(parameters, " ");

            string[] values = computeValues.Split(" ");

            List<string> valueList = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                int parseIntResult;
                double parseDoubleResult;

                string valAtiThPosition = values[i];
                string valAtPrevPosition = "?";
                string vatAtNextPosition = "?";

                if (i - 1 >= 0)
                {
                    valAtPrevPosition = values[i - 1];
                }

                if (i + 1 < values.Length)
                {
                    vatAtNextPosition = values[i + 1];
                }

                if (int.TryParse(valAtiThPosition, out parseIntResult))
                {
                    valueList.Add(valAtiThPosition);
                }
                else if (double.TryParse(valAtiThPosition, out parseDoubleResult))
                {
                    valueList.Add(valAtiThPosition);
                }
                else if (isValidOperand(valAtiThPosition))
                {
                    valueList.Add(valAtiThPosition);
                }
                else if (isFraction(valAtiThPosition))
                {
                    valueList.Add(valAtiThPosition);
                }
            }

            string eval = "";
            string temp = "";
            
            for (int j = 0; j < valueList.Count; j++)
            {
                if (!isValidOperand(valueList.ElementAt(j)))
                {
                        temp = FractionToDouble(valueList.ElementAt(j)).ToString() + " ";
                }
                else
                {
                    temp = valueList.ElementAt(j) + " ";
                }
                    
                eval = eval + temp;
            }
            
            Console.WriteLine(Decimal2Fraction(Convert.ToDouble(new DataTable().Compute(eval, null))));
        }

        bool isFraction(string fraction)
        {
            int lValResult;
            int lNumerator;
            int lDenominator;
            int numResult;
            int denomResult;
            string lVal;
            string rVal;

            m_hasFractions = false;

            if (fraction.IndexOf("_") > 0 && Regex.Matches(fraction, "/").Count == 1)
            {
                lVal = fraction.Substring(0, fraction.IndexOf("_"));
                rVal = fraction.Substring(fraction.IndexOf("_") + 1);
                if (int.TryParse(lVal, out lValResult) && rVal.Contains("/") && Regex.Matches(rVal, "/").Count == 1 && int.TryParse(rVal.Substring(0, rVal.IndexOf("/")), out numResult) && int.TryParse(rVal.Substring(rVal.IndexOf("/") + 1), out denomResult))
                {
                    m_hasFractions = true;
                }
            }
            else if (Regex.Matches(fraction, "/").Count == 1)
            {
                int.TryParse(fraction.Substring(0, fraction.IndexOf("/")), out lNumerator);
                int.TryParse(fraction.Substring(fraction.IndexOf("/") + 1), out lDenominator);
                if (lNumerator >= 0 && lDenominator > 0)
                {
                    m_hasFractions = true;
                }
            }
            return m_hasFractions;
        }

        bool isValidOperand(string op)
        {
            
            if (!isFraction(op))
            {
                if (op == "*" || op == "/" || op == "+" || op == "-")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        double FractionToDouble(string fraction)
        {
            double result;
            double val;

            if (double.TryParse(fraction, out result))
            {
                return result;
            }

            int wholeNum = 0;
            if (fraction.IndexOf("_") > 0)
            {
                int.TryParse(fraction.Substring(0, fraction.IndexOf("_")), out wholeNum);
                fraction = fraction.Substring(fraction.IndexOf("_") + 1);
            }

            string[] split = fraction.Split(new char[] { ' ', '/' });

            if (split.Length == 2 || split.Length == 3)
            {
                int a, b;

                if (int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
                {
                    if (split.Length == 2)
                    {
                        val = (double)a / b;
                        return wholeNum + val;
                    }

                    int c;

                    if (int.TryParse(split[2], out c))
                    {
                        val = a + (double)b / c;
                        return wholeNum + val;
                    }
                }
            }

            throw new FormatException("Not a valid fraction.");
        }

        

        public int GetGreatestDivisor(int m, int h)
        {

            return (int) BigInteger.GreatestCommonDivisor(m, h);
        }

        public string Decimal2Fraction(double dblDecimal)
        {

            string txtDecimal = null;
            txtDecimal = dblDecimal.ToString();
            double result;
            if ( !double.TryParse(txtDecimal, out result))
            {
                throw new FormatException("Not a valid decimal number.");
            }

            int wholeNumber = 0;
            string decimalPart;

            try
            {
                if (txtDecimal.IndexOf(".") > 0)
                    wholeNumber = int.Parse(txtDecimal.Substring(0, txtDecimal.IndexOf(".")));
                else
                    wholeNumber = int.Parse(txtDecimal);
            }
            catch
            {
                throw new FormatException("Not a valid integer.");
            }

            double lenDecimal;
            double decNum;
            if (txtDecimal.IndexOf(".") > 0)
                decimalPart = txtDecimal.Substring(txtDecimal.IndexOf(".") + 1);
            else
                decimalPart = "";
            try
            {
                double.TryParse(decimalPart, NumberStyles.Any, CultureInfo.InvariantCulture, out decNum);
            }
            catch
            {
                throw new FormatException("Unable to parse the numerator.");
            }

            lenDecimal = decimalPart.Length;

            double decimalDenom = Math.Pow((double)10, (double)lenDecimal);
            if (decNum % decimalDenom != 0)
            {
                int ggd = GetGreatestDivisor((int)decNum, (int)decimalDenom);
                decNum = decNum / ggd;
                decimalDenom = decimalDenom / ggd;
            }
            string fractionString = decNum == 0 ? "" : decNum.ToString() + "/" + decimalDenom.ToString();

            if (wholeNumber == 0)
                return fractionString;
            else
                if (fractionString == String.Empty)
                    return wholeNumber.ToString();
                else
                    return fractionString == "" ? wholeNumber.ToString() : wholeNumber.ToString() + "_" + fractionString;
        }

        public static void Main(string[] args)
        {
            string line;

            while (((line = Console.ReadLine()) != null) && (line != ""))
            {
                FractionsChallenge fc = new FractionsChallenge(line);
                fc.Compute();
            }
        }

           
    }
}

