using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{

    public GameObject mSeekerFov;
    [Range(0f, 60f)]
    public float mRange;
    private float mDegree = 0;
    private bool isForward = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<Missile>().Mode.ToLower() == "brm")
        {
            Vector3 _from = new Vector3(mSeekerFov.transform.position.x, 0, mSeekerFov.transform.position.z);
            Vector3 _to = SimulationManager.GetMissile(this.gameObject.GetComponent<Missile>().Target).transform.position;
            _to.y = 0f;
            
            //mSeekerFov.transform.LookAt(_to);
            mSeekerFov.transform.LookAt(new Vector3(_to.x, 0f, _to.z));
            mSeekerFov.transform.position.Set(mSeekerFov.transform.position.x, 64.5f, mSeekerFov.transform.position.z);
            return;
        } else
            homing();
        
    }

    /// <summary>
    /// 미사일의 호밍 애니메이션을 구현
    /// </summary>
    private void homing()
    {
        float _deltaDegree = isForward? 0.1f:-0.1f;

            mSeekerFov.transform.Rotate(0, _deltaDegree, 0);
            mDegree += _deltaDegree;

        if (mDegree >= mRange / 2)
            isForward = false;
        else if (mDegree <= -mRange / 2)
            isForward = true;
    }

    public void Reset()
    {
        mSeekerFov.transform.LookAt(new Vector3(0f, 1f, 0f));
    }
}
