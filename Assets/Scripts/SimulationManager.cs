using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager mInstance;

    private float mDelta = 0f;
    private float mStep = 0.1f;
    // private float mStep = Time.deltaTime;
    private int mTimeStamp = 12;
    private int mMaxTimeStamp = 0;

    private static List<GameObject> mShips = new List<GameObject>();
    private static List<GameObject> mMissiles = new List<GameObject>();
    public static int x_transformFactor = 300;
    public static int z_transformFactor = 300;
    public static int xScaleFactor = 1;
    public static int zScaleFactor = 1;
    public static int Interval = 2;

    private void Awake()
    {
        if (mInstance == null)
            mInstance = this;
        else
            Destroy(this.gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ShipBuilder>().Build("Assets//Input//fri_ship.csv");
        GetComponent<ShipBuilder>().Build("Assets//Input//foe_ship.csv");
        GetComponent<MissileBuilder>().Build("Assets//Input//fri_sam.csv");
        GetComponent<MissileBuilder>().Build("Assets//Input//foe_ssm.csv");
        CalculateMaxTimeStamp();
    }

    private void CalculateMaxTimeStamp()
    {
        foreach (GameObject ship in mShips)
            if (ship.GetComponent<Ship>().MaxTimeStamp > mMaxTimeStamp)
                mMaxTimeStamp = ship.GetComponent<Ship>().MaxTimeStamp;
        foreach (GameObject missile in mMissiles)
            if (missile.GetComponent<Missile>().MaxTimeStamp > mMaxTimeStamp)
                mMaxTimeStamp = missile.GetComponent<Missile>().MaxTimeStamp;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject ship in mShips)
            ship.GetComponent<Ship>().move(mTimeStamp, mStep);
        foreach (GameObject missile in mMissiles)
            if (missile != null)
                missile.GetComponent<Missile>().move(mTimeStamp, mStep);
        
        if (mDelta < 2f)
        {
            mDelta += mStep;
            return;
        }
        // 각 GameObject를 다음 위치값으로 옮기고 delta 값을 초기화함
        mTimeStamp += Interval;
        mDelta = 0f;

        if (mTimeStamp > mMaxTimeStamp + 20)
            mTimeStamp = 0;
        
    }

    public static Vector3 translate(Vector3 location)
    {
        return new Vector3((location.x + x_transformFactor) * xScaleFactor, location.y, (location.z + z_transformFactor) * zScaleFactor);
    }

    public static void AddShip(GameObject ship)
    {
        mShips.Add(ship);
    }

    public static void AddMissile(GameObject missile)
    {
        mMissiles.Add(missile);
    }

    public static void ScatterFoeShips()
    {
        Vector3 _center = new Vector3();
        float _scaleFactor = 20f;
        int _count = 0;
       foreach(GameObject ship in mShips)
        {
            if (ship.name == "Foe Ship")
            {
                _center += ship.transform.position;
                _count++;
            }
        }
        _center = _center / _count;
       foreach (GameObject ship in mShips)
        {
            if (ship.name != "Foe Ship") continue;
            Dictionary<int, Vector3> _scaled = new Dictionary<int, Vector3>();
            foreach(var location in ship.GetComponent<Ship>().Locations)
            {
                Vector3 _scaledLocation = location.Value;
                _scaledLocation.x = ((location.Value - _center) * _scaleFactor + _center).x;
                //_scaledLocation.z = ((location.Value - _center) * _scaleFactor + _center).z;
                _scaled.Add(location.Key, _scaledLocation);
            }
            ship.GetComponent<Ship>().Locations = _scaled;
            ship.transform.position = ship.GetComponent<Ship>().Locations.First().Value;
        }
    }
}
