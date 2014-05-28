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
        private Variables _variables;

	    public delegate void ExecuteCode(string function, string command);
		public event ExecuteCode ExecuteCodeEvent = delegate { };

        public Functions(FunctionItem function)
        {
            _variables = new Variables();
            _currentFunc = function;
            _commands = new Commands();
            _variableList = new Hashtable();
        }

	    public void Execute()
        {
			bool excludeElseBlock = false;

            var managerialWords = new List<string>() { "if", "else", "end", "while" };
            var variableTypeList = _variables.GetVariableTypeList();
	        var commandList = _commands.GetCommandNameList();

            //foreach (var codeItem in _currentFunc.Code)
			for(var pos=0; pos<_currentFunc.Code.Count; pos++)
			{
				var codeItem = _currentFunc.Code[pos];
	            ExecuteCodeEvent(_currentFunc.Name,codeItem);

                // Detect variable
                if (variableTypeList.Any(i => codeItem.ToLower().Contains(i.ToLower())))
                    _variables.CreateVariable(codeItem);

                if (commandList.Any(codeItem.Contains))
                    _commands.Execute(codeItem);

                // Detect managed words
                if (managerialWords.Any(codeItem.Contains))
                {
                    /* Execute jumping operators */
	                CalculateJumpOper(codeItem);

                }


            }
        }

	    private bool CalculateJumpOper(string data)
        {


            return false;
        }


    }
}
