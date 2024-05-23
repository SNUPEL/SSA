using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public ParticleSystem mExplosion;
    public ParticleSystem mSmoke;

    private string mId = string.Empty;
    private string mShipId = string.Empty;
    private string mTarget = string.Empty;
    private string mCla = string.Empty;
    private int mTimeStamp;
    private Dictionary<int, string> mModes;
    private string mMode = string.Empty;

    private Dictionary<int, Vector3> mLocations;
    private int mInterval = SimulationManager.Interval;
    private State mState = State.INACTIVE;

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

    public string Target
    {
        get { return mTarget; }
        set { mTarget = value; }
    }

    public string Cla
    {
        get { return mCla; }
        set { }
    }

    public Dictionary<int, string> Modes
    {
        get
        {
            if (mModes == null)
                mModes = new Dictionary<int, string>();
            return mModes;
        } set
        { mModes = value; }
    }

    public string Mode
    {
        get {
            return mMode;
        }
        set { mMode = value; }
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
            return Locations.Last().Key;
        }
        set
        {

        }
    }

    public Dictionary<int, Vector3> Locations {
        get
        {
            if (mLocations == null)
                mLocations = new Dictionary<int, Vector3>();
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

    public Missile setTarget(string target)
    {
        this.mTarget = target;
        return this;
    }

    public Missile addLocation(int timeStamp, Vector3 location)
    {
        Locations.Add(timeStamp, location);
        return this;
    }

    public Missile Classification(string cla)
    {
        this.mCla = cla;
        return this;
    }

    public Missile addMode(int timeStamp, string mode)
    {
        this.Modes.Add(timeStamp, mode);
        return this;
    }

    private State State
    {
        get 
        {
            if (!mLocations.ContainsKey(TimeStamp))
                mState = global::State.INACTIVE;
            else if (mLocations.Last().Key == TimeStamp)
                mState = global::State.STOP;
            else if (mLocations.ContainsKey(TimeStamp) && mLocations.ContainsKey(TimeStamp + mInterval))
                mState = global::State.MOVING;
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
        if (Modes.ContainsKey(timeStamp))
            this.Mode = Modes[timeStamp];
        switch(State)
        {
            case State.INACTIVE:
                this.gameObject.SetActive(false);
                this.gameObject.transform.position = mLocations.First().Value;
                this.gameObject.GetComponent<Seeker>().Reset();
                return;

            case State.MOVING:
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                mExplosion.transform.position = this.gameObject.transform.position;
                mExplosion.transform.parent = this.gameObject.transform;
                mSmoke.transform.position = this.gameObject.transform.position;
                mSmoke.transform.parent = this.gameObject.transform;
                mSmoke.Play();
                if (isArrived())
                    return;
                this.transform.position += (mLocations[timeStamp + mInterval] - mLocations[timeStamp]) / (mInterval / deltaTime);
                if (Vector3.Angle(mLocations[timeStamp + mInterval] - mLocations[timeStamp], this.transform.forward) >= 2.0f)
                    this.transform.forward = (this.transform.forward * 100 + mLocations[timeStamp + mInterval] - mLocations[timeStamp]) / 2;
                this.gameObject.SetActive(true);
                return;

            case State.STOP:
                mExplosion.transform.parent = null;
                mExplosion.Play();
                mSmoke.transform.parent = null;
                mSmoke.Stop();
                this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                return;
            default:
                return;

        }
    }
}
