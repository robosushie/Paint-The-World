using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHandPose : MonoBehaviour
{
    public float poseTransitionDuration = 0.2f;
    public HandData rightHandPose;
    public HandData rightHandController;

    private Vector3 startHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startFingerRotations;
    private Quaternion[] finalFingerRotations;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);
        rightHandPose.gameObject.SetActive(false);
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor || arg.interactorObject is XRRayInteractor)
        {
            Debug.Log(arg.interactorObject);
            HandData handData = rightHandController;
            Debug.Log(handData);
            handData.animator.enabled = true;

            StartCoroutine(SetHandDataRoutine(handData, startHandPosition, startHandRotation, startFingerRotations, finalHandPosition, finalHandRotation, finalFingerRotations));
        }
    }
    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if(arg.interactorObject is XRDirectInteractor || arg.interactorObject is XRRayInteractor)
        {
            Debug.Log(arg.interactorObject);
            HandData handData = rightHandController;
            Debug.Log(handData);
            handData.animator.enabled = false;

            SetHandDataValues(handData, rightHandPose);
            StartCoroutine(SetHandDataRoutine(handData, finalHandPosition, finalHandRotation, finalFingerRotations, startHandPosition, startHandRotation, startFingerRotations));
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startHandPosition = h1.root.localPosition;
        finalHandPosition = h2.root.localPosition;

        startHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for(int i=0; i<h1.fingerBones.Length; i++)
        {
            startFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for(int i=0; i<newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation, Vector3 startPosition, Quaternion startRotation, Quaternion[] startBonesRotation)
    {
        float timer = 0;
        
        while(timer < poseTransitionDuration)
        {

            Vector3 p = Vector3.Lerp(startPosition, newPosition, timer/poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startRotation, newRotation,timer/poseTransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;

            for(int i=0; i<newBonesRotation.Length; i++)
            {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startBonesRotation[i], newBonesRotation[i], timer/poseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

}
