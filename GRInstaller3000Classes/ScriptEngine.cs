using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace GRInstaller3000Classes
{
    public class ScriptEngine
    {
        class FunctionItem : ICloneable
        {
            public string Name;
            public List<string> Code;

	        public object Clone()
	        {
				return new FunctionItem() { Name = this.Name, Code = this.Code };
	        }
        }


		class VariableItem
	    {
		    public string Name { get; set; }
			public object Data { get; set; }
			public string Type { get; set; }
	    }

        private readonly List<FunctionItem> _functionList;
	    private readonly Commands _commands;
	    private Hashtable _variableList; 

        public Commands Command
        {
            get { return _commands; }
        }

        public ScriptEngine()
        {
			_commands = new Commands();
            _functionList = new List<FunctionItem>();
			_variableList = new Hashtable();
        }

	    public void ExecuteFunc(string funcName="main")
	    {
		    var managerialWords = new List<string>(){"if","else","end", "while"};
			var variableTypeList = Enum.GetNames(typeof(VariableType));
		    var commandList = _commands.GetCommandList();
		    var func = _functionList.FirstOrDefault(i => i.Name == funcName);
			if(func==null) throw new Exception("Function ["+funcName+"] not found !");

			foreach (var codeItem in func.Code)
			{
				var newVar = variableTypeList.FirstOrDefault(i => codeItem.Contains(i));

				bool isManagerialWords = managerialWords.Any(managerialWord => codeItem.Contains(managerialWord));
				if (!isManagerialWords) _commands.Execute(codeItem);
		    }

	    }

	    public void CreateVariable(string data)
	    {
		    if (data.Contains("="))
		    {
				// contains initializer
				var dataMass = data.Split(' ','=');
			    if (dataMass.Count() != 3)  throw new Exception("Variable with initializer structure error !");
			    object variable = null;

			    if (string.Equals(dataMass[0], "String", StringComparison.CurrentCultureIgnoreCase))
				    variable = dataMass[3];

			    if (string.Equals(dataMass[0], "Int", StringComparison.CurrentCultureIgnoreCase))
				    variable = int.Parse(dataMass[3]);

			    if (string.Equals(dataMass[0], "Bool", StringComparison.CurrentCultureIgnoreCase))
				    variable = Boolean.Parse(dataMass[3]);

				if (string.Equals(dataMass[0], "Double", StringComparison.CurrentCultureIgnoreCase))
					variable = Double.Parse(dataMass[3]);

				// Example ListString Bob = {"1","1","1","1","1"}
			    if (string.Equals(dataMass[0], "ListString", StringComparison.CurrentCultureIgnoreCase))
			    {
				    if (!dataMass[3].Contains('{') || !dataMass[3].Contains('}'))
						throw new Exception("ListString initializer incorrect !");

				    var r = dataMass[3].Split('s');
				    

					variable = dataMass[3].Replace('{', ' ').Replace('}', ' ').Replace('"', ' ').Trim().Split(',').ToList();
			    }
			    //variable = string.Empty;

				if (string.Equals(dataMass[0], "ListInt", StringComparison.CurrentCultureIgnoreCase))
					variable = string.Empty;

				//dataMass: 0 - type, 1 - name, 2 - data
				_variableList.Add(dataMass[1], new VariableItem() { Name = dataMass[1], Type = dataMass[0], Data = variable });
		    }
		    else
		    {
			    
		    }

		    


			
	    }


	    public void LoadScript(string file)
        {
            int ifLevel = 0;
            var func = new List<FunctionItem>();

            using (TextReader reader = new StreamReader(file))
	        {
                string buf;
                while ((buf = reader.ReadLine()) != null)
                {
                    buf = buf.Trim();

                    if (buf.Contains("def"))
                    {
                        var funcName = buf.Remove(buf.IndexOf("def"), 3).Trim();
                        func.Add(new FunctionItem() { Name = funcName, Code = new List<string>() });
                    }

                    if (buf.Contains("if"))
                    {
                        ifLevel++;
                    }

                    if (func != null && !buf.Contains("def")
                        && !(ifLevel == 0 && buf.Contains("end"))
                        && buf.Length > 0)
                    {
                        func[func.Count - 1].Code.Add(buf);
                    }


                    if (buf.Contains("end"))
                    {
                        if (ifLevel > 0) ifLevel--;
                        else
                        {
                            _functionList.Add(func[func.Count - 1].Clone() as FunctionItem);
                            func.Remove(func[func.Count - 1]);
                        }

                    }
                }
	        }
            

	        MessageBox.Show("1");
        }



    }
}
