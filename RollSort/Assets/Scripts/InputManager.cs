  using System;
  using Commands;
  using Cysharp.Threading.Tasks;
  using Data.UnityObject;
using Data.ValueObject;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
         #region Self Variables

        [SerializeField] private bool isFirstTimeTouchTaken;
        private Vector2? _mousePosition;
        private QueryPointerOverUIElementCommand _queryPointerOverUIElementCommand;
        private float _currentVelocity;
        private float _targetMoveVectorX;
        private float _moveVectorX;
        private InputData _inputData;
        private bool _isInputEnabled = true;
        private bool _isTakenInput;

        #endregion

        private async void Awake()
        {
            _queryPointerOverUIElementCommand = new QueryPointerOverUIElementCommand();
            AsyncOperationHandle<SO_InputData> handle = Addressables.LoadAssetAsync<SO_InputData>("SO_InputData");
            SO_InputData soInputData = await handle.ToUniTask();
            _inputData = soInputData.Data;
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
        }

        private void StartTakenInput()
        {
            _isTakenInput = true;
            _isInputEnabled = true;
        }

        private void UnsubscribeEvents()
        {
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void OnEnableInput()
        {
            _isInputEnabled = true;
        }

        private void OnDisableInput()
        {
            _isInputEnabled = false;
        }
        private void StopTakenInput()
        {
            _isTakenInput = false;
        }
        private void Update()
        {
            if (_queryPointerOverUIElementCommand.Execute())
            {
                return;   
            }

            if (!_isTakenInput)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                MouseButtonDown();
            }
           
            else if (Input.GetMouseButton(0))
            {
                MouseButtonDrag();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                MouseButtonUp();
            }
        }

        private void MouseButtonDrag()
        {
            if (_mousePosition != null)
            {
                Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;
                _targetMoveVectorX = Mathf.Clamp(mouseDeltaPos.x / Screen.width,_inputData.ClampVector.x , _inputData.ClampVector.y);
                // Smooth geçişi hesapla
                _moveVectorX = Mathf.SmoothDamp(_moveVectorX, _targetMoveVectorX, ref _currentVelocity, _inputData.SmoothTime);
                _mousePosition = Input.mousePosition;
                if (_moveVectorX<= 0.0002f && _moveVectorX >= -0.0002f)
                {
                    _moveVectorX = 0;
                }
            }
        }

        private void MouseButtonUp()
        {
            _moveVectorX = 0;
            _targetMoveVectorX = 0;
            _currentVelocity = 0;
            _mousePosition = null;
            if (!_isInputEnabled)
            {
                return;
            }
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.GetComponent<ISelectable>() != null)
                        {
                            hit.collider.gameObject.GetComponent<ISelectable>().OnDeselect();
                        }
                    }
                }
            }
        }

        private void MouseButtonDown()
        {
            if (!isFirstTimeTouchTaken)
            {
                _mousePosition = Input.mousePosition;
                isFirstTimeTouchTaken = true;
               // _isInputEnabled = true;
                return;
            }
            _mousePosition = Input.mousePosition;
            _moveVectorX = 0; 
            _targetMoveVectorX = 0; 
            _currentVelocity = 0;

            if (!_isInputEnabled)
            {
                return;
            }
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.GetComponent<ISelectable>() != null)
                        {
                            hit.collider.gameObject.GetComponent<ISelectable>().OnSelect();
                        }
                    }
                }
            }
        }
    }
    public struct HorizontalInputDragParams
    {
        public float XValues;
    }
}
