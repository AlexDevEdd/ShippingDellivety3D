using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPhysicsDemo : MonoBehaviour
{
    public ParticleSystem ps;

    public Vector3 velocityToAdd;

    public Vector3 addNewPos;

    // Also we can use list of transforms/foreach position we assign our particles

    [NaughtyAttributes.Button]
    private void GetParticlesCount()
    {
        Debug.Log($"Count: {ps.particleCount}");
    }

    [NaughtyAttributes.Button]
    private ParticleSystem.Particle[] GetAllParticles()
    {
        var count = ps.particleCount;
        var particles = new ParticleSystem.Particle[count];
        var got = ps.GetParticles(particles, count);
        return particles;
    }

    [NaughtyAttributes.Button]
    private void MoveAll()
    {
        var particles = GetAllParticles();
        var count = particles.Length;
        for (int i = 0; i < count; i++)
        {
            particles[i].position += velocityToAdd;
        }
        ps.SetParticles(particles, count);
    }


    [NaughtyAttributes.Button]
    private void VelocityAll()
    {
        var particles = GetAllParticles();
        var count = particles.Length;
        for (int i = 0; i < count; i++)
        {
            particles[i].velocity += velocityToAdd;
        }
        ps.SetParticles(particles, count);
    }

    [NaughtyAttributes.Button]
    private void AddParticle()
    {
        var particles = GetAllParticles();
        var count = particles.Length;
        var list = new List<ParticleSystem.Particle>();
        list.AddRange(particles);
        var particle = particles[count - 1];
        Debug.Log($"{particle.color}\n{particle.remainingLifetime}\n{particle.remainingLifetime}\n{particle.rotation}\n{particle.size}\n{particle.startLifetime}\n{particle.startSize}\n{particle.velocity}");
        particle.position = addNewPos;
        list.Add(particle);
        ps.SetParticles(list.ToArray(), count + 1);
    }

    //void OnParticleCollision(GameObject other)
    //{
    //    Debug.Log($"Collision with {other}");

    //    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    //    int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
    //    Rigidbody rb = other.GetComponent<Rigidbody>();
    //    int i = 0;

    //    while (i < numCollisionEvents)
    //    {
    //        if (rb)
    //        {
    //            Vector3 pos = collisionEvents[i].intersection;
    //            Vector3 force = collisionEvents[i].velocity * 10;
    //            rb.AddForce(force);
    //        }
    //        i++;
    //    }
    //}

}
