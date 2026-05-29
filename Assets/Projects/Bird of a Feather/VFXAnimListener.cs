using UnityEngine;

public class VFXAnimListener : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticles;


    // Called by animation events
    public void PlayDeathVFX() => deathParticles?.Play();
}