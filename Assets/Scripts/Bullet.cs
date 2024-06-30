using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{

    private Vector3 _direction;
    public void Shoot(Vector3 direction)
    {
        this._direction = direction;
        Invoke("DestroyBullet", 1f);
    }

    public void DestroyBullet()
    {
        ObjectPool.ReturnObject(this);
    }

    private void Update()
    {
        transform.Translate(this._direction);
    }

}
