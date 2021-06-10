using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
   public interface IBuilder
    {
        GameObject GetResult();

        void BuildGameObject();
    }
}
