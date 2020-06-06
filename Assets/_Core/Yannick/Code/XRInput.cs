using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRInput : MonoBehaviour
{

    public static XRInput Instance => instance;

    private static XRInput instance;

    private void Awake()
    {
        instance = this;

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
    }

}
