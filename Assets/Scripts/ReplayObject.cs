using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ReplayObject : MonoBehaviour
{
    #region Fields
    List<TransformInfo> movementInfo = new List<TransformInfo>();
    int iteration = 1;
    bool canChangePoint = false;
    #endregion

    #region Private Methods
    async void MoveToPoint()
    {
        for (int i = 1; i < movementInfo.Count; i++)
        {
            StartCoroutine(Move());
            while (!canChangePoint)
            {
                await Task.Yield();
            }
            iteration++;
        }
    }

    IEnumerator Move()
    {
        canChangePoint = false;
        while (transform.position != movementInfo[iteration].position)
        {

            transform.position = Vector3.MoveTowards(transform.position, movementInfo[iteration].position, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, movementInfo[iteration].rotation, 1);
            yield return new WaitForFixedUpdate();
        }
        canChangePoint = true;
    }
    #endregion

    #region Public Methods
    public void SetMovementInfo(IReadOnlyList<TransformInfo> transformInfo)
    {
        foreach (var info in transformInfo)
        {
            TransformInfo transformInformation = info;
            movementInfo.Add(transformInformation);
        }
    }
    public void StartMovement()
    {
        MoveToPoint();
    }
    #endregion
}
