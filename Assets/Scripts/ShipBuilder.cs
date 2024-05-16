using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ShipBuilder : MonoBehaviour
{
    private bool isFirstLine = true;
    public GameObject mShipPrefab;
    private List<Ship> mShips = new List<Ship>();

    public void Build(string url)
    {
        StreamReader _streamReader = new StreamReader(url);

        int _timeStamp = 0;
        int _shipId = 1;
        int _ship_x = 2;
        int _ship_y = 3;
        float _shipHeight = -2.5f;
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

            if (mShips.Exists(ship => ship.Id == _data[_shipId])) 
            {
                Vector3 _originalPosition = new Vector3(float.Parse(_data[_ship_x]), _shipHeight, float.Parse(_data[_ship_y]));
                mShips.Find(ship => ship.Id == _data[_shipId]).addLocation(int.Parse(_data[_timeStamp]), SimulationManager.translate(_originalPosition));
            } 
            else
                mShips.Add(new Ship().id(_data[_shipId]).addLocation(int.Parse(_data[_timeStamp]), SimulationManager.translate(new Vector3(float.Parse(_data[_ship_x]), _shipHeight, float.Parse(_data[_ship_y])))));
        }

        foreach (Ship ship in mShips)
        {
            GameObject _ship = Instantiate(mShipPrefab);
            if (url.ToLower().Contains("foe"))
                _ship.name = "Foe Ship";
            else
                _ship.name = "Friend Ship";
            _ship.AddComponent<Ship>();
            _ship.GetComponent<Ship>().id(ship.Id);
            _ship.GetComponent<Ship>().Locations = ship.Locations;
            _ship.SetActive(false);
            _ship.transform.position = _ship.GetComponent<Ship>().Locations.First().Value;
            SimulationManager.AddShip(_ship);
        }
        SimulationManager.ScatterFoeShips();
        mShips.Clear();
        isFirstLine = true;
    }
}
