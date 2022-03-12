using lab_1;
using System;
using Xunit;

namespace lab_1_test
{
    public class TestsMathHelpers
    {
        public class TestsGetGDC
        {
            [Theory]
            [InlineData(15, 6, 3)]
            [InlineData(10, 5, 5)]
            [InlineData(10, 3, 1)]
            [InlineData(3, 10, 1)]
            public void GetGDC_PositiveValues_ReturnTrue(int a, int b, int correctResult)
            {
                int result = MathHelpers.GetGreatestCommonDivisor(a, b);
                Assert.Equal(correctResult, result);
            }

            [Theory]
            [InlineData(15, -6, 3)]
            [InlineData(10, -5, 5)]
            [InlineData(10, -3, 1)]
            [InlineData(-10, 3, 1)]
            public void GetGDC_PositiveAndNegativeValues_ReturnTrue(int a, int b, int correctResult)
            {
                int result = MathHelpers.GetGreatestCommonDivisor(a, b);
                Assert.Equal(correctResult, result);
            }

            [Theory]
            [InlineData(-15, -6, 3)]
            [InlineData(-10, -5, 5)]
            [InlineData(-10, -3, 1)]
            [InlineData(-3, -3, 3)]
            public void GetGDC_NegativeValues_ReturnTrue(int a, int b, int correctResult)
            {
                int result = MathHelpers.GetGreatestCommonDivisor(a, b);
                Assert.Equal(correctResult, result);
            }

            [Theory]
            [InlineData(1, 0)]
            [InlineData(0, 1)]
            [InlineData(0, -1)]
            [InlineData(-1, 0)]
            public void GetGDC_WithZero_ThrowsExeption(int a, int b)
            {
                Exception exception = Record.Exception(() =>
                {
                    MathHelpers.GetGreatestCommonDivisor(a, b);
                });
                Assert.NotNull(exception);
            }
        }        
    }
}
