using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlayBack : MonoBehaviour
{
    [SerializeField] private string m_fieldName;

    InputPlayBack m_playback;

    private void Awake()
    {
        m_playback = GetComponent<InputPlayBack>();
        if (m_playback)
        {
            m_playback.Config(m_fieldName, true);
        }
        else
        {
            Debug.LogError("No playback component found.");
        }
    }

    public void Finished()
    {
        Debug.Log("Finished.");
    }

    public void Paused()
    {
        Debug.Log("Paused.");
    }

    public void KeyDown(KeyData keyData)
    {
        Debug.Log(keyData);
    }

    public void KeyUp(KeyData keyData)
    {
        Debug.Log(keyData);
    }

    public void KeyHeld(KeyData keyData)
    {
        Debug.Log(keyData);
    }
}