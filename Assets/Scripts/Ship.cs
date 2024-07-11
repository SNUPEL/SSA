using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

/// <summary>
/// 선박 관리 클래스
/// </summary>
public class Ship : MonoBehaviour
{
    public string mId;
    private int mInterval = SimulationManager.Interval;
    private Dictionary<int, Vector3> mLocations;
    private bool mIsFirst = true;

    public string Id
    {
        get { return mId; }
        set { }
    }

    public Dictionary<int, Vector3> Locations
    {
        get {
            if (mLocations == null)
                mLocations = new Dictionary<int, Vector3>();
            return mLocations; 
        }
        set {
            mLocations = value; 
        }
    }

    public int MaxTimeStamp
    {
        get
        {
            return mLocations.Last().Key;
        }
        set { }
    }

    public Ship id(string id)
    {
        this.mId = id;
        return this;
    }

    public Ship addLocation(int timeStamp, Vector3 location)
    {
        Locations.Add(timeStamp, location);
        return this;
    }

    public void move(int timeStamp, float deltaTime)
    {
        if (mLocations.Last().Key == timeStamp)
        {
            //this.gameObject.transform.position = mLocations.First().Value;
            mIsFirst = true;
            //this.gameObject.SetActive(false);
            return;
        }

        if (mIsFirst && timeStamp == 0)
        {
            this.gameObject.transform.position = mLocations.First().Value;
            mIsFirst = false;
        }

        //if (!mLocations.ContainsKey(timeStamp))
        //{
        //    this.gameObject.SetActive(false);
        //    return;
        //}
        if (mLocations.ContainsKey(timeStamp) && mLocations.ContainsKey(timeStamp + mInterval))
        {
            this.gameObject.SetActive(true);
            this.transform.position += (mLocations[timeStamp + mInterval] - mLocations[timeStamp]) / (mInterval / deltaTime);
            this.transform.rotation = Quaternion.LookRotation(mLocations[timeStamp + mInterval] - mLocations[timeStamp]);
        }
    }
}
