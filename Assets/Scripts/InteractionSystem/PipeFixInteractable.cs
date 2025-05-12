using UnityEngine;

public class PipeFixInteractable : InteractableBase
{
    [SerializeField] private GameObject goodPipe;              // PIPE (GOOD)
    [SerializeField] private ParticleSystem waterParticles;    // Particle System on PIPE (BAD)
    public ScoreScript ScoreScript;


    public void Start()
    {
        ScoreScript = FindAnyObjectByType<ScoreScript>();
    }
    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with broken pipe");
        

        if (waterParticles != null && waterParticles.isPlaying)
        {
            waterParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        
        if (goodPipe != null)
            goodPipe.SetActive(true);
            ScoreScript.Score += 20;

        gameObject.SetActive(false);
    }
}
