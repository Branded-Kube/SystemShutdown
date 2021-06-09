using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
   public class Director
    {
        private IBuilder builder;

        public Director(IBuilder builder)
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
