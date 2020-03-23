using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct KeyData
{
    public enum EventType
    {
        DOWN,
        UP,
        HELD
    }

    public KeyCode m_key;
    public float m_time;
    public EventType m_eventType;

    public KeyData(KeyCode key, float time, EventType eventType)
    {
        m_key = key;
        m_time = time;
        m_eventType = eventType;
    }

    public override string ToString()
    {
        return m_eventType + "  " + m_key + "  " + m_time;
    }
}

[System.Serializable]
public class KeyDataObject
{
    //Object to use so we can save out the list to json
    //cant serialise a list bt we can serialise the object with a list in it
    public List<KeyData> m_keyList;
}

public class InputCapture : MonoBehaviour
{
    const string FILE_PATH = "Assets/Resources/";

    Array m_allKeyCodes;

    List<KeyData> m_keyDatas = new List<KeyData>();

    float m_time;

    void Awake()
    {
        m_allKeyCodes = Enum.GetValues(typeof(KeyCode));
    }

    private void Update()
    {
        foreach (KeyCode tempKey in m_allKeyCodes)
        {
            if (Input.GetKeyDown(tempKey))
            {
                RegKey(tempKey, m_time, KeyData.EventType.DOWN);
            }
        }

        foreach (KeyCode tempKey in m_allKeyCodes)
        {
            if (Input.GetKeyUp(tempKey))
            {
                RegKey(tempKey, m_time, KeyData.EventType.UP);
            }
        }

        foreach (KeyCode tempKey in m_allKeyCodes)
        {
            if (Input.GetKey(tempKey))
            {
                RegKey(tempKey, m_time, KeyData.EventType.HELD);
            }
        }
        m_time += Time.deltaTime;
    }

    public bool DoesFileExist(string fileName)
    {
        return File.Exists(FILE_PATH + fileName + ".txt");
    }

    public void SaveEvents(string fileName)
    {
        try
        {
            string filePath = FILE_PATH + fileName + ".txt";
            KeyDataObject data = new KeyDataObject();
            data.m_keyList = m_keyDatas;
            string dataString = JsonUtility.ToJson(data);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            StreamWriter writer = new StreamWriter(filePath, false);
            writer.Write(dataString);
            writer.Close();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private void RegKey(KeyCode key, float time, KeyData.EventType eventType)
    {
        m_keyDatas.Add(new KeyData(key, time, eventType));
    }
}