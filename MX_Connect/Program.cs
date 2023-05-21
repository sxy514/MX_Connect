using System;
using System.Collections.Generic;
using System.Threading;
using ActUtlTypeLib;

namespace MX_Connect
{
    internal class Program
    {
        private static ActUtlType  plc = new  ActUtlType();
        
       
        private static int z1Pos, z2Pos, t1Pos, y1Pos;
        static Dictionary<int, string> axisPos = new Dictionary<int, string>();
        private static int[] axisPosArray = new int[7];
       
        static void Connect(int i)
        {
            plc.ActLogicalStationNumber = i;
            var returnCode = plc.Open();
            Console.WriteLine(returnCode == 0 ? "success" : "fail");
        }
        
        static int GetStatus()
        {
            plc.GetDevice(axisPos[1], out int status);
            return status;
        }


        public static void Main(string[] args)
        {
            #region  折叠内容
            // 现在让我们整理一下思路：
            // 1.连接PLC后，如果我们想要清除PIO报警，需要先判断各个轴的位置，全部在原点后，再清除PIO报警
            // 现在，读取各个轴的位置Z1:D4420 Z2:D4620 T1:D4820 Y1:D5020 


            /*     plc.GetDevice("D4420", out z1Pos);
                   plc.GetDevice("D4620", out z2Pos);
                   plc.GetDevice("D4820", out t1Pos);
                   plc.GetDevice("D5020", out y1Pos);
                   Console.WriteLine("Z1 Position: " + z1Pos);
                   Console.WriteLine("Z2 Position: " + z2Pos);
                   Console.WriteLine("T1 Position: " + t1Pos);
                   Console.WriteLine("Y1 Position: " + y1Pos);

                   if(z1Pos != 0)Console.WriteLine("Z1 is not at home position");
                   if(z2Pos != 0)Console.WriteLine("Z2 is not at home position");
                   if(t1Pos != 0)Console.WriteLine("T1 is not at home position");
                   if(y1Pos != 0)Console.WriteLine("Y1 is not at home position");

                   plc.SetDevice("M37", 1);
                   Thread.Sleep(1000);
                   plc.SetDevice("M37", 0);
                   plc.SetDevice("M7790", 1);
                   Thread.Sleep(1000);
                   plc.SetDevice("M7790", 0);
                            
            */
            #endregion
            // 在字典中放入元素
           axisPos.Add(1, "M37");
            // 使用字典
            // Console.WriteLine(axisPos[1]);

            // 连接
            Connect(1);
            plc.SetDevice("D4420", 0);
            plc.SetDevice(axisPos[1], 1);

            int z1HomePos;
            plc.GetDevice("M9050", out z1HomePos);
            // getStatus()
            Console.WriteLine(GetStatus());
            Console.WriteLine(z1HomePos);

            plc.ReadDeviceBlock("M240" , 7 , out axisPosArray[0]);
            for (int i = 0; i < axisPosArray.Length; i++)
            {
                //Console.WriteLine("这是批量读取的： "+axisPosArray[i]);
                if (axisPosArray[i] != 0)
                {
                    Console.WriteLine("这个轴不在原点：" + axisPosArray[i]);
                    //break;
                }
            }


        }

    }
}