using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBodyParts : MonoBehaviour
{
    public Transform HMD, RightController, LeftController;

    public Transform Root, RightHand, LeftHand;

    public Vector3 RootToCamOffset, LeftHandRotationOffset, RightHandRotationOffset;

    void Update()
    {
        EqualizeRootTransform(HMD, Root);

        EqualizeTransform(LeftController, LeftHand, true); 
        EqualizeTransform(RightController, RightHand, false);
    }

    private void EqualizeRootTransform(Transform hmd, Transform root)
    {
        root.transform.position = hmd.transform.position + RootToCamOffset;
    }

    private void EqualizeTransform(Transform first, Transform second, bool isLeftHand)
    {
        second.transform.position = first.transform.position;

        second.transform.localRotation = Quaternion.Euler(first.transform.rotation.eulerAngles - (isLeftHand ? LeftHandRotationOffset: RightHandRotationOffset));
    }
}
