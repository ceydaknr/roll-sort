using System;
using System.Collections.Generic;

namespace RollSort.Runtime.Pool
{
    public class ObjectPool<T> : AbstractObjectPool
    {
        private readonly List<T> _currentStock;
        private readonly Func<T> _factoryMethod;
        private readonly bool _isDynamic;
        private readonly Action<T> _turnOffCallback;
        private readonly Action<T> _turnOnCallback;

        public ObjectPool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback,
            int initialStock = 0, bool isDynamic = true)
        {
            _factoryMethod = factoryMethod;
            _isDynamic = isDynamic;

            _turnOffCallback = turnOffCallback;
            _turnOnCallback = turnOnCallback;

            _currentStock = new List<T>();

            for (int i = 0; i < initialStock; i++)
            {
                T o = _factoryMethod();
                _turnOffCallback(o);
                _currentStock.Add(o);
            }
        }

        public ObjectPool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback,
            List<T> initialStock, bool isDynamic = true)
        {
            _factoryMethod = factoryMethod;
            _isDynamic = isDynamic;

            _turnOffCallback = turnOffCallback;
            _turnOnCallback = turnOnCallback;

            _currentStock = initialStock;
        }

        public T GetObject()
        {
            T result = default(T);
            if (_currentStock.Count > 0)
            {
                result = _currentStock[0];
                _currentStock.RemoveAt(0);
            }
            else if (_isDynamic)
            {
                result = _factoryMethod();
            }

            _turnOnCallback(result);
            return result;
        }

        public void ReturnObject(T o)
        {
            _turnOffCallback(o);
            _currentStock.Add(o);
        }
    }
}