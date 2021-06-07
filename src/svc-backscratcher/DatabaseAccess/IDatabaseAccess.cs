using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.DatabaseAccess
{
    public interface IDatabaseAccess
    {
        IDbConnection GetDatabaseConnection();
    }
}
