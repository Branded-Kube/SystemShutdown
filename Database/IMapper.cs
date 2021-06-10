using System;
using System.Collections.Generic;
using System.Data;

namespace SystemShutdown.Database
{
    // Lead author: Lau
    public interface IMapper
    {
        List<Mods> MapModsFromReader(IDataReader reader);

        List<Effects> MapEffectsFromReader(IDataReader reader);

        List<Highscores> MapHighscoresReader(IDataReader reader);
    }
}
