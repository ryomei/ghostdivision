using UnityEngine;

class TankAudioPUN : MonoBehaviour
{
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling = null;
    public AudioClip m_EngineDriving = null;
    public float m_PitchRange = 0.2f;

    private float m_OriginalPitch;
    private TankMovementPUN m_TankMovementPUN;

    void Start()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
        m_TankMovementPUN = GetComponent<TankMovementPUN>();
    }

    void Update()
    {
        EngineAudio();
    }

    void EngineAudio()
    {   
        if (m_TankMovementPUN.IsDriving && m_MovementAudio.clip == m_EngineIdling) ToggleAudio();
        if (!m_TankMovementPUN.IsDriving && m_MovementAudio.clip == m_EngineDriving) ToggleAudio();
    }

    void ToggleAudio()
    {
        m_MovementAudio.clip = m_MovementAudio.clip == m_EngineIdling ? m_EngineDriving : m_EngineIdling;
        m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
        m_MovementAudio.Play();
    }
}
