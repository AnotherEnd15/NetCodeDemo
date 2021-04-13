using System;
using UnityEngine.Lumin;

namespace ET
{
    public class CircularList<T> where T: IDisposable
    {
        private readonly T[] _list;
        private int _maxIndex;
        private readonly int _capacity;

        public int Capacity
        {
            get
            {
                return _capacity;
            }
        }
        
        public int MaxIndex
        {
            get
            {
                return _maxIndex;
            }
        }

        public CircularList(int capacity)
        {
            _capacity = capacity;
            this._list = new T[_capacity];
            for (int i = 0; i < capacity; i++)
            {
                this._list[i] = default (T);
            }
        }


        public void Clear()
        {
            Array.Clear(this._list, 0, _capacity);
            _maxIndex = 0;
        }

        private int MinIndex()
        {
            return this._maxIndex - this._capacity;
        }

        public T this[int index]
        {
            get
            {
                if (index < this.MinIndex())
                {
                    return default(T);
                }

                var i = index % this._capacity;
                return this._list[i];
            }
            set
            {
                if (index < this.MinIndex())
                {
                    return;
                }

                var i = index % this._capacity;
                if (index > _maxIndex)
                {
                    this._maxIndex = index;
                }

                if (this[i] != null)
                    this[i].Dispose();
                this._list[i] = value;
            }
        }

        public T GetLast()
        {
            return this[this._maxIndex];
        }
        
        
    }
}