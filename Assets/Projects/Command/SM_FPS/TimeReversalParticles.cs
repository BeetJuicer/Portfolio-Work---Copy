using CommandPattern;
using UnityEngine;

public class TimeReversalParticles : MonoBehaviour
{
    private ParticleSystem particles;
    bool isPlaying;
    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(TimeManager.Instance.IsReversing && !isPlaying)
        {
            print("playing..");
            particles.Play();
            isPlaying = true;
        }
        else
        {
            print("stopping..");
            particles.Stop();
            isPlaying = false;
        }
    }
}
