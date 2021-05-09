using System;
using UnityEngine;

namespace ET
{
    public class   CircularList<T> where T: IDisposable
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
                if (index < this.MinIndex()
                    || index > this._maxIndex)
                {
                    return default (T);
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

                if (index > 10 * 10000 * 10000)
                {
                    throw new Exception("索引太大了, 检查是否有代码错误");
                }

                var i = index % this._capacity;
                if (index > _maxIndex + this._capacity)
                {
                    this.Clear();
                    this._maxIndex = index;  
                }
                else if (index > this._maxIndex)
                {
                    // 触发了边界移动,删除之前的记录
                    for (int j = this.MinIndex(); j < this.MinIndex() + index - this._maxIndex; j++)
                    {
                        this[j]?.Dispose();
                        this[j] = default (T);
                    }
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