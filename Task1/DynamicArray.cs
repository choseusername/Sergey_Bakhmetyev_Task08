using System;
using System.Collections.Generic;

namespace Task1
{
    public class DynamicArray<T>/*: IList<T>*/
    {
        static readonly T[] _emptyArray = new T[0];
        static readonly T[] _defaultArray = new T[_defaultCapacity];

        private const int _defaultCapacity = 8;
        private T[] _items;
        private int _size;

        public int Count => _size;
        public int Length => _size;
        public int Capacity
        {
            get => _items.Length;
            set
            {
                if (value < _size)
                    throw new ArgumentOutOfRangeException("Small capacity.");

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                            Array.Copy(_items, 0, newItems, 0, _size);
                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return _items[index];
            }

            set
            {
                if ((uint)index >= (uint)_size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _items[index] = value;
            }
        }

        public DynamicArray()
        {
            _items = _emptyArray;
        }

        public DynamicArray(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("Need non neg numbers capacity.");

            if (capacity == 0)
                _items = _emptyArray;
            else
                _items = new T[capacity];
        }

        public DynamicArray(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            ICollection<T> c = collection as ICollection<T>;
            if (c != null)
            {
                int count = c.Count;
                if (count == 0)
                {
                    _items = _emptyArray;
                }
                else
                {
                    _items = new T[count];
                    c.CopyTo(_items, 0);
                    _size = count;
                }
            }
            else
            {
                _size = 0;
                _items = _emptyArray;

                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Add(en.Current);
                    }
                }
            }
        }

        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                // internal const int MaxArrayLength = 0X7FEFFFFF;
                // internal const int MaxByteArrayLength = 0x7FFFFFC7;

                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > /*Array.MaxArrayLength*/0X7FEFFFFF) newCapacity = /*Array.MaxArrayLength*/0X7FEFFFFF;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }

        public void Add(T item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size++] = item;
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            if ((uint)index > (uint)_size)
            {
                throw new ArgumentOutOfRangeException();
            }

            ICollection<T> c = collection as ICollection<T>;
            if (c != null)
            {
                int count = c.Count;
                if (count > 0)
                {
                    EnsureCapacity(_size + count);
                    if (index < _size)
                    {
                        Array.Copy(_items, index, _items, index + count, _size - index);
                    }

                    if (this == c)
                    {
                        // Copy first part of _items to insert location
                        Array.Copy(_items, 0, _items, index, index);
                        // Copy last part of _items back to inserted location
                        Array.Copy(_items, index + count, _items, index * 2, _size - index);
                    }
                    else
                    {
                        T[] itemsToInsert = new T[count];
                        c.CopyTo(itemsToInsert, 0);
                        itemsToInsert.CopyTo(_items, index);
                    }
                    _size += count;
                }
            }
            else
            {
                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            InsertRange(_size, collection);
        }


        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, _size);
        }

        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_size)
            {
                throw new ArgumentOutOfRangeException();
            }
            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default(T);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void Insert(int index, T item)
        {
            if ((uint)index > (uint)_size)
                throw new ArgumentOutOfRangeException();

            if (_size == _items.Length) EnsureCapacity(_size + 1);

            if (index < _size)
                Array.Copy(_items, index, _items, index + 1, _size - index);

            _items[index] = item;
            _size++;
        }

        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
        }

        public bool Contains(T item)
        {
            if ((Object)item == null)
            {
                for (int i = 0; i < _size; i++)
                    if ((Object)_items[i] == null)
                        return true;
                return false;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;
                for (int i = 0; i < _size; i++)
                {
                    if (c.Equals(_items[i], item)) return true;
                }
                return false;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

    }
}
