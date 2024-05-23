using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView _fieldOfView = (FieldOfView)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(_fieldOfView.transform.position, Vector3.up, Vector3.forward, 360, _fieldOfView.mRadius);

        Vector3 _viewAngles01 = DirectionFromAngle(_fieldOfView.transform.eulerAngles.y, _fieldOfView.mAngle / 2);
        Vector3 _viewAngles02 = DirectionFromAngle(_fieldOfView.transform.eulerAngles.y, -_fieldOfView.mAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(_fieldOfView.transform.position, _fieldOfView.transform.position + _viewAngles01 * _fieldOfView.mRadius);
        Handles.DrawLine(_fieldOfView.transform.position, _fieldOfView.transform.position + _viewAngles02 * _fieldOfView.mRadius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
