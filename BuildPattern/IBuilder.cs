using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
    // Frederik
    public interface IBuilder
    {
        GameObject GetResult();

        void BuildGameObject();
    }
}
