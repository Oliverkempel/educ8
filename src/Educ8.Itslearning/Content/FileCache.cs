namespace Educ8.Itslearning.Content;

/// <summary>Small in-memory LRU cache of downloaded files, bounded by count, total size and age.</summary>
internal sealed class FileCache(int maxEntries, long maxTotalBytes, TimeSpan ttl)
{
    private readonly LinkedList<(int Key, DownloadedFile File, DateTime Added)> _entries = new();
    private readonly Lock _lock = new();

    public bool TryGet(int key, out DownloadedFile file)
    {
        lock (_lock)
        {
            for (var node = _entries.First; node is not null; node = node.Next)
            {
                if (node.Value.Key != key) continue;
                if (DateTime.UtcNow - node.Value.Added > ttl) { _entries.Remove(node); break; }
                _entries.Remove(node);
                _entries.AddFirst(node);
                file = node.Value.File;
                return true;
            }
        }
        file = null!;
        return false;
    }

    public void Add(int key, DownloadedFile file)
    {
        if (file.Content.LongLength > maxTotalBytes) return;
        lock (_lock)
        {
            for (var node = _entries.First; node is not null; node = node.Next)
                if (node.Value.Key == key) { _entries.Remove(node); break; }
            _entries.AddFirst((key, file, DateTime.UtcNow));
            while (_entries.Count > maxEntries || _entries.Sum(e => e.File.Content.LongLength) > maxTotalBytes)
                _entries.RemoveLast();
        }
    }
}
