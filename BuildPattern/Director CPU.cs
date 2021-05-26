using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
   public class DirectorCPU
    {
        private IBuilder builder;

        public DirectorCPU(IBuilder builder)
        {
            this.builder = builder;
        }

        public GameObject1 Contruct()
        {
            builder.BuildGameObject();
            return builder.GetResult();
        }
    }
}
