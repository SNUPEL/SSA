using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    public float mRadius;
    [Range(0f, 270f)]
    public float mAngle;
    public LayerMask targetMask;

    // 임시로 정의한 변수(1초 동안 하나의 미사일에 사격하는 방식으로)
    private float mDeltaTime = 0f;
    private List<String> mShootedMissile = new List<string>();

    private void Start()
    {
        SetTargetMask();
        StartCoroutine(FovRoutine());
    }

    private void SetTargetMask()
    {
        if (this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Foe Ship"))
            targetMask = LayerMask.NameToLayer("Friend Missile");
        else if (this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Friend Ship"))
            targetMask = LayerMask.NameToLayer("Foe Missile");

    }

    private IEnumerator FovRoutine()
    {
        float delay = 0.06f;
        WaitForSeconds _wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return _wait;
            CheckFieldofView();
        }
    }

    /// <summary>
    /// 격추한 미사일 리스트를 저장, 관리하는 함수
    /// </summary>
    private int SetTarget(Collider[] colliders)
    {
        int a = (int)UnityEngine.Random.Range(0f, colliders.Length);
        return a;
    }

    private void CheckFieldofView()
    {
        Collider[] _collider = Physics.OverlapSphere(this.transform.position, mRadius, 1 << targetMask);
        if (_collider.Length > 0 )
        {
            Transform _target = _collider[SetTarget(_collider)].transform;
            Vector3 _directionToTarget = (_target.position - transform.position).normalized;
            float _angle = Vector3.Angle(new Vector3(0, 0, 1), _directionToTarget);
            if (_angle < mAngle / 2)
            {
                var _bullet = ObjectPool.GetObject();
                _bullet.transform.position = this.transform.position;
                var _direction = _target.position - transform.position;
                _bullet.Shoot(_direction.normalized);
            }
        }
    }
}
