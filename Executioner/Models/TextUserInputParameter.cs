using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executioner.Models
{
    public class TextUserInputParameter : BaseUserInputParameter
    {
        public TextUserInputParameter(string keyword, string name) : base(keyword, name, ParameterType.Text)
        {
        }
    }
}
