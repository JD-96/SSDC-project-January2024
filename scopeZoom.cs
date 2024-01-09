using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to the player which handels the sensitivity and fov while zoomed in

public class scopeZoom : MonoBehaviour
{
    public float zoomFov = 30f;
    public float lookSensitivityX = 10f;
    public float lookSensitivityY = 10f;
    public Camera mainCamrea;
    public bool isZoomed = false;
    private float normalFov;

    void Start()
    {
        normalFov = mainCamrea.fieldOfView;
    }

    void Update()
    {
        //zoom trigger to switch state between zoom in and zoom out
        if (Input.GetMouseButtonDown(1))
        {
            if (!isZoomed)
            {
                DoZoom();
                isZoomed = true;
            }
            else
            {
                DoUnzoom();
                isZoomed=false;
            }
        }
    }
    //Changes Fov while zooming in and out
    private void DoZoom()
    {
        mainCamrea.fieldOfView = zoomFov;
    }
    private void DoUnzoom()
    {
        mainCamrea.fieldOfView = normalFov;
    }
}
