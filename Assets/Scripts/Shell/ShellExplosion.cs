using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;

    private TankHealthPUN m_CurrentTarget;


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);        
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++) {
            Rigidbody targetRigidBody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidBody) continue;

            targetRigidBody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            var targetHealth = targetRigidBody.GetComponent<TankHealthPUN>();
            if (!targetHealth) continue;

            float damage = CalculateDamage(targetRigidBody.position);
            targetHealth.TakeDamage(damage);
        }

        var cloneParticles = Instantiate(m_ExplosionParticles);
        var explosionParticles = cloneParticles.GetComponent<ParticleSystem>();
        explosionParticles.transform.position = transform.position;
        explosionParticles.transform.parent = null;
        explosionParticles.Play();
        Destroy(explosionParticles, explosionParticles.main.duration);
        Destroy(cloneParticles.gameObject, explosionParticles.main.duration);

        var cloneAudio = Instantiate(m_ExplosionAudio);
        var audioComponent = cloneAudio.GetComponent<AudioSource>();
        audioComponent.transform.parent = null;
        audioComponent.Play();
        Destroy(audioComponent, audioComponent.clip.length);
        Destroy(cloneAudio.gameObject, audioComponent.clip.length);

        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
        float damage = relativeDistance * m_MaxDamage;
        return Mathf.Max(0f, damage);
    }
}