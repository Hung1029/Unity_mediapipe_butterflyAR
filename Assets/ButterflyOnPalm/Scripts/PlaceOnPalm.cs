using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaceOnPalm : MonoBehaviour
{
    bool showButterfly = true;
    public Text ButterFlyPos;
    public Text ButterFlyShow;
    public Text HandState;

    private void Start()
    {
        ButterFlyPos.text = "INIT";
        ButterFlyShow.text = "INIT";
        HandState.text = "INIT";

        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ARSession.state == ARSessionState.SessionTracking)
        {
            FollowPalmCenter();
        }
    }

    private void FollowPalmCenter()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        HandInfo currentlyDetectedHand = ManomotionManager.Instance.Hand_infos[0].hand_info;
        ManoGestureContinuous currentlyDetectedContinousGesture = currentlyDetectedHand.gesture_info.mano_gesture_continuous;

        Vector3 pinchPos = currentlyDetectedHand.tracking_info.palm_center;
        HandState.text = "HAND"+ pinchPos.ToString();

        if (currentlyDetectedContinousGesture == ManoGestureContinuous.OPEN_HAND_GESTURE)
        {
            HandState.text = "OPENHAND " + pinchPos.ToString();
            Vector3 newPos = ManoUtils.Instance.CalculateNewPosition(pinchPos, currentlyDetectedHand.tracking_info.depth_estimation);
            if (!showButterfly)
            {
                foreach (var mr in meshRenderers)
                {
                    mr.enabled = true;
                }
                transform.position = newPos;
                showButterfly = true;
                ButterFlyPos.text = transform.position.ToString();
                ButterFlyShow.text = "TRUE";
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5f);
                ButterFlyPos.text = transform.position.ToString();
                ButterFlyShow.text = "TRUE";
            }
        }
        else
        {
            HandState.text = "NO HAND";
            if (showButterfly)
            {
                foreach (var mr in meshRenderers)
                {
                    mr.enabled = false;
                }
                showButterfly = false;
                ButterFlyPos.text = "Missing";
                ButterFlyShow.text = "FALSE";

            }
        }
        }
}
