using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectSpawner : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject spawnObj;
    [SerializeField] float planeSize;
    List<GameObject> objectsinScene;
    Reproduction reproduction;
    Recorder recorder;
    float y = 5;
    bool isRecorded;
    #endregion

    #region Events
    public event Action<GameObject> Spawn;
    #endregion

    #region CoreMethods
    void OnEnable()
    {
        recorder = FindObjectOfType<Recorder>();
        reproduction = FindObjectOfType<Reproduction>();
        recorder.ChangeRecordingState += ChangeState;
        reproduction.RemoveObjects += RemoveObjects;
        objectsinScene = new List<GameObject>();
        isRecorded = false;    
    }

    void OnDisable()
    {
        recorder.ChangeRecordingState -= ChangeState;
        reproduction.RemoveObjects -= RemoveObjects;
    }
    #endregion

    #region Private Methods
    void ChangeState() => isRecorded = !isRecorded;

    void RemoveObjects()
    {
        foreach(var obj in objectsinScene)
        {
            Destroy(obj);
        }
    }
    #endregion

    #region Public Methods
    public void SpawnObject()
    {
        float x = UnityEngine.Random.Range(-planeSize, planeSize);
        float z = UnityEngine.Random.Range(-planeSize, planeSize);
        Vector3 position = new Vector3(x, y, z);
        GameObject obj = Instantiate(spawnObj, position, Quaternion.identity);
        objectsinScene.Add(obj);
        if(isRecorded)
        {
            Spawn(obj);
        }
    }
    #endregion
}