using System.Diagnostics;


namespace ConsoleApp2
{
    class Program
    {
        static void Main()
        {
            const int itemCount = 100_000;

            var customDict = new MyCustomDictionary<string, int>();
            var systemDict = new Dictionary<string, int>();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < itemCount; i++)
            {
                customDict.Add(i.ToString(), i);
            }
            sw.Stop();
            Console.WriteLine($"MyCustomDictionary Add: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < itemCount; i++)
            {
                systemDict.Add(i.ToString(), i);
            }
            sw.Stop();
            Console.WriteLine($".NET Dictionary Add: {sw.ElapsedMilliseconds} ms");

            Console.WriteLine("Benchmark complete.");
        }
    }
}

    