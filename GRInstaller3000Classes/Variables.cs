using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GRInstaller3000Classes
{
    internal class Variables
    {
        private readonly Hashtable _variableList;

        private enum VariableType
        {
            String, Int, Bool, Double, ListString, ListInt
        }

        class VariableItem
        {
            public string Name { get; set; }
            public object Data { get; set; }
            public VariableType Type { get; set; }
        }

        public string[] GetVariableTypeList()
        {
            return Enum.GetNames(typeof(VariableType));
        }

        public Variables()
        {
            _variableList = new Hashtable();
        }

        #region Create Variable
        public void CreateVariable(string data)
        {
            if (data.Contains("="))
            {
                // contains initializer
                var dataMass = data.Split(' ', '=');
                var varContains = data.Substring(data.IndexOf('=') + 1, data.Length - (data.IndexOf('=') + 1));
                //if (dataMass.Count() != 3)  throw new Exception("Variable with initializer structure error !");
                object variable = null;

                // example: string boby = "hi"
                if (string.Equals(dataMass[0], "String", StringComparison.CurrentCultureIgnoreCase))
                    variable = varContains.Replace('"', ' ').Trim();

                // example: int boby = 10
                if (string.Equals(dataMass[0], "Int", StringComparison.CurrentCultureIgnoreCase))
                    variable = int.Parse(varContains);

                // example: bool boby = false
                if (string.Equals(dataMass[0], "Bool", StringComparison.CurrentCultureIgnoreCase))
                    variable = Boolean.Parse(varContains);

                // example: double boby = 1.25
                if (string.Equals(dataMass[0], "Double", StringComparison.CurrentCultureIgnoreCase))
                    variable = Double.Parse(varContains, NumberStyles.Any);

                // Example: ListString Boby = {"1","1","1","1","1"}
                if (string.Equals(dataMass[0], "ListString", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!varContains.Contains('{') || !dataMass[3].Contains('}'))
                        throw new Exception("ListString initializer incorrect !");
                    variable = varContains.Replace('{', ' ').Replace('}', ' ').Replace('"', ' ').Trim().Split(',').ToList();
                }

                // Example: ListString Boby = {1,1,1,1,1}
                if (string.Equals(dataMass[0], "ListInt", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!varContains.Contains('{') || !dataMass[3].Contains('}'))
                        throw new Exception("ListInt initializer incorrect !");
                    var buf2 = varContains.Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
                    variable = buf2.Select(int.Parse).ToList();
                }

                //dataMass: 0 - type, 1 - name, 2 - data
                _variableList.Add(dataMass[1], new VariableItem() { Name = dataMass[1], Type = (VariableType)Enum.Parse(typeof(VariableType), dataMass[0], true), Data = variable });

            }
            else
            {
                // contains without initializer
                var dataMass = data.Split(' ');
                if (dataMass.Count() != 2) throw new Exception("Variable with initializer structure error !");
                object variable = null;

                // example: string boby
                if (string.Equals(dataMass[0], "String", StringComparison.CurrentCultureIgnoreCase))
                    variable = string.Empty;

                // example: int boby
                if (string.Equals(dataMass[0], "Int", StringComparison.CurrentCultureIgnoreCase))
                    variable = new int();

                // example: bool boby
                if (string.Equals(dataMass[0], "Bool", StringComparison.CurrentCultureIgnoreCase))
                    variable = new bool();

                // example: double boby
                if (string.Equals(dataMass[0], "Double", StringComparison.CurrentCultureIgnoreCase))
                    variable = new double();

                // Example: ListString Boby
                if (string.Equals(dataMass[0], "ListString", StringComparison.CurrentCultureIgnoreCase))
                    variable = new List<string>();

                // Example: ListString Boby
                if (string.Equals(dataMass[0], "ListInt", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!dataMass[3].Contains('{') || !dataMass[3].Contains('}'))
                        throw new Exception("ListInt initializer incorrect !");
                    var buf2 = dataMass[3].Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
                    variable = buf2.Select(int.Parse).ToList();
                }

                //dataMass: 0 - type, 1 - name, 2 - data
                _variableList.Add(dataMass[1], new VariableItem() { Name = dataMass[1], Type = (VariableType)Enum.Parse(typeof(VariableType), dataMass[0], true), Data = variable });
            }
        }

        #endregion

    }
}
