using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaceOnPalm : MonoBehaviour
{
    [SerializeField] private FingerInfoGizmo fingerInfoGizmo;

    bool showButterfly = true;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (fingerInfoGizmo == null) 
        {
            try
            {
                fingerInfoGizmo = GameObject.Find("TryOnManager").GetComponent<FingerInfoGizmo>();
                
            }
            catch
            {
                Debug.Log("Can't find 'TryOnManager' GameObject");
            }

        
        }
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class == ManoClass.GRAB_GESTURE)
        {
           
            HandInfo currentlyDetectedHand = ManomotionManager.Instance.Hand_infos[0].hand_info;
            ManoGestureContinuous currentlyDetectedContinousGesture = currentlyDetectedHand.gesture_info.mano_gesture_continuous;

            Vector3 pinchPos = Vector3.Lerp(fingerInfoGizmo.LeftFingerPoint3DPosition, fingerInfoGizmo.RightFingerPoint3DPosition, 0.5f);

            Vector3 newPos = ManoUtils.Instance.CalculateNewPosition(pinchPos, currentlyDetectedHand.tracking_info.depth_estimation);

            if (!showButterfly)
            {
                foreach (var mr in meshRenderers)
                {
                    mr.enabled = true;
                }
                transform.position = newPos;
                showButterfly = true;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5f);
            }



        }
        else {
            if (showButterfly)
            {
                foreach (var mr in meshRenderers)
                {
                    mr.enabled = false;
                }
                showButterfly = false;
            }

        }

    }
}
