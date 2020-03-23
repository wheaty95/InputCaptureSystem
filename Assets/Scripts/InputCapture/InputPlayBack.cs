using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InputEvent : UnityEvent<KeyData>
{
}

public class InputPlayBack : MonoBehaviour
{
    const string FILE_PATH = "Assets/Resources/";
    [SerializeField] private InputEvent m_keyDown;
    [SerializeField] private InputEvent m_keyUp;
    [SerializeField] private InputEvent m_keyHeld;
    [SerializeField] private UnityEvent m_finished;
    [SerializeField] private UnityEvent m_paused;

    List<KeyData> m_playBackData = new List<KeyData>();

    float m_time;
    int m_currentDataTime;
    bool m_isPlaying = false;
    bool m_hasFinished = false;


    public void Config(string fileName, Action<KeyData> keyDown = null, Action<KeyData> keyUp = null, Action<KeyData> keyHeld = null, Action finished = null, Action paused = null, bool start = true)
    {
        m_keyDown.AddListener((KeyData keyData) => keyDown?.Invoke(keyData));
        m_keyUp.AddListener((KeyData keyData) => keyUp?.Invoke(keyData));
        m_keyHeld.AddListener((KeyData keyData) => keyHeld?.Invoke(keyData));

        m_finished.AddListener(()=> finished());
        m_paused.AddListener(()=> paused());

        Config(fileName, start);
    }

    public void Config(string fileName, bool start = true)
    {
        m_playBackData = Read(fileName);

        if (m_playBackData.Count > 0 && start)
        {
            m_isPlaying = true;
        }
    }

    public List<KeyData> Read(string fileName)
    {
        try
        {
            string filePath = FILE_PATH + fileName + ".txt";
            StreamReader reader = new StreamReader(filePath);
            string dataString = reader.ReadToEnd();
            KeyDataObject data = JsonUtility.FromJson<KeyDataObject>(dataString);
            reader.Close();
            return data.m_keyList;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        return new List<KeyData>();
    }

    private void Update()
    {
        if (m_isPlaying)
        {
            if (m_currentDataTime < m_playBackData.Count)
            {
                int index = m_currentDataTime;
                KeyData data = m_playBackData[index];
                bool stillValid;
                do
                {
                    if (m_time >= data.m_time)
                    {
                        switch (data.m_eventType)
                        {
                            case KeyData.EventType.DOWN:
                                m_keyDown?.Invoke(data);
                                break;
                            case KeyData.EventType.UP:
                                m_keyUp?.Invoke(data);
                                break;
                            case KeyData.EventType.HELD:
                                m_keyHeld?.Invoke(data);
                                break;
                        }
                        index++;
                        stillValid = index < m_playBackData.Count;
                        if (stillValid)
                        {
                            data = m_playBackData[index];
                        }
                    }
                    else
                    {
                        stillValid = false;
                    }
                }
                while (stillValid);
                m_currentDataTime = index;
            }
            else
            {
                if (!HasFinished())
                {
                    m_finished?.Invoke();
                    m_hasFinished = true;
                    m_isPlaying = false;
                }
            }
            m_time += Time.deltaTime;
        }
    }

    public bool IsPlaying()
    {
        return m_isPlaying;
    }

    public bool HasFinished()
    {
        return m_hasFinished;
    }

    public void Pause()
    {
        m_paused?.Invoke();
        m_isPlaying = false;
    }

    public void Stop()
    {
        m_isPlaying = false;
        m_time = 0;
    }
}