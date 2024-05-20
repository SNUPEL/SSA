using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (mLastingTime < timeStamp)
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
}
