using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MX_Form
{
    internal class DBoperator
    {
        private int logicNumber = 0;    // PLC编号

        private bool IsUnitActive()
        {
            var SubunitState = new Dictionary<string, string>();
            var Result = false;
            SubunitState = GetSubUnitState();
            foreach (var i in SubunitState.Keys)
                if (SubunitState[i] != "NOTASSIGN")
                {
                    Result = true;
                    return Result;
                }

            return Result;
        }

        private bool IsUnitRun()
        {
            var unitState = new Dictionary<string, string>();
            var Result = true;
            unitState = GetUnitState();
            foreach (var i in unitState.Keys)
                if (!(unitState[i] == "RUN") || unitState[i] == "RESERVED") // 有一个不是RUN或RESERVED就返回false
                {
                    Result = false;
                    return Result;
                }

            return Result;
        }

        private string MessageFormat()
        {
            var unitState = new Dictionary<string, string>();
            var Message = string.Empty;
            unitState = GetUnitState();
            foreach (var key in unitState.Keys)
                if (!(unitState[key] == "RUN") || unitState[key] == "RESERVED")
                    Message += string.Format("The State Of {0} is {1}!", key, unitState[key]);
            return Message;
        }

        private Dictionary<string, string> GetSubUnitState()
        {
            var nSubUnitState = new Dictionary<string, string>();
            var connstr =
                "data source=localhost;database=tcsv2;user id=root;password=808999;pooling=false;charset=utf8";
            using (var conn = new MySqlConnection(connstr))
            {
                var Station = "MCT" + (111 + logicNumber) + "_V";
                var sql = $"select ID,Substate from tcsunit where ID like '%{Station}%'";
                var cmd = new MySqlCommand(sql, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    nSubUnitState.Add(reader.GetString("ID"), reader.GetString("SubState"));
                conn.Close();
            }

            return nSubUnitState;
        }

        private Dictionary<string, string> GetUnitState()
        {
            var nUnitState = new Dictionary<string, string>();
            var connstr =
                "data source=localhost;database=tcsv2;user id=root;password=808999;pooling=false;charset=utf8";
            using (var conn = new MySqlConnection(connstr))
            {
                var Station = "MCT" + (111 + logicNumber);
                var sql = string.Format("select ID,State from tcsunit where ID like '%{0}%'", Station);
                var cmd = new MySqlCommand(sql, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    nUnitState.Add(reader.GetString("ID"), reader.GetString("State"));
                conn.Close();
            }

            return nUnitState;
        }

    }
}
