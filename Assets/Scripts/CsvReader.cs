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

            // �� �پ� �о�´�.

            //string line = sr.ReadLine();

            // ��ǥ( , )�� �������� �����͸� �и��Ѵ�.

            //string[] data = line.Split(',');

            // ����� ����غ���.

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
