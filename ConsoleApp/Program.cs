using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using HashTable;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();

            var sr = new StreamReader("WarAndWorld.txt");
            Dictionary<string, int> Frequencies = new Dictionary<string, int>();

            string[] words = sr.ReadToEnd().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //docs.microsoft.com/en-us/previous-versions/ms379571(v=vs.80)?redirectedfrom=MSDN#datastructures20_2_topic6

            Init();
            double startFillFactor, endFillFactor;
            int countTests;

            Console.WriteLine("> Process started...");
            Console.Write("Use optimal settings? (y/n):\t");

            switch (Console.ReadLine()) {
                case "n":
                    Console.Write("Enter the start FillFactor (,):\t");
                    startFillFactor = double.Parse(Console.ReadLine());
                    Console.Write("Enter the end FillFactor (,):\t");
                    endFillFactor = double.Parse(Console.ReadLine());
                    Console.Write("Enter the count of tests (int):\t");
                    countTests = int.Parse(Console.ReadLine());
                    break;
                case "y":
                    startFillFactor = 0.55; //optimal: 0.77; microsoft: 0.72
                    endFillFactor = 0.6;
                    countTests = 3;
                    break;
                default:
                    Console.ReadKey();
                    return;
            }

            Console.WriteLine("*******************************************");
            double mediumTimeOfHashTableWork = 0;
            double timeOfDictionaryWork = 0;

            int iterationsCount = 0;
            for (double i = startFillFactor; i <= endFillFactor; i += 0.01, iterationsCount++) {
                Console.Write("FillFactor: {0:0.00}", i);
                double testMediumTimeOfHashTableWork = 0;
                stopWatch.Restart();
                TestHashTable(words, i);
                stopWatch.Stop();
                Console.Write("\t HASH: {0}", testMediumTimeOfHashTableWork = stopWatch.ElapsedMilliseconds);
                for (int j = 0; j < countTests - 1; j++) {
                    stopWatch.Restart();
                    TestHashTable(words, i);
                    stopWatch.Stop();
                    Console.Write("  {0}", stopWatch.ElapsedMilliseconds);
                    testMediumTimeOfHashTableWork += stopWatch.ElapsedMilliseconds;
                }
                testMediumTimeOfHashTableWork /= countTests;
                Console.WriteLine("\n\t\t\t Medium Time: {0:0.0}", testMediumTimeOfHashTableWork);
                mediumTimeOfHashTableWork += stopWatch.ElapsedMilliseconds;
            }
            mediumTimeOfHashTableWork /= iterationsCount;
            Console.WriteLine("\n\t\t\t Medium Time: {0:0.0}", mediumTimeOfHashTableWork);
            Console.WriteLine("*******************************************");
            stopWatch.Restart();
            TestDictionary(words);
            stopWatch.Stop();
            Console.WriteLine("\t\t\t DICT: {0}", timeOfDictionaryWork = stopWatch.ElapsedMilliseconds);
            Console.WriteLine("*******************************************");
            Console.WriteLine("Difference ratio: {0:0.0000}", mediumTimeOfHashTableWork / timeOfDictionaryWork);
            //Console.WriteLine(hashTable.Capacity);

            Console.ReadKey();
        }

        public static void TestHashTable(string[] words, double FillFactor)
        {
            var hashTable = new OpenAddressHashTable<string, int>(FillFactor);

            foreach (var item in words) {
                if (hashTable.ContainsKey(item))
                    hashTable[item]++;
                else
                    hashTable.Add(item, 1);
            }

            var WordsWithFreqMore27 = new List<string>(hashTable.Where(word => word.Value > 27).Select(word => word.Key));

            foreach (var word in WordsWithFreqMore27) {
                hashTable.Remove(word);
            }
        }

        public static void TestDictionary(string[] words)
        {
            var dictionary = new Dictionary<string, int>();

            foreach (var item in words)
                if (dictionary.ContainsKey(item))
                    dictionary[item]++;
                else
                    dictionary.Add(item, 1);

            var WordsWithFreqMore27 = new List<string>(dictionary.Where(word => word.Value > 27).Select(word => word.Key));

            foreach (var word in WordsWithFreqMore27) {
                dictionary.Remove(word);
            }
        }

        public static void Init()
        {
            OpenAddressHashTable<int, int> hashTable = new OpenAddressHashTable<int, int>();
            Dictionary<int, int> dict = new Dictionary<int, int>();

            for (int i = 0; i < 10000; i++) {
                hashTable.Add(i, i);
                dict.Add(i, i);
            }
        }
    }
}
