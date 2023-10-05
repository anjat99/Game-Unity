using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Data
    [SerializeField]
    private float xMin;

    [SerializeField]
    private float yMin;

    [SerializeField]
    private float xMax;

    [SerializeField]
    private float yMax;

    private Transform meta;
    #endregion

    void Start()
    {
        meta = GameObject.Find("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(meta.position.x, xMin, xMax), Mathf.Clamp(meta.position.y, yMin, yMax), transform.position.z);
    }
}