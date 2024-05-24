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

        Handles.Label(_fieldOfView.transform.position + Vector3.up, _fieldOfView.transform.position.ToString());

        // forward ∫§≈Õ «•Ω√
        Handles.color = Color.green;
        Handles.DrawLine(_fieldOfView.transform.position, _fieldOfView.transform.position + _fieldOfView.transform.parent.gameObject.transform.forward * 3, 3);
        Handles.Label(_fieldOfView.transform.position, _fieldOfView.transform.parent.name + ": " +_fieldOfView.transform.parent.gameObject.transform.forward.ToString());

        float newAmount = Handles.ScaleValueHandle(Mathf.Clamp01(1), _fieldOfView.transform.position, Quaternion.identity, 5, Handles.ArrowHandleCap, 0.1f);


        foreach (var collider in _fieldOfView.GetColliders())
        {
            Handles.color = Color.blue;
            Vector3 _directionToTarget = (collider.transform.position - _fieldOfView.transform.position).normalized;
            Handles.Label((collider.transform.position + _fieldOfView.transform.position) / 2 + Vector3.up, collider.transform.position.ToString() + "\nVector: " + _directionToTarget + "\nDegree: " + Vector3.Angle(_fieldOfView.transform.parent.gameObject.transform.forward, _directionToTarget));
            Handles.DrawLine(_fieldOfView.transform.position, collider.transform.position);

        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
