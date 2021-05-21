using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SystemShutdown.Database
{
    public interface IDatabaseProvider
    {
        IDbConnection CreateConnection();
    }
}
