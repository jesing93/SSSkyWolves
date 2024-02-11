using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform target;
    private PlayerCameraData data;
    public void SetUp(Transform target, PlayerCameraData data)
    {
        this.target = target;
        this.data = data;
        this.AddComponent<Camera>();
        transform.position = target.position + data.offset + target.TransformDirection(data.localOffset);
        transform.LookAt(target.transform.position + target.TransformDirection(data.localOffset));
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position + data.offset + target.TransformDirection(data.localOffset), Vector3.Distance(transform.position, target.transform.position + data.offset + target.TransformDirection(data.localOffset)) * data.tensor * Time.deltaTime);
    }
}
