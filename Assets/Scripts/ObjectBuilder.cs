using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{

    private bool isFirstLine = true;
    protected List<string[]> mDatas = new List<string[]>();

    public void ReadCsv(string url)
    {
        
        StreamReader _streamReader = new StreamReader(url);
        while (!_streamReader.EndOfStream)
        {
            string _line = _streamReader.ReadLine();
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }
            string[] _datas = _line.Split(',');
            mDatas.Add(_datas);
        }
        isFirstLine = true;
    }
}