using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Reproduction : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject spawnObj;
    List<GameObject> objects;
    Recorder recorder;
    float delayTime = 0.5f;
    #endregion

    #region Events
    public event Action RemoveObjects;
    #endregion

    #region Core Methods
    void OnEnable()
    {
        recorder = FindObjectOfType<Recorder>();
        objects = new List<GameObject>();
    }
    #endregion

    #region Private Methods
    void DestroyObjects()
    {
        foreach(var obj in objects)
        {
            Destroy(obj);
        }
    }

    IEnumerator InstantiateRoutine()
    {
        foreach(var obj in recorder.ObjectsDictionary)
        {
            GameObject instance = Instantiate(spawnObj, obj.Value.Positions[0].position, obj.Value.Positions[0].rotation);
            objects.Add(instance);
            ReplayObject replayObject = instance.GetComponent<ReplayObject>();
            replayObject.SetMovementInfo(obj.Value.Positions);
            replayObject.StartMovement();
            yield return new WaitForSeconds(delayTime);
        }
    }
    #endregion

    #region Public Methods
    public void Replay()
    {
        DestroyObjects();
        RemoveObjects();
        if (recorder.ObjectsDictionary.Count > 0)
        {
            StartCoroutine(InstantiateRoutine());
        }
    }
    #endregion
}
