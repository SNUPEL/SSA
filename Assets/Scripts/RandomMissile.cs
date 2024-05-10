using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMissile : MonoBehaviour
{

    public float mSpeed;
    private float mTimeLimit = 3.0f;
    private float mTimer = 0f;
    private List<Vector3> mTargets = new List<Vector3>();
    private int mIndex = 0;
    private int mIndex2 = 0;            // 회전 인덱스
    private int NumberOfTargets = 20;

    public float mDistance;
    public Vector3 mTarget;

    private string mId;
    private string mShipId;
    private string mCla = string.Empty;
    private Dictionary<int, Vector3> mLocations = new Dictionary<int, Vector3>();

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

    public Dictionary<int, Vector3> Locations
    {
        get
        {
            return mLocations;
        }
        set
        {
            mLocations = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetRandomTargets();
        mSpeed = Random.Range(30, 100);
        this.transform.forward = mTargets[mIndex];
        mTarget = mTargets[mIndex];
    }

    private void GetRandomTargets()
    {
        mTargets.Clear();
        for (int i = 0; i < NumberOfTargets; i++)
            mTargets.Add(new Vector3(Random.Range(-2000, 2000), Random.Range(0, 2000), Random.Range(-2000, 2000)));
    }

    public void move(int timeStamp)
    {
        if (mLocations.ContainsKey(timeStamp))
            this.transform.position = mLocations[timeStamp];
    }


    // Update is called once per frame
    void Update()
    {
        mDistance = Vector3.Distance(transform.position, mTarget);
        if (mDistance < 400)
        {
            if (mIndex >= mTargets.Count - 1)
            {

                GetRandomTargets();
                mIndex = 0;
                this.transform.forward = mTargets[mIndex];
                return;
            }
            mIndex++;
            mTarget = mTargets[mIndex];
        }

        if (Vector3.Angle(mTarget - transform.position, this.transform.forward) >= 5.0f)
            this.transform.forward = (this.transform.forward * 100000 + mTarget - this.transform.position) / 2;
        // 방향을 한 번에 바꾸는 로직
        // this.transform.rotation = Quaternion.LookRotation(mTarget - this.transform.position).normalized;

        transform.position += Vector3.Normalize(mTarget - transform.position) * mSpeed / 100;
    }
}
