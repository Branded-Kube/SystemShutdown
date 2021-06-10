using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
   public interface IBuilder
    {
        GameObject1 GetResult();

        void BuildGameObject();
    }
}
