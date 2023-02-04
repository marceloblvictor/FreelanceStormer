namespace FreelanceStormer.Utils.Interfaces
{
    public interface IDataCache
    {
        T? Get<T>(int key) where T : class;
        void Set<T>(int key, T data);
    }
}