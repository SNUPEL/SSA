using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public string mId;
    private int mInterval = SimulationManager.Interval;

    public string Id
    {
        get { return mId; }
        set { }
    }

    public Dictionary<int, Vector3> Locations
    {
        get { return mLocations; }
        set { mLocations = value; }
    }

    public int MaxTimeStamp
    {
        get
        {
            return mLocations.Last().Key;
        }
        set { }
    }

    private Dictionary<int ,Vector3> mLocations = new Dictionary<int, Vector3>();

    public Ship id(string id)
    {
        this.mId = id;
        return this;
    }

    public Ship addLocation(int timeStamp, Vector3 location)
    {
        mLocations.Add(timeStamp, location);
        return this;
    }

    public void move(int timeStamp, float deltaTime)
    {
        if (mLocations.Last().Key == timeStamp)
        {
            this.gameObject.transform.position = mLocations.First().Value;
            this.gameObject.SetActive(false);
            return;
        }

        if (!mLocations.ContainsKey(timeStamp))
        {
            this.gameObject.SetActive(false);
            return;
        }
        if (mLocations.ContainsKey(timeStamp) && mLocations.ContainsKey(timeStamp + mInterval))
        {
            this.gameObject.SetActive(true);
            this.transform.position += (mLocations[timeStamp + mInterval] - mLocations[timeStamp]) / (mInterval / deltaTime);
            this.transform.rotation = Quaternion.LookRotation(mLocations[timeStamp + mInterval] - mLocations[timeStamp]);
        }
    }
}
