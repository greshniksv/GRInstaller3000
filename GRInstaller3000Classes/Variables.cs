using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace GRInstaller3000Classes
{
    public class VariableItem
    {
        public VariableItem(object ob, string name)
        {
            Name = name;
            Data.Set(ob, name);
        }

        public string StatementId { get; set; }
        public string Name { get; set; }

        #region Class contain variable
        static private class Data
        {
            public static string _String { get; set; }
            public static int? _Int { get; set; }
            public static double? _Double { get; set; }
            public static bool? _Bool { get; set; }
            public static byte? _Byte { get; set; }
            public static char? _Char { get; set; }

            public static string[] _StringMas { get; set; }
            public static int[] _IntMas { get; set; }
            public static double[] _DoubleMas { get; set; }
            public static bool[] _BoolMas { get; set; }
            public static byte[] _ByteMas { get; set; }
            public static char[] _CharMas { get; set; }

            public static List<string> _StringList { get; set; }
            public static List<int> _IntList { get; set; }
            public static List<double> _DoubleList { get; set; }
            public static List<bool> _BoolList { get; set; }
            public static List<byte> _ByteList { get; set; }
            public static List<char> _CharList { get; set; }

            public static void Set(object ob, string name)
            {
                switch (ob.GetType().Name)
                {
                    case "String": _String = (string)ob; break;
                    case "Boolean": _Bool = (bool?) ob; break;
                    case "Double": _Double = (double?)ob;break;
                    case "Int32": _Int = (int?)ob; break;
                    case "Char": _Char = (char?)ob; break;
                    case "Byte": _Byte = (byte?)ob; break;

                    case "String[]": _StringMas = (string[])ob; break;
                    case "Boolean[]": _BoolMas = (bool[])ob; break;
                    case "Double[]": _DoubleMas = (double[])ob; break;
                    case "Int32[]": _IntMas = (int[])ob; break;
                    case "Char[]": _CharMas = (char[])ob; break;
                    case "Byte[]": _ByteMas = (byte[])ob; break;
                       
                    default: throw new Exception("Not found variable type! Name:"+name);
                }

                /*
                 +		ob.GetType()	
{Name = "List`1" 
FullName = "System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"}	System.Type {System.RuntimeType}


+		i.GetType()	
{Name = "List`1" 
FullName = "System.Collections.Generic.List`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"}	System.Type {System.RuntimeType}

                 */


            }

            public static object GetData()
            {
                object r = null;

                if (_Bool != null)      { r = _Bool; return r; }
                if (_String != null)    { r = _String; return r; }
                if (_Int != null)       { r = _Int; return r; }
                if (_Double != null)    { r = _Double; return r; }
                if (_Byte != null)      { r = _Byte; return r; }
                if (_Char != null)      { r = _Char; return r; }
                if (_StringMas != null) { r = _StringMas; return r; }
                if (_IntMas != null)    { r = _IntMas; return r; }
                if (_DoubleMas != null) { r = _DoubleMas; return r; }
                if (_BoolMas != null)   { r = _BoolMas; return r; }
                if (_ByteMas != null)   { r = _ByteMas; return r; }
                if (_CharMas != null)   { r = _CharMas; return r; }
                if (_StringList != null) { r = _StringList; return r; }
                if (_IntList != null)   { r = _IntList; return r; }
                if (_DoubleList != null) { r = _DoubleList; return r; }
                if (_BoolList != null)  { r = _BoolList; return r; }
                if (_ByteList != null)  { r = _ByteList; return r; }
                if (_CharList != null)  { r = _CharList; return r; }

                return null;
            }

        }
        #endregion

        public override string ToString()
        {
            return Data.GetData().ToString();
        }
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
			return ((VariableItem)_variableList[name]);
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
                var dataMas = data.Split(' ', '=');
                var dataList = new List<string>(dataMas);
                dataList.RemoveAll(i => i.Trim().Length < 1);

                var varContainer = data.Substring(data.IndexOf('=') + 1, data.Length - (data.IndexOf('=') + 1));
                //if (dataMass.Count() != 3)  throw new Exception("Variable with initializer structure error !");
                object variable = null;

                // example: string boby = "hi"
                if (string.Equals(dataList[0], "String", StringComparison.CurrentCultureIgnoreCase))
                    variable = varContainer.Replace('"', ' ').Trim();

                // example: int boby = 10
                if (string.Equals(dataList[0], "Int", StringComparison.CurrentCultureIgnoreCase))
                    variable = int.Parse(varContainer);

                // example: bool boby = false
                if (string.Equals(dataList[0], "Bool", StringComparison.CurrentCultureIgnoreCase))
                    variable = Boolean.Parse(varContainer);

                // example: double boby = 1.25
                if (string.Equals(dataList[0], "Double", StringComparison.CurrentCultureIgnoreCase))
                    variable = Double.Parse(varContainer, NumberStyles.Any);

                // Example: ListString Boby = {"1","1","1","1","1"}
                if (string.Equals(dataList[0], "ListString", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!varContainer.Contains('{') || !varContainer.Contains('}'))
                        throw new Exception("ListString initializer incorrect !");
                    variable = varContainer.Replace('{', ' ').Replace('}', ' ').Replace('"', ' ').Trim().Split(',').ToList();
                }

                // Example: ListString Boby = {1,1,1,1,1}
                if (string.Equals(dataList[0], "ListInt", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!varContainer.Contains('{') || !varContainer.Contains('}'))
                        throw new Exception("ListInt initializer incorrect !");
                    var buf2 = varContainer.Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
                    variable = buf2.Select(int.Parse).ToList();
                }

                //dataMass: 0 - type, 1 - name, 2 - data
                _variableList.Add(dataList[1], new VariableItem(variable, dataList[1]) { StatementId = statementId });
                //_variableList.Add(dataMass[1], new VariableItem() { StatementId =statementId, Name = dataMass[1], Data = variable });

            }
            else
            {
                // contains without initializer
                //var dataMass = data.Split(' ');
                var dataMas = data.Split(' ', '=');
                var dataList = new List<string>(dataMas);
                dataList.RemoveAll(i => i.Trim().Length < 1);

                if (dataList.Count() != 2) throw new Exception("Variable with initializer structure error !");
                object variable = null;
                Type varType = null;

                // example: string boby
                if (string.Equals(dataList[0], "String", StringComparison.CurrentCultureIgnoreCase))
                    variable = string.Empty;

                // example: int boby
                if (string.Equals(dataList[0], "Int", StringComparison.CurrentCultureIgnoreCase))
                    variable = new int();

                // example: bool boby
                if (string.Equals(dataList[0], "Bool", StringComparison.CurrentCultureIgnoreCase))
                    variable = new bool();

                // example: double boby
                if (string.Equals(dataList[0], "Double", StringComparison.CurrentCultureIgnoreCase))
                    variable = new double();

                // Example: ListString Boby
                if (string.Equals(dataList[0], "ListString", StringComparison.CurrentCultureIgnoreCase))
                    variable = new List<string>();

                // Example: ListString Boby
                if (string.Equals(dataList[0], "ListInt", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!dataList[3].Contains('{') || !dataList[3].Contains('}'))
                        throw new Exception("ListInt initializer incorrect !");
                    var buf2 = dataList[3].Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
                    variable = buf2.Select(int.Parse).ToList();
                }

                //dataMass: 0 - type, 1 - name, 2 - data
                _variableList.Add(dataList[1], new VariableItem(variable, dataList[1]) { StatementId = statementId });
                //_variableList.Add(dataMass[1], new VariableItem() { StatementId = statementId, Name = dataMass[1], VarType = varType, Data = variable });
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
