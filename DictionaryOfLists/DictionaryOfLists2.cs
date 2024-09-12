using System.Collections;
using System.Diagnostics;

namespace DictionaryOfLists;

/// <summary>
///     A dictionary of lists. When accessing elements via the indexer, the list for a key is already
///     constructed and can be used immediately. When a list becomes empty, it is automatically removed from the
///     dictionary.
/// </summary>
/// <typeparam name="TKey">The type of the keys.</typeparam>
/// <typeparam name="TItem">The type for the items in the list.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public class DictionaryOfLists2<TKey, TItem> : IDictionary<TKey, IList<TItem>> where TKey : notnull
{
    private readonly Dictionary<TKey, ListProxy> _dict;

    /// <summary>
    ///     Constructor.
    /// </summary>
    public DictionaryOfLists2()
    {
        _dict = new Dictionary<TKey, ListProxy>();
    }

    /// <summary>
    ///     Copy-constructor.
    /// </summary>
    /// <param name="other">The list dictionary to copy.</param>
    public DictionaryOfLists2(DictionaryOfLists2<TKey, TItem> other)
    {
        _dict = new Dictionary<TKey, ListProxy>(other._dict);
    }

    /// <summary>
    ///     The total capacity of all lists.
    /// </summary>
    public long TotalCapacity => _dict.Sum(kv => kv.Value.Capacity);

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<TKey, IList<TItem>>> GetEnumerator()
    {
        return _dict.Select(kv => new KeyValuePair<TKey, IList<TItem>>(kv.Key, kv.Value)).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dict).GetEnumerator();
    }

    /// <inheritdoc />
    public void Add(TKey key, IList<TItem> value)
    {
        if (_dict.ContainsKey(key))
            throw new ArgumentException("An item with the same key has already been added.");

        if (value.Any())
            _dict.Add(key, new ListProxy(this, key, value));
    }

    /// <inheritdoc />
    public void Add(KeyValuePair<TKey, IList<TItem>> kv)
    {
        Add(kv.Key, kv.Value);
    }

    /// <inheritdoc />
    public bool ContainsKey(TKey key)
    {
        return _dict.TryGetValue(key, out var listProxy) && listProxy.Count != 0;
    }

    /// <inheritdoc />
    public bool Contains(KeyValuePair<TKey, IList<TItem>> kv)
    {
        return ContainsKey(kv.Key);
    }

    /// <inheritdoc />
    public void CopyTo(KeyValuePair<TKey, IList<TItem>>[] array, int arrayIndex)
    {
        var dict = _dict.ToDictionary(kv => kv.Key, kv => (IList<TItem>)kv.Value);
        ((ICollection<KeyValuePair<TKey, IList<TItem>>>)dict).CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    public bool Remove(TKey key)
    {
        if (_dict.TryGetValue(key, out var listProxy))
        {
            listProxy.ClearInternal();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc />
    public bool Remove(KeyValuePair<TKey, IList<TItem>> kv)
    {
        return Remove(kv.Key);
    }

    /// <inheritdoc />
    public void Clear()
    {
        _dict.Clear();
    }

    /// <inheritdoc />
    public int Count => _dict.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public bool TryGetValue(TKey key, out IList<TItem> value)
    {
        if (_dict.TryGetValue(key, out var listProxy))
        {
            value = listProxy;
            return listProxy.Count != 0;
        }
        else
        {
            var newCollection = new ListProxy(this, key);
            _dict[key] = newCollection;
            value = newCollection;
            return false;
        }
    }

    /// <inheritdoc />
    public IList<TItem> this[TKey key]
    {
        get
        {
            if (_dict.TryGetValue(key, out var listProxy))
            {
                return listProxy;
            }
            else
            {
                var newCollection = new ListProxy(this, key);
                _dict[key] = newCollection;
                return newCollection;
            }
        }

        set
        {
            if (value.Any())
                _dict[key] = new ListProxy(this, key, value);
            else
                Remove(key);
        }
    }

    /// <inheritdoc />
    public ICollection<TKey> Keys => _dict.Keys;

    /// <inheritdoc />
    public ICollection<IList<TItem>> Values
    {
        get { return _dict.Values.Select(v => (IList<TItem>)v).ToList(); }
    }

    /// <summary>
    ///     A proxy that wraps a list and in addition to forwarding the operations also performs the necessary
    ///     housekeeping procedures for the list dictionary.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    private sealed class ListProxy : IList<TItem>
    {
        private readonly TKey _key;
        private readonly List<TItem> _list;
        private readonly DictionaryOfLists2<TKey, TItem> _owner;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="owner">The owning list dictionary.</param>
        /// <param name="key">The key for this list.</param>
        /// <param name="list">The list to copy from or to use as underlying list.</param>
        public ListProxy(DictionaryOfLists2<TKey, TItem> owner, TKey key, IList<TItem> list)
        {
            _owner = owner;
            _key = key;
            _list = [..list]; // copy - otherwise the guarantees of the list dictionary may fail!
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="owner">The owning list dictionary.</param>
        /// <param name="key">The key for this list.</param>
        public ListProxy(DictionaryOfLists2<TKey, TItem> owner, TKey key)
        {
            _owner = owner;
            _key = key;
            _list = [];
        }

        /// <summary>
        ///     The capacity of the list.
        /// </summary>
        public long Capacity => _list.Capacity;

        /// <inheritdoc />
        public IEnumerator<TItem> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(TItem item)
        {
            _list.Add(item);
        }

        /// <inheritdoc />
        public void Insert(int index, TItem item)
        {
            _list.Insert(index, item);
        }

        /// <inheritdoc />
        public bool Remove(TItem item)
        {
            var result = _list.Remove(item);

            if (_list.Count == 0)
                _owner.Remove(_key);

            return result;
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);

            if (_list.Count == 0)
                _owner.Remove(_key);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _list.Clear();
            _owner.Remove(_key);
        }

        /// <inheritdoc />
        public bool Contains(TItem item)
        {
            return _list.Contains(item);
        }

        /// <inheritdoc />
        public int IndexOf(TItem item)
        {
            return _list.IndexOf(item);
        }

        /// <inheritdoc />
        public void CopyTo(TItem[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public int Count => _list.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public TItem this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        /// <summary>
        ///     Clear the internal list without notifying the owner.
        /// </summary>
        internal void ClearInternal()
        {
            _list.Clear();
        }
    }
}