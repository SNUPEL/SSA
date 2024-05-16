using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mMissileCamera;
    public Camera mFoeShipCamera;
    public Camera mFriendShipCamera;
    public Camera mExplosionCamera;
    public Camera mMainCamera;

    // Start is called before the first frame update
    void Start()
    {
        SimulationManager.AddMissile(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
