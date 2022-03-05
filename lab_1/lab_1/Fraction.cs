using System;
using System.Diagnostics.CodeAnalysis;

namespace lab_1
{
    public readonly struct Fraction : IEquatable<Fraction>
    {
        private readonly int denominator;
        private readonly int numerator;

        public float Denominator => denominator;
        public float Numerator => numerator;

        public Fraction(int denominator, int numerator)
        {
            int greatestCommonDivisor = MathHelpers.GetGreatestCommonDivisor(denominator, numerator);
            this.denominator = denominator / greatestCommonDivisor;
            this.numerator  = numerator / greatestCommonDivisor;
        }

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
            return $"{numerator}/{denominator}";
        }
    }
}
