using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CamereControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraController cameraController = (CameraController)target;

        if (GUILayout.Button("Reset Camera Position"))
        {
            cameraController.transform.position = new Vector3(-0.39f, 4.95f, -41.25f);
            cameraController.transform.rotation = Quaternion.Euler(0.082f, -1.293f, 0f);
        }
    }
}
