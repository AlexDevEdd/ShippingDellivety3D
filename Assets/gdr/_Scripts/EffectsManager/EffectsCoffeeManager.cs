using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsCoffeeManager : MonoBehaviour
{
    public Transform aspectTr;
    public Vector2Int particleCount;
    public RectTransform energyIconTr;
    public float particlesSize = 25.0f;
    private ParticleSystem rewardPS;
    private ParticleSystem.Particle[] particles;

    private void Awake()
    {
        rewardPS = GetComponent<ParticleSystem>();
        enabled = true;
    }

    [NaughtyAttributes.Button]
    public void PlayEffect()
    {
        rewardPS.Play();
        particles = new ParticleSystem.Particle[Random.Range(particleCount.x, particleCount.y)];
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i] = new ParticleSystem.Particle()
            {
                position = new Vector3(Camera.main.pixelWidth / aspectTr.position.x / 2.0f, Camera.main.pixelHeight / aspectTr.position.x / 1.8f, 0),
                remainingLifetime = 1000.0f,
                startSize = particlesSize,
                startColor = Color.white,
                startLifetime = 1000.0f,
                velocity = Random.insideUnitSphere * 30.0f
            };
        }
        rewardPS.SetParticles(particles);
        CoroutineActions.ExecuteAction(0.4f, () =>
        {
            var target = energyIconTr.position - rewardPS.transform.position;
            rewardPS.GetParticles(particles);
            if (particles != null && particles.Length > 0)
                for (int i = 0; i < particles.Length; i++)
                {
                    Vector3 dir = target + particles[i].position;
                    var _targetDis = (energyIconTr.position - particles[i].position * aspectTr.position.x) / aspectTr.position.x;
                    particles[i].velocity = _targetDis / 12.0f;
                    particles[i].remainingLifetime = 12.0f;
                }
            rewardPS.SetParticles(particles);
        });
    }
}
