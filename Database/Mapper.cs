using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SystemShutdown.Database
{
    public class Mapper : IMapper
    {
        public List<Mods> MapModsFromReader(IDataReader reader)
        {
            var result = new List<Mods>();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                result.Add(new Mods() { Id = id, Name = name});
            }
            return result;
        }

        public List<Effects> MapEffectsFromReader(IDataReader reader)
        {
            var result = new List<Effects>();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var effect = reader.GetInt32(1);
                var name = reader.GetString(2);
                var modfk = reader.GetInt32(3);
                result.Add(new Effects() { Id = id, Effect = effect, Effectname = name, ModFK = modfk });
            }
            return result;
        }


    }
}
