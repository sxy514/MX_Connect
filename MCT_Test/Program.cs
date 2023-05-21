using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActUtlTypeLib;

namespace MCT_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ActUtlType plc = new ActUtlType();

            plc.ActLogicalStationNumber = 1;

            int ReturnCode= plc.Open();
            if (ReturnCode == 0)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine(ReturnCode);
            }

            int[] IoNum = new int[8];

            int mTest;
           plc.GetDevice("M241",out mTest);
           Console.WriteLine(mTest);



        }
    }
}
