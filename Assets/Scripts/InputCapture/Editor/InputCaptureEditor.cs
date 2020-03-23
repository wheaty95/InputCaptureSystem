using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputCapture))]
public class InputCaptureEditor : Editor
{
    string m_fieldName;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_fieldName = EditorGUILayout.TextField("Field Name", m_fieldName);

        if (GUILayout.Button("Save Input"))
        {
            if (Application.isPlaying)
            {
                InputCapture capture = (InputCapture)target;
                if (capture.DoesFileExist(m_fieldName))
                {
                    int option = EditorUtility.DisplayDialogComplex("File exists.", "The file needs to be deleted", "Delete", "Cancel", "");
                    if (option != 0)
                    {
                        capture.SaveEvents(m_fieldName);
                    }
                }
                else
                {
                    capture.SaveEvents(m_fieldName);
                }
            }
            else
            {
                Debug.Log("Saving only works in play mode");
            }
        }
    }
}