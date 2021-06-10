using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SystemShutdown.Database
{
    // Lau
    public interface IDatabaseProvider
    {
        IDbConnection CreateConnection();
    }
}
