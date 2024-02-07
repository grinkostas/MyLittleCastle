using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttackRotator : MonoBehaviour
{
    [SerializeField] private AttackState _enemyAttack;
    [SerializeField] private Vector3 _lookAxis = Vector3.up;
    [SerializeField, Range(-1, 1)] private float _minDot;
    [SerializeField] private float _rotateSpeed;

    private Transform EnemyTransform => _enemyAttack.StateEnemy.transform;
    public Vector3 TargetPosition => _enemyAttack.StateEnemy.TargetPostion;
    private void Update()
    {
        if(_enemyAttack.enabled == false)
            return;
        Vector3 vector = TargetPosition - EnemyTransform.position;
        if (Vector3.Dot(EnemyTransform.TransformDirection(Vector3.forward), vector) > _minDot)
            return;
        
        Vector3 relativePos = TargetPosition - EnemyTransform.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles;
        targetRotation = Vector3.Scale(_lookAxis, targetRotation);
        targetRotation = Vector3.Lerp(EnemyTransform.transform.rotation.eulerAngles, targetRotation, _rotateSpeed * Time.deltaTime);
        EnemyTransform.rotation = Quaternion.Euler(targetRotation);
    }
}
