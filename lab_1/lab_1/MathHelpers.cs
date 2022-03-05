namespace lab_1
{
    public static class MathHelpers
    {
        public static int GetGreatestCommonDivisor(int a, int b)
        {
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
                a1 = 1;
            }
            return a1;
        }
    }
}
