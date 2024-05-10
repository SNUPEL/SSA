using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvReader : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        String _url = "";
        StreamReader _streamReader = new StreamReader(_url);

        while (!_streamReader.EndOfStream)
        {

            // 한 줄씩 읽어온다.

            //string line = sr.ReadLine();

            // 쉼표( , )를 기준으로 데이터를 분리한다.

            //string[] data = line.Split(',');

            // 결과를 출력해본다.

            //Console.WriteLine("{0}, {1}, {2}, ... ", data[0], data[1], data[2], ... );

        }
    }

    private void readFriendShipData()
    {

    }

    private void readFoeShipData()
    {

    }

    private void readFriendSamData()
    {

    }

    private void readFoeSsmData()
    {

    }
}
