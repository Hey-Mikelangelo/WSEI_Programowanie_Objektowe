using lab_1;
using Xunit;

namespace lab_1_test
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(-1, 5, 1)]
        [InlineData(5, -1, 1)]
        [InlineData(15, 6, 3)]
        [InlineData(10, 5, 5)]

        public void CorrectGDC_NormalValues_ReturnTrue(int a, int b, int correctResult)
        {
            int result =  MathHelpers.GetGreatestCommonDivisor(a, b);
            Assert.Equal(correctResult, result);
        }
    }
}
