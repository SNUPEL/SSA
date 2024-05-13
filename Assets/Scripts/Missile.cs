using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public ParticleSystem mParticleSystem;

    private string mId = string.Empty;
    private string mShipId = string.Empty;
    private string mCla = string.Empty;
    private int mTimeStamp;
    private string mMode = string.Empty;
    public Dictionary<int, Vector3> mLocations = new Dictionary<int, Vector3>();
    private int mInterval = SimulationManager.Interval;
    private Mode mState = global::Mode.INACTIVE;

    public string Id
    {
        get { return mId; }
        set { }
    }

    public string ShipId
    {
        get { return mShipId; } 
        set { }
    }

    public string Cla
    {
        get { return mCla; }
        set { }
    }

    private int TimeStamp
    {
        get { return mTimeStamp; }
        set
        {
            mTimeStamp = value;
        }
    }

    public int MaxTimeStamp
    {
        get
        {
            return mLocations.Last().Key;
        }
        set
        {

        }
    }

    public Dictionary<int, Vector3> Locations { 
        get
        {
            return mLocations;
        } 
        set 
        {
            mLocations = value;
        }
    }
    public Missile id(string id)
    {
        this.mId = id;
        return this;
    }

    public Missile shipId(string shipId)
    {
        this.mShipId = shipId;
        return this;
    }

    public Missile addLocation(int timeStamp, Vector3 location)
    {
        this.mLocations.Add(timeStamp, location);
        return this;
    }

    public Missile Classification(string cla)
    {
        this.mCla = cla;
        return this;
    }

    public Missile Mode(string mode)
    {
        this.Mode(mode);
        return this;
    }

    private Mode State
    {
        get 
        {
            if (!mLocations.ContainsKey(TimeStamp))
                mState = global::Mode.INACTIVE;
            else if (mLocations.Last().Key == TimeStamp)
                mState = global::Mode.STOP;
            else if (mLocations.ContainsKey(TimeStamp) && mLocations.ContainsKey(TimeStamp + mInterval))
                mState = global::Mode.MOVING;
            return mState; 
        }
    } 

    private bool isArrived()
    {
        if (Vector3.Distance(this.gameObject.transform.position, mLocations[TimeStamp + mInterval]) < 0.1)
            return true;
        return false;
    }

    public void move(int timeStamp, float deltaTime)
    {
        TimeStamp = timeStamp;
        switch(State)
        {
            case global::Mode.INACTIVE:
                this.gameObject.SetActive(false);
                this.gameObject.transform.position = mLocations.First().Value;
                return;

            case global::Mode.MOVING:
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                mParticleSystem.transform.parent = this.gameObject.transform;
                if (isArrived())
                    return;
                this.transform.position += (mLocations[timeStamp + mInterval] - mLocations[timeStamp]) / (mInterval / deltaTime);
                if (Vector3.Angle(mLocations[timeStamp + mInterval] - mLocations[timeStamp], this.transform.forward) >= 2.0f)
                    this.transform.forward = (this.transform.forward * 100 + mLocations[timeStamp + mInterval] - mLocations[timeStamp]) / 2;
                this.gameObject.SetActive(true);
                return;

            case global::Mode.STOP:
                mParticleSystem.transform.parent = null;
                mParticleSystem.transform.position = this.transform.position;
                mParticleSystem.Play();
                this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                return;
            default:
                return;

        }
    }
}
