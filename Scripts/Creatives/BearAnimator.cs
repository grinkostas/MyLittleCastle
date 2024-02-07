using NaughtyAttributes;
using StaserSDK;
using UnityEngine;

namespace GameCore.Scripts.Creatives
{
    public class BearAnimator : MonoBehaviour
    {
        [SerializeField] private bool _animatorLinker;
        [SerializeField, HideIf(nameof(_animatorLinker))] private Animator _animator;
        [SerializeField, ShowIf(nameof(_animatorLinker))] private AnimatorLinker _linker;
        [SerializeField] private CharacterControllerMovement _characterMovement;
        [SerializeField] public float _maxSpeed;
        [SerializeField] private float _maxRotationDot;
        
        [SerializeField] private string _speedRatioParameter = "Speed";

        [SerializeField] private float _minMoveRatio = 0.16f;
        private Animator Animator => _animatorLinker ? _linker.Animator : _animator;

        private const string RotateLeft = "RotateLeft";
        private const string RotateRight = "RotateRight";
        
        private void OnEnable()
        {
            _characterMovement.Handler.OnMove += OnMove;
            _characterMovement.Handler.OnStopMove += OnStopMove;
        }
        
        private void OnDisable()
        {
            _characterMovement.Handler.OnMove -= OnMove;
            _characterMovement.Handler.OnStopMove -= OnStopMove;
        }

        private void OnMove(Vector3 input)
        {
            float maxRatio = _characterMovement.ActualSpeed / _maxSpeed;
            SetRatio(Mathf.Clamp(input.magnitude, _minMoveRatio, maxRatio));
            Quaternion rotation = Quaternion.LookRotation(input, Vector3.up);
            float dot = Quaternion.Dot(rotation, _characterMovement.RotateModel.rotation);
            Debug.Log(dot);
            
            if (Mathf.Abs(dot) > _maxRotationDot)
            {
                SetRotate();
                return;
            }
            
            SetRotate(dot > 0, dot < 0, 1-Mathf.Abs(dot));
            
        }

        private void SetRotate(bool left = false, bool right = false, float weight = 0.0f)
        {
            Animator.SetLayerWeight(1, weight);
            Animator.SetBool(RotateLeft, left);
            Animator.SetBool(RotateRight, right);
        }

        private void OnStopMove()
        {
            SetRatio(0);
            SetRotate();
        }
        

        private void SetRatio(float ratio)
        {
            Animator.SetFloat(_speedRatioParameter, Mathf.Clamp(ratio, 0, 1));
        }
    }
}