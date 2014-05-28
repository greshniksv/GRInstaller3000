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

        public Functions(FunctionItem function)
        {
            _variables = new Variables();
            _currentFunc = function;
            _commands = new Commands();
            _variableList = new Hashtable();


        }


        public void Execute()
        {

            var managerialWords = new List<string>() { "if", "else", "end", "while" };
            var variableTypeList = _variables.GetVariableTypeList();
            var commandList = _commands.GetCommandList();

            foreach (var codeItem in _currentFunc.Code)
            {
                // Detect variable
                if (variableTypeList.Any(i => codeItem.ToLower().Contains(i.ToLower())))
                    _variables.CreateVariable(codeItem);

                if (commandList.Any(i => codeItem.Contains(i.Name())))
                    _commands.Execute(codeItem);

                // Detect managed words
                if (managerialWords.Any(managerialWord => codeItem.Contains(managerialWord)))
                {
                    /* Execute jumping operators */


                }


            }
        }

        private bool CalculateJumpOper(string data)
        {


            return false;
        }


    }
}
