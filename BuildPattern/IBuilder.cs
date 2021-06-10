using SystemShutdown.ComponentPattern;

namespace SystemShutdown.BuildPattern
{
    //  Lead author: Frederik
    public interface IBuilder
    {
        GameObject GetResult();

        void BuildGameObject();
    }
}
