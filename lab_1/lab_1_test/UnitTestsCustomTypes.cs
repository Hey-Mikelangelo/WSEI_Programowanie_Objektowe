using lab_1;
using System;
using Xunit;

namespace lab_1_test
{
    public class UnitTestsCustomTypes
    {
        public class TestsFraction
        {
            [Fact]
            public void Constructor_Default_ZeroValues()
            {
                Fraction fraction = new Fraction();
                Assert.Equal(0, fraction.Numerator);
                Assert.Equal(0, fraction.Denominator);
            }

            [Fact]
            public void Constructor_ZeroDenominator_ThrowsExeption()
            {
                Assert.Throws<DivideByZeroException>(() => new Fraction(99, 0));
            }

            [Theory]
            [InlineData(9999, 1)]
            [InlineData(1, 9999)]
            [InlineData(1, 1)]
            public void Constructor_PositiveNumbers_NoExeptions(int numerator, int denominator)
            {
                var exeption = Record.Exception(() =>
                {
                    Fraction fraction = new Fraction(numerator, denominator);
                });
                Assert.Null(exeption);
            }

            [Theory]
            [InlineData(1, 0)]
            [InlineData(9999, 0)]
            [InlineData(-1, 0)]
            [InlineData(-9999, 0)]
            public void Constructor_NumeratorWithZeroDenominator_ThrowsExeption(int numerator, int denominator)
            {
                Assert.Throws<DivideByZeroException>(() => new Fraction(numerator, denominator));
            }

            [Fact]
            public void Consturctor_ZeroNumerator_DenominatorIsOne()
            {
                Fraction fraction = new Fraction(0, 55);
                Assert.Equal(1, fraction.Denominator);
            }

            [Theory]
            [InlineData(1, 1, 1, 1, 2, 1)]
            [InlineData(1, 2, 1, 2, 1, 1)]
            [InlineData(10, 3, 1, 5, 53, 15)]
            [InlineData(-1, -1, -1, 5, 4, 5)]
            [InlineData(-1, -1, 0, 1, 1, 1)]

            public void AdditionOperator(int numerator1, int denominator1,
                int numerator2, int denominator2,
                int expectedNumerator, int expectedDenominator)
            {
                Fraction fraction1 = new Fraction(numerator1, denominator1);
                Fraction fraction2 = new Fraction(numerator2, denominator2);
                Fraction result = fraction1 + fraction2;
                Fraction expectedResult = new Fraction(expectedNumerator, expectedDenominator);
                Assert.Equal(result.Numerator, expectedResult.Numerator);
                Assert.Equal(result.Denominator, expectedResult.Denominator);
            }

            [Theory]
            [InlineData(1, 1)]
            [InlineData(-99, 25)]
            [InlineData(-99, 1)]
            [InlineData(-1, 99)]
            public void Is_Substraction_SameTo_NegativeAddtion(int numerator, int denominator)
            {
                Fraction fraction1 = new Fraction(numerator, denominator);
                Fraction fraction2 = new Fraction(numerator + 1, denominator + 1);
                Fraction substractionResult = fraction1 - fraction2;
                Fraction additionResult = fraction1 + (-fraction2);
                Assert.Equal(substractionResult, additionResult);
            }

            [Fact]
            public void IComparable_Sorting()
            {
                var v2 = new Fraction(1, 3);
                var v1 = new Fraction(1, 2);
                var v6 = new Fraction(2, 3);
                var v5 = new Fraction(3, 2);
                var v3 = new Fraction(5, 1);
                var v4 = new Fraction(6, 1);

                Fraction[] array = new Fraction[]
                {
                    v1, v2, v3, v4, v5, v6
                };

                Array.Sort(array);

                Fraction[] expectedArray = new Fraction[]
                {
                    v2, v1, v6, v5, v3, v4
                };

                for (int i = 0; i < expectedArray.Length; i++)
                {
                    Assert.Equal(expectedArray[i], array[i]);
                }
            }

            [Theory]
            [InlineData(1, 2, 1)]
            [InlineData(4, 2, 2)]
            [InlineData(5, 2, 3)]
            [InlineData(101, 10, 11)]
            public void CeilToInt(int numerator, int denomintator, int rounded)
            {
                Fraction fraction = new Fraction(numerator, denomintator);
                int roundedFraction = fraction.CeilToInt();
                Assert.Equal(rounded, roundedFraction);
            }

            [Theory]
            [InlineData(1, 2, 0)]
            [InlineData(4, 2, 2)]
            [InlineData(5, 2, 2)]
            [InlineData(101, 10, 10)]
            public void FloorToInt(int numerator, int denomintator, int rounded)
            {
                Fraction fraction = new Fraction(numerator, denomintator);
                int roundedFraction = fraction.FloorToInt();
                Assert.Equal(rounded, roundedFraction);
            }
        }
    }
}
