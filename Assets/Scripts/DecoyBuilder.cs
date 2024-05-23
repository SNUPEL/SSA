using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;

public class DecoyBuilder : ObjectBuilder
{
    public GameObject mDecoyPrefab;
    private List<UnityEngine.Vector2> mLocations = new List<UnityEngine.Vector2>();

    private int mIndexDecoyTime = 0;
    private int mIndexFriendId = 1;
    private int mIndexDecoyX = 2;
    private int mIndexDecoyY = 3;
    private float mHeight = 8f;
    
    public void Build(string url)
    {
        ReadCsv(url);
        foreach (var data in mDatas)
        {
            if (mLocations.Contains(new UnityEngine.Vector2(float.Parse(data[mIndexDecoyX]), float.Parse(data[mIndexDecoyY]))))
                continue;
            mLocations.Add(new UnityEngine.Vector2(float.Parse(data[mIndexDecoyX]), float.Parse(data[mIndexDecoyY])));
            Decoy decoy = new Decoy();
            UnityEngine.Vector3 _location = SimulationManager.translate(new UnityEngine.Vector3(float.Parse(data[mIndexDecoyX]), mHeight, float.Parse(data[mIndexDecoyY])));
            int _startTimeStamp = int.Parse(data[mIndexDecoyTime]);
            decoy.SetLocation(_location).SetStartTimeStamp(_startTimeStamp);

            GameObject _decoy = Instantiate(mDecoyPrefab);
            _decoy.GetComponent<Decoy>().SetLocation(_location).SetStartTimeStamp(_startTimeStamp);
            SimulationManager.AddDecoy(_decoy);
        }
        SimulationManager.ScaleDecoy();
    }
}
