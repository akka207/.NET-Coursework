using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParamContainers
{
    public class Param
    {
        public string Value { get; set; }

        public Param(string value) 
        {
            Value = value;
        }
    }
}
