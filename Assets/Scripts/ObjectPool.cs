using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool mInstance;

    [SerializeField]
    private GameObject poolingObject;

    Queue<Bullet> mPoolingObjectQueue = new Queue<Bullet>();

    private void Awake()
    {
        mInstance = this;
        Initialize(20);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
            mPoolingObjectQueue.Enqueue(CreateNewObject());
    }

    private Bullet CreateNewObject()
    {
        var _object = Instantiate(poolingObject).GetComponent<Bullet>();
        _object.gameObject.SetActive(false);
        _object.transform.SetParent(transform);
        return _object;
    }

    public static Bullet GetObject()
    {
        if (mInstance.mPoolingObjectQueue.Count > 0)
        {
            var _object = mInstance.mPoolingObjectQueue.Dequeue();
            _object.transform.SetParent(null);
            _object.gameObject.SetActive(true);
            return _object;
        } else
        {
            var _object = mInstance.CreateNewObject();
            _object.gameObject.SetActive(true);
            _object.transform.SetParent(null);
            return _object;
        }
    }

    public static void ReturnObject(Bullet obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(mInstance.transform);
        mInstance.mPoolingObjectQueue.Enqueue(obj);
    }
}
