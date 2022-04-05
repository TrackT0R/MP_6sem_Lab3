namespace Lab3
{
    internal interface IHashTable<in TKey, in TValue>
    {
        void Add(TKey x, TValue y);
        bool Remove(TKey key);
        bool ContainsKey(TKey x);
    }

    public interface IHash<in TKey>
    {
        int GetHash(TKey key, int i = 0);
    }
}
