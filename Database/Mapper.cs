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
                var effect = reader.GetInt32(2);
                result.Add(new Mods() { Id = id, Name = name, Effect = effect});
            }
            return result;
        }

        
    }
}
