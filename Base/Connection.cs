using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatorScript.Base
{
    sealed class Connection
    {
        public readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ihoms"].ConnectionString;
    }
}
