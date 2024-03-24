using Executioner.InputWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executioner.Models
{
    public class NumberUserInputParameter : IBaseUserInputParameter
    {
        public NumberUserInputParameter(string keyword, string name) : base(keyword, name, ParameterType.Number)
        {
        }

        public override string Execute()
        {
            TextInputParameterWindow paramWindow = new TextInputParameterWindow(Name, Name);
            if (paramWindow.ShowDialog() != true)
                throw new ArgumentException($"User cancel");

            return paramWindow.OutputData;
        }
    }
}
