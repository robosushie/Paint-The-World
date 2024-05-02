using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class PaintController : MonoBehaviour
{
    public ParticleSystem MainParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(StartPainting);
        grabbable.deactivated.AddListener(StopPainting);
    }

    private void StopPainting(DeactivateEventArgs arg0)
    {
        MainParticleSystem.Stop();
    }

    private void StartPainting(ActivateEventArgs arg0)
    {
        MainParticleSystem.Play();
    }

}
