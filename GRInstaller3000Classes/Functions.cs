using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRInstaller3000Classes
{
    internal class Functions : IDisposable
    {
        private Commands _commands;
        private readonly FunctionItem _currentFunc;
        private Variables _variables;
	    private Hashtable _jumpList;

	    public delegate void ExecuteCode(string function, string command);
		public event ExecuteCode ExecuteCodeEvent = delegate { };


	    public Functions(FunctionItem function)
        {
            _variables = new Variables();
            _currentFunc = function;
            _commands = new Commands();
			_jumpList = new Hashtable();
        }


	    public void Execute()
        {
            var managerialWords = new List<string>() { "if", "else", "end", "while" };
            var variableTypeList = _variables.GetVariableTypeList();
	        var commandList = _commands.GetCommandNameList();
	        var statementList = new List<string> {_currentFunc.Id};

	        for(var pos=0; pos<_currentFunc.Code.Count; pos++)
			{
				if (_jumpList[pos] != null)
				{
					pos = (int)_jumpList[pos];
					_jumpList.Remove(pos);
					continue;
				}




				var codeItem = _currentFunc.Code[pos];
	            ExecuteCodeEvent(_currentFunc.Name,codeItem);

				// remove comment string
				if (codeItem.Contains("#"))
				{
					codeItem = codeItem.Substring(codeItem.IndexOf("#"), codeItem.Length - codeItem.IndexOf("#")).Trim();
					if (codeItem.Length < 1) continue;
				}


				// Detect managed words
				if (managerialWords.Any(codeItem.Contains))
				{
					/* Execute jumping operators */
                    CalculateJumpOper(pos, ref statementList);
					continue;
				}

                // Detect variable
                if (variableTypeList.Any(i => codeItem.ToLower().Contains(i.ToLower())))
                    _variables.CreateVariable(codeItem, statementList[statementList.Count-1]);

				// Detect command
                if (commandList.Any(codeItem.Contains))
                    _commands.Execute(codeItem);

                // Remove last statement id
			    if (codeItem.Contains("end"))
			    {
                    _variables.ClearByStatementId(statementList[statementList.Count - 1]);
			        statementList.Remove(statementList[statementList.Count - 1]);
			    }
			}
        }

        private void CalculateJumpOper(int pos, ref List<string> statementList)
	    {
            var codeItem = _currentFunc.Code[pos];
            statementList.Add(Guid.NewGuid().ToString());
            var cmdList = new List<string>(codeItem.Split(' '));

	        if (cmdList[0].Equals("if", StringComparison.OrdinalIgnoreCase))
	        {
	            for (int i = 1; i < cmdList.Count; i++)
	            {
	                if (cmdList[i].Equals(">") || cmdList[i].Equals(">=") ||
	                    cmdList[i].Equals("<") || cmdList[i].Equals("<=") ||
	                    cmdList[i].Equals("==") || cmdList[i].Equals("!="))
	                {
	                    var o1 = _variables.GetVariable(cmdList[i - 1]);
                        var o2 = _variables.GetVariable(cmdList[i + 1]);
	                    bool rez;

	                    switch (cmdList[i])
	                    {
	                        case ">": rez = o1 > o2; break;
                            case ">=": rez = o1 >= o2; break;
                            case "<": rez = o1 < o2; break;
                            case "<=": rez = o1 <= o2; break;
                            case "==": rez = o1 == o2; break;
                            case "!=": rez = o1 != o2; break;
                            default: throw new Exception("MAGIC!");
	                    }
	                    cmdList[i - 1] = "";
                        cmdList[i + 1] = "";
                        cmdList[i] = (rez?"true":"false");
	                }

	            }



	        }


	    }




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
                if (_jumpList != null)
                {
                    _jumpList.Clear();
                    _jumpList = null;
                }

                if (_variables != null)
                {
                    _variables.Dispose();
                    _variables = null;
                }

                if (_commands != null)
                {
                    _commands.Dispose();
                    _commands = null;
                }
            }
        }
        #endregion
    }
}
