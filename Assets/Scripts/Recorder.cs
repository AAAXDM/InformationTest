using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

public class Recorder : MonoBehaviour
{
    #region Fields
    Dictionary<GameObject, ObjectInfo> objectsDictionary;
    Text[] texts;
    ObjectSpawner objectSpawner;
    Reproduction reproduction;
    int maxSamePositions = 6;
    float recordTime = 0.1f;
    bool isRecorded;
    #endregion

    #region Properties
    public IReadOnlyDictionary<GameObject, ObjectInfo> ObjectsDictionary => objectsDictionary;
    #endregion

    #region Events
    public event Action ChangeRecordingState;
    #endregion

    #region Core Methods
    void Awake()
    {
        texts = GetComponentsInChildren<Text>();
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        reproduction = FindObjectOfType<Reproduction>();
        objectsDictionary = new Dictionary<GameObject, ObjectInfo>();
        texts[1].gameObject.SetActive(false);
        isRecorded = false;    
    }

    void OnEnable()
    {
        objectSpawner.Spawn += AddNewObject;
    }

    void OnDisable()
    {
        objectSpawner.Spawn -= AddNewObject;
    }
    #endregion

    #region Private Methods
    void Recording()
    {
        reproduction.gameObject.SetActive(false);
        objectsDictionary.Clear();
        List<GameObject> objects = FindObjectsOfType<Rigidbody>().Select(x => x.gameObject).ToList();

        for(int i = 0; i < objects.Count; i++)
        {
            objectsDictionary.Add(objects[i], new ObjectInfo());
        }
        texts[0].gameObject.SetActive(false);
        texts[1].gameObject.SetActive(true);
        StartCoroutine(RecordingRoutine());
    }

    void StopRecord()
    {
        reproduction.gameObject.SetActive(true);
        texts[0].gameObject.SetActive(true);
        texts[1].gameObject.SetActive(false);
        StopAllCoroutines();
    }

    void AddNewObject(GameObject gameObject)
    {
        objectsDictionary.Add(gameObject, new ObjectInfo());
    }

    IEnumerator RecordingRoutine()
    {
        while (isRecorded)
        {
            foreach (var obj in objectsDictionary)
            {
                if (obj.Value.SamePositionsCount < maxSamePositions)
                {
                    obj.Value.AddTransform(obj.Key.transform.position, obj.Key.transform.rotation);
                    int count = obj.Value.Positions.Count;
                    if (count > 1)
                    {
                        if (obj.Value.Positions[count - 2].Comparsion(obj.Value.Positions[count - 1]))
                        {
                            obj.Value.PlusSamePosition();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(recordTime);
        }
    }
    #endregion

    #region Public Methods
    public void ButtonDown()
    {
        isRecorded = !isRecorded;
        ChangeRecordingState();
        if (isRecorded) Recording();
        else StopRecord();
    }
    #endregion
}
