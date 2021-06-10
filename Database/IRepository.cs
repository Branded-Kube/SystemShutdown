using System;
using System.Collections.Generic;

namespace SystemShutdown.Database
{
    public interface IRepository
    {
        // Lead author: Lau
        Mods FindMods(string name);
        void AddMods(string name);

        List <Effects> FindEffects(int modfk);
        void AddEffects(int effect, string effectname, int modfk);

        void Open();

        void Close();
    }
}
