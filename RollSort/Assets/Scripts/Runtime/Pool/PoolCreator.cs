using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.UnityObject;
using Managers;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Pool
{
    public class PoolCreator : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private Transform _objTransformCache;
        [ShowInInspector] private SO_Pool _poolData;
        private PoolTypes _listCache;
        private Dictionary<PoolTypes, GameObject> _poolGroup = new();

        #endregion

        #endregion

        private async void Awake()
        {
            AsyncOperationHandle<SO_Pool> handle = Addressables.LoadAssetAsync<SO_Pool>("SO_Pool");
            _poolData = await handle.ToUniTask();
            CreatGameObjectGroup();
            InitPool();
        }

        private void CreatGameObjectGroup()
        {
            foreach (var value in _poolData.PoolTypeDatas)
            {
                var gameObjectCache = new GameObject
                {
                    name = value.Key.ToString(),
                    transform =
                    {
                        parent = transform
                    }
                };
                _poolGroup.Add(value.Key, gameObjectCache);
            }
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PoolSignals.Instance.onGetPoolObject += OnGetPoolObject;
            PoolSignals.Instance.onReleasePoolObject += OnReleasePoolObject;
        }

        private void UnsubscribeEvents()
        {
            PoolSignals.Instance.onGetPoolObject -= OnGetPoolObject;
            PoolSignals.Instance.onReleasePoolObject -= OnReleasePoolObject;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private GameObject OnGetPoolObject(PoolTypes poolType, Transform objTransform)//TODO:should be deleted transform
        {
            _listCache = poolType;
            _objTransformCache = objTransform;
            var obj = PoolManager.Instance.GetObject<GameObject>(poolType);
            obj.transform.SetParent(objTransform,false);
            return obj;
        }
        private GameObject OnGetPoolObjectWithParent(PoolTypes poolType, Transform objTransform)
        {
            _listCache = poolType;
            _objTransformCache = objTransform;
            var obj = PoolManager.Instance.GetObject<GameObject>(poolType);
           // obj.transform.parent = objTransform;
            return obj;
        }

        private void OnReleasePoolObject(PoolTypes poolType, GameObject obj)
        {
            _listCache = poolType;
            obj.transform.SetParent(_poolGroup[_listCache].transform,false);
            PoolManager.Instance.ReturnObject(obj, poolType);
        }

        #region Pool Initialization

        private void InitPool()
        {
            foreach (var variable in _poolData.PoolTypeDatas)
            {
                _listCache = variable.Key;
                PoolManager.Instance.AddObjectPool<GameObject>(FabricateGameObject, TurnOnGameObject, TurnOffGameObject,
                    variable.Key, variable.Value.ObjectLimit, true);
            }
        }

        private void TurnOnGameObject(GameObject gameObject)
        {
            gameObject.transform.localPosition = _objTransformCache.position;
            gameObject.SetActive(true);
        }

        private void TurnOffGameObject(GameObject gameObject)
        {
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.SetParent(_poolGroup[_listCache].transform);
            gameObject.SetActive(false);
        }

        private GameObject FabricateGameObject()
        {
            return Instantiate(_poolData.PoolTypeDatas[_listCache].PooledGameObject, Vector3.zero,
                _poolData.PoolTypeDatas[_listCache].PooledGameObject.transform.rotation,
                _poolGroup[_listCache].transform);
        }

        #endregion
    }
    public enum PoolTypes
    {
        Coin
    }
}