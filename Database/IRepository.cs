using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.Database
{
    public interface IRepository
    {
        Mods FindMods(string name);
        void Open();

        void Close();
    }
}
