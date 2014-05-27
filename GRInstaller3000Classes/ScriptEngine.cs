using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private List<FunctionItem> _functionList;
	    private Commands _commands;

        public Commands Command
        {
            get { return _commands; }
        }

        public ScriptEngine()
        {
			_commands = new Commands();
            _functionList = new List<FunctionItem>();
        }

	    public void ExecuteFunc(string funcName="main")
	    {
		    var managerialWords = new List<string>(){"if","else","end", "while"};
		    var commandList = _commands.GetCommandList();
		    var func = _functionList.FirstOrDefault(i => i.Name == funcName);
			if(func==null) throw new Exception("Function ["+funcName+"] not found !");

			foreach (var codeItem in func.Code)
			{
				bool isManagerialWords = managerialWords.Where(managerialWord => codeItem.Contains(managerialWord)).Any();
				if (!isManagerialWords) _commands.Execute(codeItem);
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
