using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp2
{
    public class MyCustomDictionary<Tkey, TValue> : IEnumerable<KeyValuePair<Tkey, TValue>>
    {
        private class EntryList
        {
            public Tkey Key;
            public TValue Value;
            public EntryList? Next;

            public EntryList(Tkey key, TValue value, EntryList? next = null)
            {
                Key = key;
                Value = value;
                Next = next;
            }
        }

        private const int defaultCapacity = 19;
        private int _count;
        private EntryList[] _list;
        private const double LoadFactor = 0.82;
        private readonly object _dictionaryLock = new object();

        public MyCustomDictionary(int capacity = defaultCapacity)
        {
            _list = new EntryList[capacity];
        }

        public int Count
        {
            get
            {
                lock (_dictionaryLock)
                {
                    return _count;
                }
            }
        }

        private int GetListIndex(Tkey key)
        {
            int hash = EqualityComparer<Tkey>.Default.GetHashCode(key);
            return Math.Abs(hash % _list.Length);
        }

        private void Rehash()
        {
            int newCapacity = _list.Length * 2;
            EntryList[] newList = new EntryList[newCapacity];
            foreach (EntryList entry in _list)
            {
                var temp = entry;
                while (temp != null)
                {
                    EntryList? next = temp.Next;

                    int newIndex = Math.Abs(EqualityComparer<Tkey>.Default.GetHashCode(temp.Key) % newCapacity);

                    temp.Next = newList[newIndex];
                    newList[newIndex] = temp;
                    temp = next;
                }
            }
            _list = newList;
            //Console.WriteLine($"Rehashing to new capacity: {newCapacity}");
        }

        public bool Add(Tkey key, TValue value)
        {
            
            lock (_dictionaryLock)
            {
                int index = GetListIndex(key);
                EntryList? entry = _list[index];
                while (entry != null)
                {
                    if (EqualityComparer<Tkey>.Default.Equals(key, entry.Key))
                    {
                        return false;
                    }
                    entry = entry.Next;
                }

                EntryList newEntry = new EntryList(key, value, _list[index]);
                _list[index] = newEntry;
                _count++;

                if (_count / _list.Length > LoadFactor)
                {
                    Rehash();
                }
                return true;
            }
        }

        public bool GetKey(TValue value, out Tkey key)
        {
            lock (_dictionaryLock)
            {
                foreach (EntryList entry in _list)
                {
                    var temp = entry;
                    while (temp != null)
                    {
                        if (EqualityComparer<TValue>.Default.Equals(value, temp.Value))
                        {
                            key = temp.Key;
                            return true;
                        }
                        temp = temp.Next;
                    }
                }
                key = default!;
                return false;
            }
        }

        public IEnumerable<Tkey> GetAllKeys()
        {
            lock (_dictionaryLock)
            {
                IList<Tkey> keys = new List<Tkey>();
                foreach (EntryList entry in _list)
                {
                    var temp = entry;
                    while (temp != null)
                    {
                        keys.Add(temp.Key);
                        temp = temp.Next;
                    }
                }
                return keys;
            }
        }

        public IEnumerable<TValue> GetAllValues()
        {
            lock (_dictionaryLock)
            {
                IList<TValue> values = new List<TValue>();
                foreach (EntryList entry in _list)
                {
                    var temp = entry;
                    while (temp != null)
                    {
                        values.Add(temp.Value);
                        temp = temp.Next;
                    }
                }
                return values;
            }
        }

        public bool GetValue(Tkey key, out TValue value)
        {
            lock (_dictionaryLock)
            {
                int index = GetListIndex(key);
                EntryList? entry = _list[index];
                while (entry != null)
                {
                    if (EqualityComparer<Tkey>.Default.Equals(key, entry.Key))
                    {
                        value = entry.Value;
                        return true;
                    }
                    entry = entry.Next;
                }
                value = default!;
                return false;
            }
        }

        public bool Remove(Tkey key)
        {
            lock (_dictionaryLock)
            {
                int index = GetListIndex(key);
                EntryList? entry = _list[index];
                EntryList? previous = null;
                while (entry != null)
                {
                    if (EqualityComparer<Tkey>.Default.Equals(key, entry.Key))
                    {
                        if (previous == null)
                        {
                            _list[index] = entry.Next;
                        }
                        else
                        {
                            previous.Next = entry.Next;
                        }
                        _count--;
                        return true;
                    }
                    previous = entry;
                    entry = entry.Next;
                }
                return false;
            }
        }

        public bool RemoveValue(TValue value)
        {
            lock (_dictionaryLock)
            {
                foreach (EntryList item in _list)
                {
                    EntryList? entryList = item;
                    EntryList? previous = null;
                    while (entryList != null)
                    {
                        if (EqualityComparer<TValue>.Default.Equals(value, entryList.Value))
                        {
                            int index = GetListIndex(entryList.Key);
                            if (previous == null)
                            {
                                _list[index] = entryList.Next;
                            }
                            else
                            {
                                previous.Next = entryList.Next;
                            }
                            _count--;
                            return true;
                        }
                        previous = entryList;
                        entryList = entryList.Next;
                    }
                }
                return false;
            }
        }

        public int RemoveAllValue(TValue value)
        {
            lock (_dictionaryLock)
            {
                int count = 0;
                foreach (EntryList item in _list)
                {
                    EntryList? entryList = item;
                    EntryList? previous = null;
                    while (entryList != null)
                    {
                        if (EqualityComparer<TValue>.Default.Equals(value, entryList.Value))
                        {
                            int index = GetListIndex(entryList.Key);
                            if (previous == null)
                            {
                                _list[index] = entryList.Next;
                                count++;
                                _count--;
                                entryList = _list[index];
                                continue; // reset loop since head changed
                            }
                            else
                            {
                                previous.Next = entryList.Next;
                                count++;
                                _count--;
                                entryList = previous.Next;
                                continue;
                            }
                        }
                        previous = entryList;
                        entryList = entryList.Next;
                    }
                }
                return count;
            }
        }

        public bool ContainsKey(Tkey key)
        {
            
                return GetValue(key, out _);
            
        }

        public void Clear()
        {
            lock (_dictionaryLock)
            {
                _count = 0;
                Array.Fill(_list, null);
            }
        }

        public IEnumerator<KeyValuePair<Tkey, TValue>> GetEnumerator()
        {
            List<KeyValuePair<Tkey, TValue>> snapshot;
            lock (_dictionaryLock)
            {
                snapshot = new List<KeyValuePair<Tkey, TValue>>();
                foreach (EntryList entry in _list)
                {
                    var temp = entry;
                    while (temp != null)
                    {
                        snapshot.Add(new KeyValuePair<Tkey, TValue>(temp.Key, temp.Value));
                        temp = temp.Next;
                    }
                }
            }
            return snapshot.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TValue this[Tkey key]
        {
            get
            {
                lock (_dictionaryLock)
                {
                    if (GetValue(key, out var value))
                        return value;
                    throw new KeyNotFoundException($"Key '{key}' not found.");
                }
            }
            set
            {
                lock (_dictionaryLock)
                {
                    int index = GetListIndex(key);
                    EntryList? entry = _list[index];
                    while (entry != null)
                    {
                        if (EqualityComparer<Tkey>.Default.Equals(entry.Key, key))
                        {
                            entry.Value = value; // update the value
                            return;
                        }
                        entry = entry.Next;
                    }
                    _list[index] = new EntryList(key, value, _list[index]);
                    _count++;
                    if (_count / _list.Length > LoadFactor)
                    {
                        Rehash();
                    }
                }
            }
        }
    }
}
