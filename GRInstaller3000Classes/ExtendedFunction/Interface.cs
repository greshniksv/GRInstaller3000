using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRInstaller3000Classes.ExtendedFunction
{
	interface IExtendedFunction
	{
		string Name();
		string Help();
		VariableItem Execute(string command);

	}
}
