namespace lab_1
{
    public static class MathHelpers
    {
        /// <summary>
        /// Calculates greatest common divisor using non recursive Euclid's algorithm
        /// </summary>
        /// <exception cref="System.DivideByZeroException"></exception>
        public static int GetGreatestCommonDivisor(int a, int b)
        {
            if(a == 0 || b == 0)
            {
                throw new System.DivideByZeroException();
            }
            if (a < b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            int a1 = a, b1 = b;
            do
            {
                int a1Prev = a1;
                a1 = b1;
                b1 = a1Prev % b1;
            }
            while (b1 != 0);
            if(a1 < 0)
            {
                a1 = -a1;
            }
            return a1;
        }
    }
}
