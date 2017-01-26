using UnityEngine;

public class TankMovementPUN : MonoBehaviour
{

    #region Movement variables
    public float m_Speed = 7f;
    public string m_MovementAxisName = "Vertical";
    [HideInInspector] public bool IsDriving
    {
        private set { m_IsDriving = value; }
        get { return m_IsDriving; }
    }

    private float m_MovementInputValue;
    private bool m_IsDriving;
    #endregion

    #region Turning variables
    public float m_TurnSpeed = 180f;
    public string m_TurnAxisName = "Horizontal";

    private float m_TurnInputValue;
    private Quaternion m_desiredRotation;
    #endregion

    #region DustTrail variables
    public ParticleSystem m_LeftDustTrail;
    public ParticleSystem m_RightDustTrail;
    #endregion

    void Update()
    {
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        transform.position += transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        IsDriving = false;
        if (Mathf.Abs(m_MovementInputValue) > 0.1f || Mathf.Abs(m_TurnInputValue) > 0.1f) IsDriving = true;
        PlayDustTrail();
    }

    void Turn()
    {
        var turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        if (m_MovementInputValue < 0) turn *= -1;
        transform.rotation *= Quaternion.Euler(0f, turn, 0f);        
    }

    void PlayDustTrail()
    {
        if (IsDriving && m_LeftDustTrail.isStopped)
        {
            m_LeftDustTrail.Play();
            m_RightDustTrail.Play();
        }
        else
        {
            m_LeftDustTrail.Stop();
            m_RightDustTrail.Stop();
        }
    }
}
