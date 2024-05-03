using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameColorManager : MonoBehaviour
{
    public Color paintColor;
    public ParticlesPainter[] painters;
    public ParticleSystem[] particles;
    public GameObject roller;
    public GameObject can;
    public GameObject ring;

    // Start is called before the first frame update
    void Start()
    {
        SetColor();
    }

    public void SetParticleSystemColor(Color color)
    {
        for(int i=0; i<particles.Length; i++)
        {
            ParticleSystemRenderer psRenderer = particles[i].GetComponent<ParticleSystemRenderer>();
            if (psRenderer != null)
            {
                psRenderer.material.color = color;
                var trail = particles[i].trails;
                if(trail.enabled)
                {
                    psRenderer.trailMaterial.color = color;
                }
                
            }
        }
    }

    public void SetPropColor(Color color)
    {
        roller.GetComponent<SkinnedMeshRenderer>().materials[0].color = color;
        can.GetComponent<MeshRenderer>().materials[1].color = color;
        ring.GetComponent<MeshRenderer>().materials[0].color = color;

    }

    public void SetColor(string str = "#A71E4C")
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(str, out color))
        {
            Debug.Log("Color parsed successfully: " + color);
            paintColor = color;
            for (int i = 0; i < painters.Length; i++)
            {
                painters[i].paintColor = color;
            }
            SetParticleSystemColor(color);
            SetPropColor(color);
        }
        else
        {
            Debug.LogError("Failed to parse color from string: " + str);
        }
    }

}
