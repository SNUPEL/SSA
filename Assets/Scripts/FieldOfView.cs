using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/// <summary>
/// CIWS �þ߰� ���� Ŭ����
/// </summary>
public class FieldOfView : MonoBehaviour
{
    /// <summary>
    /// Ž���� ������ �����մϴ�.
    /// </summary>
    public float mRadius = 80f;
    [Range(0f, 270f)]
    public float mAngle = 270;
    public LayerMask targetMask;


    // �ӽ÷� ������ ����(1�� ���� �ϳ��� �̻��Ͽ� ����ϴ� �������)
    private float mDeltaTime = 0f;
    private List<String> mShootedMissile = new List<string>();

    private void Start()
    {
        SetTargetMask();
        StartCoroutine(FovRoutine());
    }

    /// <summary>
    /// Ž���� ���� ������Ʈ�� �����մϴ�.
    /// <br>Foe Ship ���� ������Ʈ�� 'Friend Missile'�� Ž���Ǹ� CIWS�� �߻�ǵ���,</br>
    /// <br>Friend Ship ���� ������Ʈ�� 'Foe Missile'�� Ž���Ǹ� CIWS�� �߻�ǵ��� �����մϴ�.</br>
    /// </summary>
    /// @note ������ ����!
    private void SetTargetMask()
    {
        if (this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Foe Ship"))
            targetMask = LayerMask.NameToLayer("Friend Missile");
        else if (this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Friend Ship"))
            targetMask = LayerMask.NameToLayer("Foe Missile");

    }

    /// <summary>
    /// 0.06�ʸ��� üũ�մϴ�.
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
    /// ������ �̻��� ����Ʈ�� ����, �����ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="colliders"> �浹 ���� ����Ʈ�Դϴ�.</param>
    /// <returns>�ε����� �����մϴ�.</returns>
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
