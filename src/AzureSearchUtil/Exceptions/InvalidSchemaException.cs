using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSearchUtil.Exceptions
{
    /// <summary>
    /// This exception is thrown when the AzureSearch index definition is invalid
    /// </summary>
    public class InvalidSchemaException:Exception
    {
        public InvalidSchemaException(string message) : base(message)
        {
            
        }
    }
}
