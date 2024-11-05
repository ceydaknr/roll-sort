using System;
using System.Collections.Generic;
using RollSort.Runtime.Utilities;

namespace RollSort.Runtime.Pool
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private readonly Dictionary<PoolType, AbstractObjectPool> _pools;

        public PoolManager()
        {
            _pools = new Dictionary<PoolType, AbstractObjectPool>();
        }

        public void AddObjectPool<T>(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback,
            int initialStock = 0, bool isDynamic = true) where T : class
        {
            Type poolType = typeof(T);
            PoolType poolKey = GetPoolTypeKey<T>();
            if (!_pools.ContainsKey(poolKey))
                _pools.Add(poolKey,
                    new ObjectPool<T>(factoryMethod, turnOnCallback, turnOffCallback, initialStock, isDynamic));
        }

        public void AddObjectPool<T>(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback,
            PoolType poolType, int initialStock = 0, bool isDynamic = true) where T : class
        {
            if (!_pools.ContainsKey(poolType))
                _pools.Add(poolType,
                    new ObjectPool<T>(factoryMethod, turnOnCallback, turnOffCallback, initialStock, isDynamic));
        }

        public ObjectPool<T> GetObjectPool<T>() where T : class
        {
            PoolType poolKey = GetPoolTypeKey<T>();
            return (ObjectPool<T>)_pools[poolKey];
        }

        public ObjectPool<T> GetObjectPool<T>(PoolType poolType) where T : class
        {
            return (ObjectPool<T>)_pools[poolType];
        }

        public T GetObject<T>() where T : class
        {
            PoolType poolKey = GetPoolTypeKey<T>();
            return ((ObjectPool<T>)_pools[poolKey]).GetObject();
        }

        public T GetObject<T>(PoolType poolType) where T : class
        {
            return ((ObjectPool<T>)_pools[poolType]).GetObject();
        }

        public void ReturnObject<T>(T o) where T : class
        {
            PoolType poolKey = GetPoolTypeKey<T>();
            ((ObjectPool<T>)_pools[poolKey]).ReturnObject(o);
        }

        public void ReturnObject<T>(T o, PoolType poolType) where T : class
        {
            ((ObjectPool<T>)_pools[poolType]).ReturnObject(o);
        }

        public void RemovePool<T>() where T : class
        {
            PoolType poolKey = GetPoolTypeKey<T>();
            _pools[poolKey] = null;
        }

        public void RemovePool(PoolType poolType)
        {
            _pools[poolType] = null;
        }

        private PoolType GetPoolTypeKey<T>() where T : class
        {
            string typeName = typeof(T).Name;
            if (Enum.TryParse(typeName, out PoolType poolType)) return poolType;
            throw new ArgumentException($"No PoolType defined for {typeName}");
        }
    }
}