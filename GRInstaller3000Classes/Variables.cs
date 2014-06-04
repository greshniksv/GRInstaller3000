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
	public enum VariableType
	{
		String, Int, Bool, Double, Char, Byte, 
		ListString, ListInt, ListBool, ListDouble, ListByte, ListChar
	}

	public class VariableItem
	{
		public VariableItem(object ob, string name)
		{
			Name = name;
			Set(ob, name);
		}

		public VariableItem(object ob)
		{
			Name = Guid.NewGuid().ToString();
			Set(ob, Name);
		}

		public VariableItem(string var)
		{

			// if true|false bool
			if (var == "true" || var == "false")
			{
				bool b = (var == "true");
				Set(b, Guid.NewGuid().ToString());
			}

			// if ' - char
			if (var.Contains("'"))
			{
				char d;
				if (char.TryParse(var.Replace("'","").Trim(), out d))
				{
					Set(d, Guid.NewGuid().ToString());
				}
			}

			// if " - string
			if (var.Contains("\""))
			{
				Set(var.Replace("\"","").Trim(), Guid.NewGuid().ToString());
			}

			// if . and digits - double
			if (var.Contains("."))
			{
				if (var.Replace('.', '0').All(Char.IsDigit))
				{
					double d;
					if (double.TryParse(var, out d))
					{
						Set(d, Guid.NewGuid().ToString());
					}
				}
			}

			// byte
			if (var.All(Char.IsDigit))
			{
				byte b;
				if (byte.TryParse(var, out b))
				{
					Set(b, Guid.NewGuid().ToString());
				}
			}

			// int
			if (var.All(Char.IsDigit))
			{
				Int32 i;
				if (Int32.TryParse(var,out i))
				{
					Set(i, Guid.NewGuid().ToString());
				}
			}

		}

		public string StatementId { get; set; }
		public string Name { get; set; }
		public VariableType Type { get; set; }

		#region Emplements variable.

		public string _String { get; set; }
		public int? _Int { get; set; }
		public double? _Double { get; set; }
		public bool? _Bool { get; set; }
		public byte? _Byte { get; set; }
		public char? _Char { get; set; }

		public List<string> _StringList { get; set; }
		public List<int> _IntList { get; set; }
		public List<double> _DoubleList { get; set; }
		public List<bool> _BoolList { get; set; }
		public List<byte> _ByteList { get; set; }
		public List<char> _CharList { get; set; }

		public void Set(object ob, string name)
		{
			var typeSplited = new List<string>(ob.GetType().AssemblyQualifiedName.Split('[', ','));
			typeSplited.RemoveAll(i => i.Trim().Length < 1);

			if (!ob.GetType().Name.Contains("List"))
			{
				switch (ob.GetType().Name)
				{
					case "String":
						_String = (string)ob; Type = VariableType.String;
						break;
					case "Boolean":
						_Bool = (bool?)ob;Type = VariableType.Bool;
						break;
					case "Double":
						_Double = (double?)ob;Type = VariableType.Double;
						break;
					case "Int32":
						_Int = (int?)ob;Type = VariableType.Int;
						break;
					case "Char":
						_Char = (char?)ob;Type = VariableType.Char;
						break;
					case "Byte":
						_Byte = (byte?)ob;Type = VariableType.Byte;
						break;

					default:
						throw new Exception("Not found variable type! Name:" + name);
				}
			}
			else
			{
				switch (typeSplited[1])
				{
					case "System.String":
						_StringList = (List<string>)ob;Type = VariableType.ListString;
						break;
					case "System.Boolean":
						_BoolList = (List<bool>)ob;Type = VariableType.ListBool;
						break;
					case "System.Double":
						_DoubleList = (List<double>)ob;Type = VariableType.ListDouble;
						break;
					case "System.Int32":
						_IntList = (List<int>)ob;Type = VariableType.ListInt;
						break;
					case "System.Char":
						_CharList = (List<char>)ob;Type = VariableType.ListChar;
						break;
					case "System.Byte":
						_ByteList = (List<byte>)ob;Type = VariableType.Byte;
						break;
				}
			}

		}

		public object GetData()
		{
			object r = null;

			if (_Bool != null)
			{
				r = _Bool;
				return r;
			}
			if (_String != null)
			{
				r = _String;
				return r;
			}
			if (_Int != null)
			{
				r = _Int;
				return r;
			}
			if (_Double != null)
			{
				r = _Double;
				return r;
			}
			if (_Byte != null)
			{
				r = _Byte;
				return r;
			}
			if (_Char != null)
			{
				r = _Char;
				return r;
			}

			if (_StringList != null)
			{
				r = _StringList;
				return r;
			}
			if (_IntList != null)
			{
				r = _IntList;
				return r;
			}
			if (_DoubleList != null)
			{
				r = _DoubleList;
				return r;
			}
			if (_BoolList != null)
			{
				r = _BoolList;
				return r;
			}
			if (_ByteList != null)
			{
				r = _ByteList;
				return r;
			}
			if (_CharList != null)
			{
				r = _CharList;
				return r;
			}

			return null;
		}


		#endregion

		#region Overload perators

		public static bool operator &(VariableItem a, VariableItem b)
		{
			if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			if (a.Type == VariableType.Bool)
			{
				return (a._Bool==true && b._Bool==true);
			}
			else
			{
				throw new Exception("Types can not be compared. Variable: [" + a.Name + "] - [" + b.Name + "]");
			}

			return false;
		}


		public static bool operator |(VariableItem a, VariableItem b)
		{
			if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			if (a.Type == VariableType.Bool)
			{
				return (a._Bool == true || b._Bool == true);
			}
			else
			{
				throw new Exception("Types can not be compared. Variable: [" + a.Name + "] - [" + b.Name + "]");
			}

			return false;
		}



		public static bool operator >(VariableItem a, VariableItem b)
		{
			if(a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: ["+a.Name+"] - ["+b.Name+"]");

			if (a.Type == VariableType.Int || a.Type == VariableType.Double || a.Type == VariableType.Byte ||
			    a.Type == VariableType.Char)
			{
				switch (a.Type)
				{
					case VariableType.Int:
						return a._Int > b._Int;
					case VariableType.Double:
						return a._Double > b._Double;
					case VariableType.Byte:
						return a._Byte > b._Byte;
					case VariableType.Char:
						return a._Char > b._Char;
				}
			}
			else
			{
				throw new Exception("Types can not be compared. Variable: [" + a.Name + "] - [" + b.Name + "]");
			}

			return false;
		}

		public static bool operator <(VariableItem a, VariableItem b)
		{
			if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			if (a.Type == VariableType.Int || a.Type == VariableType.Double || a.Type == VariableType.Byte ||
				a.Type == VariableType.Char)
			{
				switch (a.Type)
				{
					case VariableType.Int:
						return a._Int < b._Int;
					case VariableType.Double:
						return a._Double < b._Double;
					case VariableType.Byte:
						return a._Byte < b._Byte;
					case VariableType.Char:
						return a._Char < b._Char;
				}
			}
			else
			{
				throw new Exception("Types can not be compared. Variable: [" + a.Name + "] - [" + b.Name + "]");
			}

			return false;
		}

        public static bool operator >=(VariableItem a, VariableItem b)
        {
            if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

            if (a.Type == VariableType.Int || a.Type == VariableType.Double || a.Type == VariableType.Byte ||
                a.Type == VariableType.Char)
            {
                switch (a.Type)
                {
                    case VariableType.Int:
                        return a._Int >= b._Int;
                    case VariableType.Double:
                        return a._Double >= b._Double;
                    case VariableType.Byte:
                        return a._Byte >= b._Byte;
                    case VariableType.Char:
                        return a._Char >= b._Char;
                }
            }
            else
            {
                throw new Exception("Types can not be compared. Variable: [" + a.Name + "] - [" + b.Name + "]");
            }

            return false;
        }

        public static bool operator <=(VariableItem a, VariableItem b)
        {
            if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

            if (a.Type == VariableType.Int || a.Type == VariableType.Double || a.Type == VariableType.Byte ||
                a.Type == VariableType.Char)
            {
                switch (a.Type)
                {
                    case VariableType.Int:
                        return a._Int <= b._Int;
                    case VariableType.Double:
                        return a._Double <= b._Double;
                    case VariableType.Byte:
                        return a._Byte <= b._Byte;
                    case VariableType.Char:
                        return a._Char <= b._Char;
                }
            }
            else
            {
                throw new Exception("Types can not be compared. Variable: [" + a.Name + "] - [" + b.Name + "]");
            }

            return false;
        }

		public static bool operator ==(VariableItem a, VariableItem b)
		{
			if (((object)a) == null || ((object)b) == null)
			{
				return (((object)a) == null && ((object)b) == null);
			}

			if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			switch (a.Type)
			{
				case VariableType.Int:
					return a._Int == b._Int;
				case VariableType.Double:
					return a._Double == b._Double;
				case VariableType.Byte:
					return a._Byte == b._Byte;
				case VariableType.Char:
					return a._Char == b._Char;
				case VariableType.String:
					return a._String == b._String;
				case VariableType.Bool:
					return a._Bool == b._Bool;
			}

			return false;
		}

		public static bool operator !=(VariableItem a, VariableItem b)
		{
			if (((object)a) == null || ((object)b) == null)
			{
				return !(((object)a) == null && ((object)b) == null);
			}

			if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			switch (a.Type)
			{
				case VariableType.Int:
					return a._Int != b._Int;
				case VariableType.Double:
					return a._Double != b._Double;
				case VariableType.Byte:
					return a._Byte != b._Byte;
				case VariableType.Char:
					return a._Char != b._Char;
				case VariableType.String:
					return a._String != b._String;
				case VariableType.Bool:
					return a._Bool != b._Bool;
			}

			return false;
		}


		public static VariableItem operator +(VariableItem a, VariableItem b)
		{
			if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			if (a.Type == VariableType.Int || a.Type == VariableType.Double || a.Type == VariableType.Byte ||
				a.Type == VariableType.String)
			{
				switch (a.Type)
				{
					case VariableType.Int:
						return new VariableItem(a._Int + b._Int);
					case VariableType.Double:
						return new VariableItem(a._Double + b._Double);
					case VariableType.Byte:
						return new VariableItem(a._Byte + b._Byte);
					case VariableType.String:
						return new VariableItem(a._String + b._String);
				}
			}

			throw new Exception("Unary operation (+) return null !");
		}

		public static VariableItem operator -(VariableItem a, VariableItem b)
		{
			if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			if (a.Type == VariableType.Int || a.Type == VariableType.Double || a.Type == VariableType.Byte)
			{
				switch (a.Type)
				{
					case VariableType.Int:
						return new VariableItem(a._Int - b._Int);
					case VariableType.Double:
						return new VariableItem(a._Double - b._Double);
					case VariableType.Byte:
						return new VariableItem(a._Byte- b._Byte);
				}
			}

			throw new Exception("Unary operation (+) return null !");
		}

		public static VariableItem operator /(VariableItem a, VariableItem b)
		{
			if (a.Type != b.Type) throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			if (a.Type == VariableType.Int || a.Type == VariableType.Double || a.Type == VariableType.Byte)
			{
				switch (a.Type)
				{
					case VariableType.Int:
						return new VariableItem(a._Int / b._Int);
					case VariableType.Double:
						return new VariableItem(a._Double / b._Double);
					case VariableType.Byte:
						return new VariableItem((byte)(a._Byte / b._Byte));
				}
			}

			throw new Exception("Unary operation (+) return null !");
		}


		public static VariableItem operator *(VariableItem a, VariableItem b)
		{

			if (a.Type != b.Type || (a.Type==VariableType.String && b.Type==VariableType.Int)) 
				throw new Exception("Type not same for unary operation. Variable: [" + a.Name + "] - [" + b.Name + "]");

			if (a.Type == VariableType.Int || a.Type == VariableType.Double || a.Type == VariableType.Byte ||
				a.Type == VariableType.String)
			{
				switch (a.Type)
				{
					case VariableType.Int:
						return new VariableItem(a._Int * b._Int);
					case VariableType.Double:
						return new VariableItem(a._Double * b._Double);
					case VariableType.Byte:
						return new VariableItem((byte)(a._Byte * b._Byte));
					case VariableType.String:
						string ret = string.Empty;
						for (int i = 0; i < b._Int; i++) ret += a._String;
						return new VariableItem(ret);
				}
			}

			throw new Exception("Unary operation (*) return null !");
		}


		

		#endregion


		public override string ToString()
		{
			return GetData().ToString();
		}
	}

	

	internal class Variables : IDisposable
	{
		private Hashtable _variableList;

		public string[] GetVariableTypeList()
		{
			return Enum.GetNames(typeof(VariableType));
		}

		public Variables()
		{
			_variableList = new Hashtable();
		}

		public VariableItem GetVariable(string name)
		{
			var retVar = ((VariableItem)_variableList[name]);
			if (retVar == null)
			{
				return new VariableItem(name);
			}
			else
			{
				return retVar;
			}
		}

		public void ClearByStatementId(string id)
		{
			var removeList =
				_variableList.Keys.Cast<object>()
					.Where(key => ((VariableItem)_variableList[key]).StatementId == id)
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
				if (string.Equals(dataList[0], "String", StringComparison.OrdinalIgnoreCase))
					variable = varContainer.Replace('"', ' ').Trim();

				// example: int boby = 10
				if (string.Equals(dataList[0], "Int", StringComparison.OrdinalIgnoreCase))
					variable = int.Parse(varContainer);

				// example: bool boby = false
				if (string.Equals(dataList[0], "Bool", StringComparison.OrdinalIgnoreCase))
					variable = Boolean.Parse(varContainer);

				// example: double boby = 1.25
				if (string.Equals(dataList[0], "Double", StringComparison.OrdinalIgnoreCase))
					variable = Double.Parse(varContainer, NumberStyles.Any);

				// example: byte boby = 125
				if (string.Equals(dataList[0], "Byte", StringComparison.OrdinalIgnoreCase))
					variable = byte.Parse(varContainer, NumberStyles.Any);

				// example: char boby = 'a'
				if (string.Equals(dataList[0], "Byte", StringComparison.OrdinalIgnoreCase))
					variable = char.Parse(varContainer);


				// Example: ListString Boby = {"1","1","1","1","1"}
				if (string.Equals(dataList[0], "ListString", StringComparison.OrdinalIgnoreCase))
				{
					if (!varContainer.Contains('{') || !varContainer.Contains('}'))
						throw new Exception("ListString initializer incorrect !");
					variable = varContainer.Replace('{', ' ').Replace('}', ' ').Replace('"', ' ').Trim().Split(',').ToList();
				}

				// Example: ListInt Boby = {1,1,1,1,1}
				if (string.Equals(dataList[0], "ListInt", StringComparison.OrdinalIgnoreCase))
				{
					if (!varContainer.Contains('{') || !varContainer.Contains('}'))
						throw new Exception("ListInt initializer incorrect !");
					var buf2 = varContainer.Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
					variable = buf2.Select(int.Parse).ToList();
				}

				// Example: ListDouble Boby = {1.56,1,1,1,1}
				if (string.Equals(dataList[0], "ListDouble", StringComparison.OrdinalIgnoreCase))
				{
					if (!varContainer.Contains('{') || !varContainer.Contains('}'))
						throw new Exception("ListDouble initializer incorrect !");
					var buf2 = varContainer.Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
					variable = buf2.Select(i => double.Parse(i, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"))).ToList();
				}

				// Example: ListBool Boby = {true,false,true}
				if (string.Equals(dataList[0], "ListBool", StringComparison.OrdinalIgnoreCase))
				{
					if (!varContainer.Contains('{') || !varContainer.Contains('}'))
						throw new Exception("ListBool initializer incorrect !");
					var buf2 = varContainer.Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
					variable = buf2.Select(bool.Parse).ToList();
				}

				// Example: ListByte Boby = {10,125,180}
				if (string.Equals(dataList[0], "ListByte", StringComparison.OrdinalIgnoreCase))
				{
					if (!varContainer.Contains('{') || !varContainer.Contains('}'))
						throw new Exception("ListByte initializer incorrect !");
					var buf2 = varContainer.Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
					variable = buf2.Select(byte.Parse).ToList();
				}

				// Example: ListChar Boby = {'a',';','r'}
				if (string.Equals(dataList[0], "ListChar", StringComparison.OrdinalIgnoreCase))
				{
					if (!varContainer.Contains('{') || !varContainer.Contains('}'))
						throw new Exception("ListChar initializer incorrect !");
					var buf2 = varContainer.Replace('{', ' ').Replace('}', ' ').Replace('\'', ' ').Trim().Split(',');
					variable = buf2.Select(i=>char.Parse(i.Trim())).ToList();
				}

				//dataMass: 0 - type, 1 - name, 2 - data
				_variableList.Add(dataList[1], new VariableItem(variable, dataList[1]) { StatementId = statementId });
				
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
				if (string.Equals(dataList[0], "String", StringComparison.OrdinalIgnoreCase))
					variable = string.Empty;

				// example: int boby
				if (string.Equals(dataList[0], "Int", StringComparison.OrdinalIgnoreCase))
					variable = new int();

				// example: bool boby
				if (string.Equals(dataList[0], "Bool", StringComparison.OrdinalIgnoreCase))
					variable = new bool();

				// example: double boby
				if (string.Equals(dataList[0], "Double", StringComparison.OrdinalIgnoreCase))
					variable = new double();

				// Example: ListString Boby
				if (string.Equals(dataList[0], "ListString", StringComparison.OrdinalIgnoreCase))
					variable = new List<string>();

				// Example: ListString Boby
				if (string.Equals(dataList[0], "ListInt", StringComparison.OrdinalIgnoreCase))
				{
					if (!dataList[3].Contains('{') || !dataList[3].Contains('}'))
						throw new Exception("ListInt initializer incorrect !");
					var buf2 = dataList[3].Replace('{', ' ').Replace('}', ' ').Trim().Split(',');
					variable = buf2.Select(int.Parse).ToList();
				}

				//dataMass: 0 - type, 1 - name, 2 - data
				_variableList.Add(dataList[1], new VariableItem(variable, dataList[1]) { StatementId = statementId });
				
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
