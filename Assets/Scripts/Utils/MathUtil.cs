public static class MathUtil
{
    public static int Modulo(int k, int n)
    {
        return ((k %= n) < 0) ? k + n : k;
    }
}
