using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
    interface IBuilder
    {
        GameObject1 GetResult();

        void BuildGameObject();
    }
}
