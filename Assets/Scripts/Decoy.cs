using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decoy 게임 오브젝트 관리 클래스
/// </summary>
public class Decoy : MonoBehaviour
{
    public ParticleSystem mFogging;
    private Vector3 mLocation;
    private int mStartTimeStamp;
    private int mEndTimeStamp;
    private int mLastingTime = 18;

    public Decoy SetLocation (Vector3 location)
    {
        mLocation = location;
        return this;
    }

    public Decoy SetStartTimeStamp (int timeStamp)
    {
        mStartTimeStamp = timeStamp;
        mEndTimeStamp = timeStamp + 18;
        return this;
    }

    public int StartTimeStamp
    {
        get { return mStartTimeStamp; }
        set { mStartTimeStamp = value; }
    }

    public void move(int timeStamp)
    {
        if (mEndTimeStamp < timeStamp)
            this.gameObject.SetActive(false);
        else if (timeStamp > mEndTimeStamp)
        {
            mFogging.Stop();
        } else
        {
            mFogging.Play();
            this.gameObject.SetActive(true);
            this.gameObject.transform.position = mLocation;
        }
    }

    /// <summary>
    /// Decoy의 위치를 재조정한다.
    /// </summary>
    public void scaleLocation()
    {
        mLocation.Set(mLocation.x + 20, mLocation.y, mLocation.z);
    }
}
