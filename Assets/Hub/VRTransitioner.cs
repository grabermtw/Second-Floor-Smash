using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class VRTransitioner : HubInteractor
{
    public GameObject player2d;
    public GameObject mainCam2d;
    public GameObject playerVr;

    bool xr = false;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    
    public override void Interact()
    {
        Debug.Log("vr switch time");
        if (!xr)
        {
            StartXR();
            Debug.Log("we should be in VR now");
            xr = true;
            player2d.SetActive(false);
            mainCam2d.SetActive(false);
            playerVr.SetActive(true);
        }
        else
        {
            Debug.Log("exit VR");
            StopXR();
            xr = false;
            playerVr.SetActive(false);
            player2d.SetActive(true);
            mainCam2d.SetActive(true);
        }
    }

    private void StartXR()
    {
        
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }
 
    private void StopXR()
    {
     
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
        }
    }

}