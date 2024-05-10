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
    private string mMode = string.Empty;
    public Dictionary<int, Vector3> mLocations = new Dictionary<int, Vector3>();
    private int mInterval = SimulationManager.Interval;

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

    public void move(int timeStamp, float deltaTime)
    {
        // 시뮬레이션 타임라인이 존재하지 않는다면 비활성화
        if (!mLocations.ContainsKey(timeStamp))
        {
            this.gameObject.SetActive(false);
            return;
        }

        // 마지막 timeStamp라면 폭발 이펙트 발생
        if (mLocations.Last().Key == timeStamp)
        {
            mParticleSystem.transform.parent = null;
            Destroy(this.gameObject);
            Destroy(mParticleSystem, mParticleSystem.duration);
            mParticleSystem.Play();
        }

        // 움직이는 중인 경우
        if (mLocations.ContainsKey(timeStamp) && mLocations.ContainsKey(timeStamp + mInterval))
        {
            // 이미 목적지에 도착했다면 멈추도록
            if (Vector3.Distance(this.gameObject.transform.position, mLocations[timeStamp + mInterval]) < 0.1)
                return;
            this.transform.position += (mLocations[timeStamp + mInterval] - mLocations[timeStamp]) / (mInterval / deltaTime);

            if (Vector3.Angle(mLocations[timeStamp + mInterval] - mLocations[timeStamp], this.transform.forward) >= 2.0f)
                this.transform.forward = (this.transform.forward * 100 + mLocations[timeStamp + mInterval] - mLocations[timeStamp]) / 2;
            // 한 번에 방향 전환이 되는 로직
            //this.transform.rotation = Quaternion.LookRotation(mLocations[timeStamp + mInterval] - mLocations[timeStamp]);
            this.gameObject.SetActive(true);
        }
    }
}
