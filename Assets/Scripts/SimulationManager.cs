using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager mInstance;

    private float mDelta = 0f;
    private float mStep = 0.05f;
    // private float mStep = Time.deltaTime;
    private int mTimeStamp = 12;
    private static List<GameObject> mShips = new List<GameObject>();
    private static List<GameObject> mMissiles = new List<GameObject>();
    public static int x_transformFactor = 300;
    public static int z_transformFactor = 300;
    public static int xScaleFactor = 10;
    public static int zScaleFactor = 10;
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
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject ship in mShips)
            ship.GetComponent<Ship>().move(mTimeStamp, mStep);
        foreach (GameObject missile in mMissiles)
            missile.GetComponent<Missile>().move(mTimeStamp, mStep);
        if (mDelta < 2f)
        {
            mDelta += mStep;
            return;
        }
        // 각 GameObject를 다음 위치값으로 옮기고 delta 값을 초기화함
        mTimeStamp += Interval;
        mDelta = 0f;
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
}
