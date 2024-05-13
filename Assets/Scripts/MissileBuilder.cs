using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MissileBuilder : MonoBehaviour
{

    public GameObject mMissilePrefab;
    private List<Missile> mMissiles = new List<Missile>();
    private bool isFirstLine = true;

    
    public void Build(string url)
    {
        StreamReader _streamReader = new StreamReader(url);

        int _timeStamp = 0;
        int _shipId = 1;
        int _missileId;
        int _missile_x;
        int _missile_y;

        // friend SAM에만 있는 value
        int _mode;
        int _target;
        int _classification;

        int _xScaleFactor = SimulationManager.xScaleFactor;
        int _zScaleFactor = SimulationManager.zScaleFactor;

        while (!_streamReader.EndOfStream)
        {
            string _line = _streamReader.ReadLine();
            if (isFirstLine) 
            { 
                isFirstLine = false; 
                continue; 
            }
            string[] _data = _line.Split(',');
            Missile _missile = new Missile();
            Vector3 _position = new Vector3();
            if (_data.Length == 8)
            {
                // Friend SAM인 경우 (Input 파일 형식이 바뀔 경우 수정 필요)
                _missileId = 4;
                _missile_x = 6;
                _missile_y = 7;
                _mode = 2;
                _target = 3;
                _classification = 5;

                Vector3 _originalPosition = new Vector3(float.Parse(_data[_missile_x]), 8f, float.Parse(_data[_missile_y]));
                _position = SimulationManager.translate(_originalPosition);
                //_position = new Vector3(float.Parse(_data[_missile_x]) * _xScaleFactor, 8f, float.Parse(_data[_missile_y]) * _zScaleFactor);

                _missile.shipId(_data[_shipId]).Classification(_data[_classification]).id(_data[_missileId]).addLocation(int.Parse(_data[_timeStamp]), _position);
            } else
            {
                // Foe SSM인 경우 (Input 파일 형식이 바뀔 경우 수정 필요)
                _missileId = 1;
                _missile_x = 2;
                _missile_y = 3;

                Vector3 _originalPosition = new Vector3(float.Parse(_data[_missile_x]), 8f, float.Parse(_data[_missile_y]));
                _position = SimulationManager.translate(_originalPosition);

                //_position = new Vector3(float.Parse(_data[_missile_x]) * _xScaleFactor, 8f, float.Parse(_data[_missile_y]) * _zScaleFactor);
                _missile.id(_data[_missileId]).addLocation(int.Parse(_data[_timeStamp]), _position);
            }

            if (mMissiles.Exists(x => x.Id == _missile.Id && x.ShipId == _missile.ShipId && x.Id == _missile.Id))
            {
                mMissiles.Find(x => x.Id == _missile.Id && x.ShipId == _missile.ShipId && x.Id == _missile.Id).addLocation(int.Parse(_data[_timeStamp]), _position);
                Destroy(_missile);
            }
            else
                mMissiles.Add(_missile);
        }

        foreach (Missile missile in mMissiles)
        {
            GameObject _missile = Instantiate(mMissilePrefab);
            _missile.name = missile.Id;
            _missile.GetComponent<Missile>().Id = missile.Id;
            _missile.GetComponent<Missile>().shipId(missile.ShipId); 
            _missile.GetComponent<Missile>().Locations = missile.Locations;
            _missile.SetActive(false);
            _missile.transform.position = _missile.GetComponent<Missile>().Locations.First().Value;
            _missile.transform.forward = _missile.GetComponent<Missile>().Locations[_missile.GetComponent<Missile>().Locations.First().Key + SimulationManager.Interval] - _missile.GetComponent<Missile>().Locations[_missile.GetComponent<Missile>().Locations.First().Key];
            SimulationManager.AddMissile(_missile);

            // Hack: Debug를 위한 경로 생성 로직 추가함
            //createMissileWay(_missile, missile.Locations);
        }

        mMissiles.Clear();
        isFirstLine = true;
    }

    private void createMissileWay(GameObject missile, Dictionary<int, Vector3> locations)
    {
        missile.AddComponent<LineRenderer>();
        missile.GetComponent<LineRenderer>().positionCount = locations.Count;
        missile.GetComponent<LineRenderer>().SetPositions(locations.Values.ToArray());
    }
}
