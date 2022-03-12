using System;

namespace lab_1
{
    public readonly struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
    {
        private readonly int denominator;
        private readonly int numerator;

        public float Denominator => denominator;
        public float Numerator => numerator;

        /// <summary>
        /// Creates a Fraction with a numerator and denominator
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <exception cref="DivideByZeroException">throws an exeption if denominator is zero</exception>
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new DivideByZeroException("Denominator of Fraction is 0");
            }
            if (numerator < 0 && denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }
            if(numerator == 0)
            {
                denominator = 1;
            }
            else
            {
                int greatestCommonDivisor = MathHelpers.GetGreatestCommonDivisor(numerator, denominator);
                if (greatestCommonDivisor != 1)
                {
                    denominator /= greatestCommonDivisor;
                    numerator /= greatestCommonDivisor;
                }
            }
            this.numerator = numerator;
            this.denominator = denominator;
        }

        /// <summary>
        /// Creates a Fraction using values of other Fraction
        /// </summary>
        /// <param name="other"></param>
        public Fraction(Fraction other)
        {
            this.denominator = other.denominator;
            this.numerator = other.numerator;
        }

        public bool Equals(Fraction other)
        {
            return other.numerator == this.numerator && other.denominator == this.denominator;
        }

        public override string ToString()
        {
            if (denominator == 0)
            {
                return "Not valid fraction, division by 0";
            }
            else if (numerator == 0)
            {
                return "0";
            }
            else
            {
                return $"{numerator}/{denominator}";
            }
        }
        public int CompareTo(Fraction other)
        {
            if (this.denominator == other.denominator)
            {
                return this.numerator.CompareTo(other.numerator);
            }
            else
            {
                int thisNumerator = this.numerator * other.denominator;
                int otherNumerator = other.numerator * this.denominator;
                return thisNumerator.CompareTo(otherNumerator);
            }
        }

        
    }
}
