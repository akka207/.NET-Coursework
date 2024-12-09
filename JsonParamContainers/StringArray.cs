using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParamContainers
{
    public class StringArray
    {
        public string[] Values {  get; set; }

        public StringArray(string[] values)
        {
            Values = values;
        }

        public StringArray()
        {

        }
    }
}
