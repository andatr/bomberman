using System;
using UnityEngine;

namespace Bomberman
{ 
    public class Pool<T>
    {
        #region Public

        public T[] Items { get { return _items; } }

        public int Size { get { return _activeCount; } }

        public Pool(int capacity, Func<int, T> itemFactory)
        {
            _items = new T[capacity];
            for (int i = 0; i < _items.Length; ++i) {
                _items[i] = itemFactory(i);
            }
        }

        public T GetItem()
        {
            if (_activeCount >= _items.Length) {
                Debug.LogWarning($"Using more items than allocated: {_activeCount}");
                return default(T);
            }
            T item = _items[_activeCount];
            ++_activeCount;        
            return item;
        }

        public int Release(int index)
        {
            --_activeCount;
            if (_activeCount == index) return index;
            // swap index <-> lastUsed
            T lastUsed = _items[_activeCount];
            T released = _items[index];
            _items[index] = lastUsed;
            _items[_activeCount] = released;
            return index - 1;
        }

        #endregion

        #region Fields

        private T[] _items;
        private int _activeCount;

        #endregion
    }
}