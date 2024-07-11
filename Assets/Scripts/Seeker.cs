using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일의 시커를 관리하는 클래스
/// </summary>
public class Seeker : MonoBehaviour
{

    public GameObject mSeekerFov;
    [Range(0f, 60f)]
    private float mRange = 60;
    private float mDegree = 0;
    private bool isForward = true;

    void Update()
    {
        if (this.transform.parent == null) return;

        if (this.transform.parent.gameObject.GetComponent<Missile>().Mode.ToLower() == "brm")
        {
            Vector3 _from = new Vector3(mSeekerFov.transform.position.x, 0, mSeekerFov.transform.position.z);
            Vector3 _to = SimulationManager.GetMissile(this.transform.parent.gameObject.GetComponent<Missile>().Target).transform.position;
            _to.y = 0f;
            mSeekerFov.transform.forward = _to - _from;
            return;
        } else
            homing();
        
    }

    /// <summary>
    /// 미사일의 호밍 애니메이션을 구현
    /// </summary>
    private void homing()
    {
        float _deltaDegree = isForward ? 0.3f : -0.3f;

        mSeekerFov.transform.Rotate(0, _deltaDegree, 0);
        mDegree += _deltaDegree;

        if (mDegree >= mRange / 2)
            isForward = false;
        else if (mDegree <= -mRange / 2)
            isForward = true;
    }

    /// <summary>
    /// 미사일이 폭발된 뒤에 시커의 위치를 초기화하는 함수입니다.
    /// </summary>
    /// @see 회전하는 함수에 대해서는 homing() 함수를, 작동 방법에 대해서는 Update() 함수를 참고하세요. ObjectPool.GetObject()도 참고할 수 있습니다.
    /// @attention 가장 약한 단계의 주의 사항에 대해서 작성
    /// @note 보통 단계의 주의 사항에 대해서 작성
    /// @warning 상위 단계의 주의 사항에 대해서 작성
    public void Reset()
    {
        mSeekerFov.transform.LookAt(new Vector3(0f, 1f, 0f));
    }


}
