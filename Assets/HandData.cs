using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandData : MonoBehaviour
{
    public enum HandModelType
    {
        Left,
        Right,
    }

    public HandModelType handType;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}
