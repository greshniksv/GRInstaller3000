﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GRInstaller3000Classes
{
    public class VariableItem
    {
        public string StatementId { get; set; }
        public string Name { get; set; }
        public object Data { get; set; }
        public Type VarType { get; set; }
    }

    internal class Variables : IDisposable
    {
        private Hashtable _variableList;

        private enum VariableType
        {
            String, Int, Bool, Double, ListString, ListInt
        }

        public string[] GetVariableTypeList()
        {
            return Enum.GetNames(typeof(VariableType));
        }

        public Variables()
        {
            _variableList = new Hashtable();
        }

	    public object GetData(string name)
	    {
			return ((VariableItem)_variableList[name]).Data;
	    }

        public void ClearByStatementId(string id)
        {
            var removeList =
                _variableList.Keys.Cast<object>()
                    .Where(key => ((VariableItem) _variableList[key]).StatementId == id)
                    .ToList();

            foreach (var removeKey in removeList)
            {
                _variableList.Remove(removeKey);
            }
        }

        #region Create Variable
        public void CreateVariable(string data, string statementId)
        {
            if (data.Contains("="))
            {
                // contains initializer
                var dataMass = data.Split(' ', '=');
                var varContains = data.Substring(data.IndexOf('=') + 1, data.Length - (data.IndexOf('=') + 1));
                //if (dataMass.Count() != 3)  throw new Exception("Variable with initializer structure error !");
                object variable = null;
                Type varType = null;

                // example: string boby = "hi"
                if (string.Equals(dataMass[0], "String", StringComparison.CurrentCultureIgnoreCase))
                {
                    variable = varContains.Replace('"', ' ').Trim();
                    varType = typeof (string);
                }

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
                _variableList.Add(dataMass[1], new VariableItem() { StatementId =statementId, Name = dataMass[1], VarType = varType, Data = variable });

            }
            else
            {
                // contains without initializer
                var dataMass = data.Split(' ');
                if (dataMass.Count() != 2) throw new Exception("Variable with initializer structure error !");
                object variable = null;
                Type varType = null;

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
                _variableList.Add(dataMass[1], new VariableItem() { StatementId = statementId, Name = dataMass[1], VarType = varType, Data = variable });
            }
        }

        #endregion

		#region VariablesProperties



		#endregion

        #region Despose patternt
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Implement dispose pattern
        public void Dispose(bool managed)
        {
            if (managed)
            {
                if (_variableList != null)
                {
                    _variableList.Clear();
                    _variableList = null;
                }
            }
        }
        #endregion
    }
}
