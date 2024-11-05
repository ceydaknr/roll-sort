using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RollSort.Runtime.Pool
{
    public class PoolCreator : MonoBehaviour
    {
        private readonly Dictionary<PoolType, GameObject> _poolGroup = new();
        private PoolType _listCache;
        private Transform _objTransformCache;
        [ShowInInspector] private SO_Pool _poolData;

        private async void Awake()
        {
            AsyncOperationHandle<SO_Pool> handle = Addressables.LoadAssetAsync<SO_Pool>("SO_Pool");
            _poolData = await handle.ToUniTask();
            CreatGameObjectGroup();
            InitPool();
        }

        private void CreatGameObjectGroup()
        {
            foreach (KeyValuePair<PoolType, PoolTypeData> value in _poolData.PoolTypeDatas)
            {
                GameObject gameObjectCache = new()
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

        private GameObject OnGetPoolObject(PoolType poolType, Transform objTransform) //TODO:should be deleted transform
        {
            _listCache = poolType;
            _objTransformCache = objTransform;
            GameObject obj = PoolManager.Instance.GetObject<GameObject>(poolType);
            obj.transform.SetParent(objTransform, false);
            return obj;
        }

        private GameObject OnGetPoolObjectWithParent(PoolType poolType, Transform objTransform)
        {
            _listCache = poolType;
            _objTransformCache = objTransform;
            GameObject obj = PoolManager.Instance.GetObject<GameObject>(poolType);
            return obj;
        }

        private void OnReleasePoolObject(PoolType poolType, GameObject obj)
        {
            _listCache = poolType;
            obj.transform.SetParent(_poolGroup[_listCache].transform, false);
            PoolManager.Instance.ReturnObject(obj, poolType);
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

        #region Pool Initialization

        private void InitPool()
        {
            foreach (KeyValuePair<PoolType, PoolTypeData> variable in _poolData.PoolTypeDatas)
            {
                _listCache = variable.Key;
                PoolManager.Instance.AddObjectPool(FabricateGameObject, TurnOnGameObject, TurnOffGameObject,
                    variable.Key, variable.Value.ObjectLimit);
            }
        }

        private void TurnOnGameObject(GameObject go)
        {
            go.transform.localPosition = _objTransformCache.position;
            go.SetActive(true);
        }

        private void TurnOffGameObject(GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.SetParent(_poolGroup[_listCache].transform);
            go.SetActive(false);
        }

        private GameObject FabricateGameObject()
        {
            return Instantiate(_poolData.PoolTypeDatas[_listCache].PooledGameObject, Vector3.zero,
                _poolData.PoolTypeDatas[_listCache].PooledGameObject.transform.rotation,
                _poolGroup[_listCache].transform);
        }

        #endregion
    }

    public enum PoolType
    {
        Container
    }
}