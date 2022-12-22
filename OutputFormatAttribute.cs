using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaluctorSchoolProect
{
    class OutputFormatAttribute : Attribute
    {
        public OutputFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; }
    }
}
