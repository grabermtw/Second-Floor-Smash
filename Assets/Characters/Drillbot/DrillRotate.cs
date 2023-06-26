using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillRotate : MonoBehaviour
{
    [SerializeField] private float rotation_DegPerSec;
    [SerializeField] private Transform drillObj;

    private void Update()
    {
        if (drillObj != null)
        {
            drillObj.Rotate(Vector3.right * rotation_DegPerSec * Time.deltaTime, Space.Self);
        }
    }
}
