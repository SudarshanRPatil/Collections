using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dict = new MyDict<int, string>();

            for (int i = 0; i < 10; i++)
            {
                dict.Add(i+1, string.Format("Sud {0}", i+1));
            }

            foreach (var valuePair in dict)
            {
               Console.WriteLine("Key {0} and Value {1}",valuePair.Key, valuePair.Value);
            }

            Console.ReadKey();
        }
    }
}
