using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.Database
{
    public interface IRepository
    {
        Mods FindMods(string name);
        void AddMods(string name);

        List <Effects> FindEffects(int modfk);
        void AddEffects(int effect, string effectname, int modfk);

        void Open();

        void Close();
    }
}
