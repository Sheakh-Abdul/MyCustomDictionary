using MyDictionaryProject;
using System.Diagnostics;


namespace ConsoleApp2
{
    class Program
    {
        static void Main()
        { 
            LongestSum sum = new LongestSum();
            Console.WriteLine(sum.LongestSubStringWithRepeat("AABABBA", 1));
            //var res = sum.FindAnagrams("cbaebabacd", "abc");
            //foreach (var item in res)
            //{
            //    Console.WriteLine(item);
            //}
        }
    }
}

    