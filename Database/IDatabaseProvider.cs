using System;
using System.Data;

namespace SystemShutdown.Database
{
    // Lead author: Lau
    public interface IDatabaseProvider
    {
        IDbConnection CreateConnection();
    }
}
