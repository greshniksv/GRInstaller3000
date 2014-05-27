using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRInstaller3000Classes
{
    public class Commands
    {
        /* Commands:
         * Copy, Move, Rm, MkDir, RmDir, 
         * File?, Dir?, Zip?, 7zip?, Bzip?
         * UnZip, Un7zip, UnBzip
         * FileCountInDir, DirCount
         * FreeSpaceOnDrive, FileLength, DirLength
         * Alert, Confirm
         * 
         * 
         * 
         */

        public interface ICommand
        {
            string Name();
            string Information();
            string Execute(string param);
        }

        private List<ICommand> _commandList;
        private Hashtable _commandHashTable;

        public Commands()
        {
            // Add command to list
            _commandList = new List<ICommand>
			{
				new Alert(),
				new Confirm()
			};

            foreach (var command in _commandList)
            {
                _commandHashTable.Add(command.Name(),command);
            }

        }

        public string GetParamsFromCommand(string command)
        {
            return command.Substring(command.IndexOf('(') + 1, command.IndexOf(')') - (command.IndexOf('(') + 1)).Trim();
        }

        public void Execute(string command)
        {
            //var cmd = _commandList.FirstOrDefault(com => command.Contains(com.Name()));
            //if (cmd == null) throw new Exception("command '" + command + "' not found !");
            //cmd.Execute(GetParamsFromCommand(command));

            var cmd = _commandHashTable[command] as ICommand;
            if(cmd!=null)
                cmd.Execute(GetParamsFromCommand(command));
            else
                throw new Exception("Command '"+command+"' not found !");
        }

        public IEnumerable<string> GetCommandNameList()
        {
            return _commandList.Select(i => i.Name());
        }

        public List<ICommand> GetCommandList()
        {
            return _commandList;
        }


        #region Commands

        private class Alert : ICommand
        {
            public string Name()
            {
                return "alert";
            }

            public string Information()
            {
                return "Info: Show a message.\n alert(massage,caption,icon)\n " +
                    "Icon: asterisk, error, exclamation, hand, information, none, question, stop, warning";
            }

            public string Execute(string param)
            {
                var parameters = param.Split(',');
                if (parameters.Count() != 3) throw new Exception("Alert error! Count of params not valid! ");

                MessageBoxIcon icon;
                if (!Enum.TryParse(parameters[2], true, out icon)) icon = MessageBoxIcon.Error;
                MessageBox.Show(parameters[0], parameters[1], MessageBoxButtons.OK, icon);

                return null;
            }

        }


        private class Confirm : ICommand
        {
            public string Name()
            {
                return "confirm";
            }

            public string Information()
            {
                return "Info: Show a message.\n alert(massage,caption,icon,button)\n " +
                    "Icon: asterisk, error, exclamation, hand, information, none, question, stop, warning\n" +
                    "Button: AbortRetryIgnore,OK,OKCancel,RetryCancel,YesNo,YesNoCancel";
            }

            public string Execute(string param)
            {
                var parameters = param.Split(',');
                if (parameters.Count() != 4) throw new Exception("Alert error! Count of params not valid! ");

                MessageBoxIcon icon;
                if (!Enum.TryParse(parameters[2], true, out icon)) icon = MessageBoxIcon.Error;

                MessageBoxButtons button;
                if (!Enum.TryParse(parameters[3], true, out button)) button = MessageBoxButtons.YesNo;

                MessageBox.Show(parameters[0], parameters[1], button, icon);

                return null;
            }

        }

        #endregion


    }
}
