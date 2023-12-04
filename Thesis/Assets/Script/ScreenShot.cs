using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenShot : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CaptureScreenshot();
            Debug.Log("Screenshot Captured");
        }
    }

    void CaptureScreenshot()
    {
        DateTime now = DateTime.Now;
        string formattedDateTime = now.ToString("yyyyMMddHHmmss");

        string screenshotPath = "Assets/Screenshot/" + formattedDateTime + ".png";
        ScreenCapture.CaptureScreenshot(screenshotPath);
    }
}
