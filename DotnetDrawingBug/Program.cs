namespace DotnetDrawingBug
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            using var bitmap = new MyBitmap();
            var hash = bitmap.GetHash();
            Console.WriteLine($"{hash:X16}");

            return 0;
        }
    }
}
