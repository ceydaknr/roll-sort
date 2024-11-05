using System;
using System.Collections.Generic;
using Pool;
using RollSort.Runtime.Utilities;

namespace Managers
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private readonly Dictionary<PoolTypes, AbstractObjectPool> _pools;

        public PoolManager()
        {
            _pools = new Dictionary<PoolTypes, AbstractObjectPool>();
        }

        public void AddObjectPool<T>(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback,
            int initialStock = 0, bool isDynamic = true) where T : class
        {
            Type poolType = typeof(T);
            PoolTypes poolKey = GetPoolTypeKey<T>();
            if (!_pools.ContainsKey(poolKey))
                _pools.Add(poolKey,
                    new ObjectPool<T>(factoryMethod, turnOnCallback, turnOffCallback, initialStock, isDynamic));
        }

        public void AddObjectPool<T>(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback,
            PoolTypes poolType, int initialStock = 0, bool isDynamic = true) where T : class
        {
            if (!_pools.ContainsKey(poolType))
                _pools.Add(poolType,
                    new ObjectPool<T>(factoryMethod, turnOnCallback, turnOffCallback, initialStock, isDynamic));
        }

        public ObjectPool<T> GetObjectPool<T>() where T : class
        {
            PoolTypes poolKey = GetPoolTypeKey<T>();
            return (ObjectPool<T>)_pools[poolKey];
        }

        public ObjectPool<T> GetObjectPool<T>(PoolTypes poolType) where T : class
        {
            return (ObjectPool<T>)_pools[poolType];
        }

        public T GetObject<T>() where T : class
        {
            PoolTypes poolKey = GetPoolTypeKey<T>();
            return ((ObjectPool<T>)_pools[poolKey]).GetObject();
        }

        public T GetObject<T>(PoolTypes poolType) where T : class
        {
            return ((ObjectPool<T>)_pools[poolType]).GetObject();
        }

        public void ReturnObject<T>(T o) where T : class
        {
            PoolTypes poolKey = GetPoolTypeKey<T>();
            ((ObjectPool<T>)_pools[poolKey]).ReturnObject(o);
        }

        public void ReturnObject<T>(T o, PoolTypes poolType) where T : class
        {
            ((ObjectPool<T>)_pools[poolType]).ReturnObject(o);
        }

        public void RemovePool<T>() where T : class
        {
            PoolTypes poolKey = GetPoolTypeKey<T>();
            _pools[poolKey] = null;
        }

        public void RemovePool(PoolTypes poolType)
        {
            _pools[poolType] = null;
        }

        private PoolTypes GetPoolTypeKey<T>() where T : class
        {
            string typeName = typeof(T).Name;
            if (Enum.TryParse(typeName, out PoolTypes poolType)) return poolType;
            throw new ArgumentException($"No PoolType defined for {typeName}");
        }
    }
}