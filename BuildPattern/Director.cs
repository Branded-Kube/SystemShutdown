using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
    // Frederik
   public class Director
    {
        private IBuilder builder;

        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        public GameObject Contruct()
        {
            builder.BuildGameObject();
            return builder.GetResult();
        }
    }
}
