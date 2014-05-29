using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		    bool main = false;
            statementList.Add(Guid.NewGuid().ToString());
            var cmdList = codeItem.Split(' ');

            for (int i = 0; i < cmdList.Count();)
            {
                object q = cmdList[i];
                var oper1 = ((q is string) ? (string)q :
                ((q is int) ? (int)q :
                ((q is bool) ? (bool)q :
                ((q is double) ? (double)q :
                ((q is List<string>) ? (List<string>)q :
                ((q is List<int>) ? (List<int>)q : q))))));

                i++;
            }


		    
			

			var oper2 = ((q is string) ? (string)q :
				((q is int) ? (int)q :
				((q is bool) ? (bool)q :
				((q is double) ? (double)q :
				((q is List<string>) ? (List<string>)q :
				((q is List<int>) ? (List<int>)q : q))))));

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
