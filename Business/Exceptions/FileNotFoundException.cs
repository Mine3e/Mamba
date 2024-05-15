using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Exceptions
{
    public class FileNotFoundException : Exception
    {
        public string PropertyName {  get; set; }
        public FileNotFoundException(string propertyName,string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
