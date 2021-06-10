using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
    // Lead author: Frederik
    public class Director
    {
        #region Fields
        private IBuilder builder;
        #endregion

        #region Constructor
        public Director(IBuilder builder)
        {
            this.builder = builder;
        }
        #endregion

        #region Methods
        public GameObject Contruct()
        {
            builder.BuildGameObject();
            return builder.GetResult();
        }
        #endregion
    }
}
