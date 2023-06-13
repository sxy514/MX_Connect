using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace SQLTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
             var nSubUnitState = new Dictionary<string, string>();
             var connstr =
                 "data source=localhost;database=tcsv2;user id=root;password=808999;pooling=false;charset=utf8";
             /* using (var conn = new MySqlConnection(connstr))
              {
                  var Station = "MCT112_V";
                  //var sql = $"use tcsv2;select ID,Substate from tcsunit where ID like '%{Station}%'";
                  var sql = $"select ID,Substate from tcsunit where ID like '%{Station}%'";
                  var cmd = new MySqlCommand(sql, conn);
                  conn.Open();
                  var reader = cmd.ExecuteReader();
 
                  while (reader.Read()) nSubUnitState.Add(reader.GetString("ID"), reader.GetString("SubState"));
                  conn.Close();
              }*/

            using (var conn = new MySqlConnection(connstr))
            {
                var Station = "MCT113";
                var sql = string.Format("select ID,State from tcsunit where ID like '%{0}%'", Station);
                var cmd = new MySqlCommand(sql, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    nSubUnitState.Add(reader.GetString("ID"), reader.GetString("State"));
                conn.Close();
            }


            //  输出字典    
            foreach (var item in nSubUnitState) Console.WriteLine(item.Key + " " + item.Value);

            Console.ReadKey();
        }
    }
}