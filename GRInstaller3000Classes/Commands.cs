using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRInstaller3000Classes
{
    public static class Commands
    {
        public interface Command
        { 
            string Name();
            string Information();
            bool Execute(string data);
            
        }


        public static List<string> GetList()
        {
            
        }



    }
}
