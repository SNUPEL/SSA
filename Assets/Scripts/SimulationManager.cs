using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 미사일 피격성 평가 시뮬레이션을 관리하는 매니저
/// </summary>
public class SimulationManager : MonoBehaviour
{
    /// <summary>
    /// 싱글턴 패턴 인스턴스
    /// </summary>
    public static SimulationManager mInstance;

    private float mDelta = 0f;

    [Range(0.02f, 0.3f)]
    public float mStep = 0.05f;
    [Range(0, 400f)]
    public float mtranslated = 40f;

    public static Vector3 mTranslated = Vector3.zero;
    // private float mStep = Time.deltaTime;
    private int mTimeStamp = 12;    // 시작 Timestamp를 12로 설정함(빠른 디버깅을 위한 설정)
    private int mMaxTimeStamp = 0;

    private static List<GameObject> mShips = new List<GameObject>();
    private static List<GameObject> mMissiles = new List<GameObject>();
    private static List<GameObject> mDecoys = new List<GameObject>();
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
        GetComponent<ShipBuilder>().Build("Assets//Input//20240509_v1//fri_ship.csv");
        GetComponent<ShipBuilder>().Build("Assets//Input//20240509_v1//foe_ship.csv");
        GetComponent<MissileBuilder>().Build("Assets//Input//20240509_v1//fri_sam.csv");
        //GetComponent<MissileBuilder>().Build("Assets//Input//20240509_v1//fri_ssm.csv");
        GetComponent<MissileBuilder>().Build("Assets//Input//20240509_v1//foe_ssm.csv");
        //GetComponent<MissileBuilder>().Build("Assets//Input//20240509_v1//foe_sam.csv");
        GetComponent<DecoyBuilder>().Build("Assets//Input//20240509_v1//decoy.csv");

        CalculateMaxTimeStamp();
        ScatterFoeShips(mtranslated);
        
    }

    /// <summary>
    /// 입력 파일(로그) 중에서 가장 큰 Timestamp를 탐색합니다.
    /// 시뮬레이션이 완료되면 다시 재시작하도록 하기 위함입니다.
    /// </summary>
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
        foreach (GameObject decoy in mDecoys)
            decoy.GetComponent<Decoy>().move(mTimeStamp);

        
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

    /// <summary>
    /// 전체 게임오브젝트의 위치를 스케일링하여 가시성을 높입니다.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static Vector3 translate(Vector3 location)
    {
        return new Vector3((location.x + x_transformFactor) * xScaleFactor, location.y, (location.z + z_transformFactor) * zScaleFactor);
    }

    /// <summary>
    /// 입력 파일 읽기 과정에 새로운 함대가 존재하면 함대 리스트에 추가합니다.
    /// </summary>
    /// <param name="ship"></param>
    public static void AddShip(GameObject ship)
    {
        mShips.Add(ship);
    }

    /// <summary>
    /// 입력 파일 읽기 과정에 새로운 미사일이 존재하면 미사일 리스트에 추가합니다.
    /// </summary>
    /// <param name="missile"></param>
    public static void AddMissile(GameObject missile)
    {
        mMissiles.Add(missile);
    }

    /// <summary>
    /// 입력 파일 읽기 과정에 새로운 Decoy가 존재하면 Decoy 리스트에 추가합니다.
    /// </summary>
    /// <param name="decoy"></param>
    public static void AddDecoy(GameObject decoy)
    {
        mDecoys.Add(decoy);
    }

    /// <summary>
    /// ID가 동일한 미사일을 탐색합니다.
    /// </summary>
    /// <param name="id">미사일 ID</param>
    /// <returns>해당 미사일 게임 오브젝트</returns>
    public static GameObject GetMissile(string id)
    {
        if (mMissiles == null) 
            return null;
        return mMissiles.Find(missile => missile.name == id);
    }

    /// <summary>
    /// 함대들 사이의 간격을 임의로 넓힙니다.
    /// </summary>
    public static void ScatterFoeShips(float translated)
    {
        Vector3 _center = new Vector3();
        //float _scaleFactor = 20f;
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
       // mTranslated = 
       foreach (GameObject ship in mShips)
        {
            if (ship.name != "Foe Ship") continue;
            Dictionary<int, Vector3> _scaled = new Dictionary<int, Vector3>();
            Vector3 _translatedVector = (ship.transform.position - _center) * translated;
            foreach (var location in ship.GetComponent<Ship>().Locations)
            {
                _scaled.Add(location.Key, location.Value + _translatedVector);
                //Vector3 _scaledLocation = location.Value;
                //_scaledLocation.x = ((location.Value - _center) * _scaleFactor + _center).x;
                //_scaledLocation.z = ((location.Value - _center) * _scaleFactor + _center).z;
                //_scaled.Add(location.Key, _scaledLocation);
            }
            ship.GetComponent<Ship>().Locations = _scaled;
            ship.transform.position = ship.GetComponent<Ship>().Locations.First().Value;
        }
    }

    /// <summary>
    /// Decoy의 위치를 임의로 넓힙니다.
    /// </summary>
    public static void ScaleDecoy()
    {
        foreach (var decoy in mDecoys)
        {
            decoy.GetComponent<Decoy>().scaleLocation();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a">a에 대한 파라미터 설명</param>
    /// <param name="b">b에 대한 파라미터 설명</param>
    /// <param name="c"></param>
    /// <returns></returns>
    /// @note 주의할 사항이 있습니다.
    /// @warning 주의할 사항이 있습니다.
    /// 
    public bool ViewSomething(int a, int b, int c)
    {
        bool _result = true;
        return _result;
    }
}
