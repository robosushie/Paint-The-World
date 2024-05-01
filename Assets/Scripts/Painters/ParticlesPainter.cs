using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPainter : MonoBehaviour
{
    public Color paintColor;

    public float minRadius = 0.05f;
    public float maxRadius = 0.2f;
    public float strength = 1;
    public float hardness = 1;
    
    public ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        
        collisionEvents = new List<ParticleCollisionEvent>();
        //var pr = part.GetComponent<ParticleSystemRenderer>();
        //Color c = new Color(pr.material.color.r, pr.material.color.g, pr.material.color.b, .8f);
        //paintColor = c;
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        Debug.Log(numCollisionEvents);
        Paintable p = other.GetComponent<Paintable>();
        if (p != null)
        {
            Debug.Log(p);
            for (int i = 0; i < numCollisionEvents; i++)
            {
                Vector3 pos = collisionEvents[i].intersection;
                float radius = Random.Range(minRadius, maxRadius);
                PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
            }
        }
    }
}