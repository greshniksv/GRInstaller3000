using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRInstaller3000Classes
{
    public static class Commands
    {
        public interface ICommand
        { 
            string Name();
            string Information();
            string Execute(string data);
        }

        static List<ICommand> _commandList = new List<ICommand>(); 

        public static IEnumerable<string> GetList()
        {
            _commandList.Add(new Test());
            return _commandList.Select(i => i.Name());
        }

        public class Test : ICommand
        {
            public string Name()
            {
                return "test";
            }

            public string Information()
            {
                return "Test co";
            }

            public string Execute(string data)
            {
                throw new Exception("Execute !");
            }
        }




    }
}
