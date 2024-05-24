using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Seeker))]
public class SeekerFOVEditor : Editor
{

    private void OnSceneGUI()
    {
        Seeker _seeker = (Seeker)target;

        Vector3 _from = _seeker.transform.position;
        Vector3 _to = new Vector3();
        if (SimulationManager.GetMissile(_seeker.transform.parent.gameObject.GetComponent<Missile>().Target) != null)
            _to = SimulationManager.GetMissile(_seeker.transform.parent.gameObject.GetComponent<Missile>().Target).transform.position;

        Handles.color = Color.yellow;
        if (_seeker.transform.parent.gameObject.GetComponent<Missile>().Mode.ToLower() == "brm")
            Handles.color = Color.red;
        Handles.DrawLine(_from, _to);
        Handles.Label(_seeker.transform.position, "Forward: " + _seeker.transform.forward + "\nVector: " + (_to - _from).normalized);
    }
}
