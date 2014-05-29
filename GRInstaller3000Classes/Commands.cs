using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRInstaller3000Classes
{
    public class Commands : IDisposable
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

            _commandHashTable = new Hashtable();
            foreach (var command in _commandList)
            {
                _commandHashTable.Add(command.Name(),command);
            }

        }

		public IEnumerable<string> GetCommandNameList()
		{
			return _commandList.Select(i => i.Name());
		}

		public Hashtable GetCommandHashTable()
		{
			return _commandHashTable;
		}

        private string GetParamsFromCommand(string command)
        {
            return command.Substring(command.IndexOf('(') + 1, command.IndexOf(')') - (command.IndexOf('(') + 1)).Trim();
        }

        private string GetCommandName(string command)
        {
            if (command.IndexOf("(", System.StringComparison.Ordinal)==-1) 
                throw new Exception("Command does not have '(' !");

            return command.Substring(0, command.IndexOf("(", System.StringComparison.Ordinal)).Trim();
        }

        public void Execute(string command)
        {

            var cmd = _commandHashTable[GetCommandName(command)] as ICommand;
            if(cmd!=null)
                cmd.Execute(GetParamsFromCommand(command));
            else
                throw new Exception("Command '"+command+"' not found !");
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
                return "Info: Show a message.\n\nExample: alert(massage,caption,icon)\n\n" +
                    "Icon: asterisk, error, exclamation, hand, information, none, question, stop, warning";
            }

            public string Execute(string param)
            {
                var parameters = param.Split(',');
                if (parameters.Count() != 3) throw new Exception("Alert error! Count of params not valid! ");

                MessageBoxIcon icon;
                if (!Enum.TryParse(parameters[2], true, out icon)) icon = MessageBoxIcon.Error;
				MessageBox.Show(parameters[0], parameters[1], MessageBoxButtons.OK, (MessageBoxIcon)icon);

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
				return "Info: Show a message.\n\nExample: confirm(massage,caption,icon,button)\n\n" +
                    "Icon: asterisk, error, exclamation, hand, information, none, question, stop, warning\n\n" +
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
                if (_commandList != null)
                {
                    _commandList.Clear();
                    _commandList = null;
                }

                if (_commandHashTable != null)
                {
                    _commandHashTable.Clear();
                    _commandHashTable = null;
                }
            }
        }
        #endregion

    }
}
