using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ExplosionTrigger : MonoBehaviour
{
    private ParticleSystem particleSyst;

    public void Explode() => particleSyst.Play();

    public void Initialize() => particleSyst = GetComponent<ParticleSystem>();
}
