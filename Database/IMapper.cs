using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SystemShutdown.Database
{
    // Lau
    public interface IMapper
    {
        List<Mods> MapModsFromReader(IDataReader reader);

        List<Effects> MapEffectsFromReader(IDataReader reader);

        List<Highscores> MapHighscoresReader(IDataReader reader);
    }
}
