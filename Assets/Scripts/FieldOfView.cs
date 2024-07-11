using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/// <summary>
/// CIWS 시야각 관리 클래스
/// </summary>
public class FieldOfView : MonoBehaviour
{
    /// <summary>
    /// 탐색할 각도를 설정합니다.
    /// </summary>
    public float mRadius = 80f;
    [Range(0f, 270f)]
    public float mAngle = 270;
    public LayerMask targetMask;


    // 임시로 정의한 변수(1초 동안 하나의 미사일에 사격하는 방식으로)
    private float mDeltaTime = 0f;
    private List<String> mShootedMissile = new List<string>();

    private void Start()
    {
        SetTargetMask();
        StartCoroutine(FovRoutine());
    }

    /// <summary>
    /// 탐색할 게임 오브젝트를 설정합니다.
    /// <br>Foe Ship 게임 오브젝트는 'Friend Missile'이 탐색되면 CIWS가 발사되도록,</br>
    /// <br>Friend Ship 게임 오브젝트는 'Foe Missile'이 탐색되면 CIWS가 발사되도록 설정합니다.</br>
    /// </summary>
    /// @note 주의할 사항!
    private void SetTargetMask()
    {
        if (this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Foe Ship"))
            targetMask = LayerMask.NameToLayer("Friend Missile");
        else if (this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Friend Ship"))
            targetMask = LayerMask.NameToLayer("Foe Missile");

    }

    /// <summary>
    /// 0.06초마다 체크합니다.
    /// </summary>
    /// <returns></returns>
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
    /// 격추할 미사일 리스트를 저장, 관리하는 함수입니다.
    /// </summary>
    /// <param name="colliders"> 충돌 검출 리스트입니다.</param>
    /// <returns>인덱스를 리턴합니다.</returns>
    private int SetTarget(Collider[] colliders)
    {
        int a = (int)UnityEngine.Random.Range(0f, colliders.Length);
        return a;
    }

    private void CheckFieldofView()
    {
        Collider[] _collider = Physics.OverlapSphere(transform.position, mRadius, 1 << targetMask);
        if (_collider.Length > 0 )
        {
            Transform _target = _collider[SetTarget(_collider)].transform;
            Vector3 _directionToTarget = (_target.position - transform.position).normalized;
            float _angle = Vector3.Angle(transform.parent.gameObject.transform.forward, _directionToTarget);
            if (_angle < mAngle / 2)
            {
                var _bullet = ObjectPool.GetObject();
                _bullet.transform.position = transform.position;
                var _direction = _target.position - transform.position;
                _bullet.Shoot(_direction.normalized);
            }
        }
    }

    public Collider[] GetColliders() 
    {
        return Physics.OverlapSphere(transform.position, mRadius, 1 << targetMask);
    }
}
