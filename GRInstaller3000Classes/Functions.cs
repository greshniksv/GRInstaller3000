using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRInstaller3000Classes
{
    internal class Functions
    {
        private Hashtable _variableList;
        private readonly Commands _commands;
        private readonly FunctionItem _currentFunc;
        private readonly Variables _variables;
	    private readonly Hashtable _jumpList;

	    public delegate void ExecuteCode(string function, string command);
		public event ExecuteCode ExecuteCodeEvent = delegate { };


	    public Functions(FunctionItem function)
        {
            _variables = new Variables();
            _currentFunc = function;
            _commands = new Commands();
            _variableList = new Hashtable();
			_jumpList = new Hashtable();
        }


	    public void Execute()
        {
            var managerialWords = new List<string>() { "if", "else", "end", "while" };
            var variableTypeList = _variables.GetVariableTypeList();
	        var commandList = _commands.GetCommandNameList();

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
					CalculateJumpOper(codeItem, pos);
					continue;
				}

                // Detect variable
                if (variableTypeList.Any(i => codeItem.ToLower().Contains(i.ToLower())))
                    _variables.CreateVariable(codeItem);

				// Detect command
                if (commandList.Any(codeItem.Contains))
                    _commands.Execute(codeItem);

                

            }
        }

	    private void  CalculateJumpOper(string data, int pos)
	    {
		    bool main = false;



		    object q;
			var oper1 = ((q is string) ? (string)q :
				((q is int) ? (int)q :
				((q is bool) ? (bool)q :
				((q is double) ? (double)q :
				((q is List<string>) ? (List<string>)q :
				((q is List<int>) ? (List<int>)q : q))))));

			var oper2 = ((q is string) ? (string)q :
				((q is int) ? (int)q :
				((q is bool) ? (bool)q :
				((q is double) ? (double)q :
				((q is List<string>) ? (List<string>)q :
				((q is List<int>) ? (List<int>)q : q))))));

	    }


    }
}
