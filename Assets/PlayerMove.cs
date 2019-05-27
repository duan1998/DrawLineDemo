using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    public enum FaceDir
    {
        Right,
        Left
    }
    private FaceDir m_faceDir;
    public float m_moveSpeed;
    public float m_jumpForce;

    private Rigidbody2D m_rigid2D;

    private void Awake()
    {
        m_rigid2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_faceDir = FaceDir.Right;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isCanDraw)
        {
            float h = Input.GetAxis("Horizontal");
            if (Mathf.Abs(h) > 0)
            {
                if (h < 0 && m_faceDir == FaceDir.Right)
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    m_faceDir = FaceDir.Left;
                }
                else if (h > 0 && m_faceDir == FaceDir.Left)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    m_faceDir = FaceDir.Right;
                }
                m_rigid2D.velocity = new Vector2((m_faceDir == FaceDir.Right ? m_moveSpeed : -m_moveSpeed) * Time.deltaTime, m_rigid2D.velocity.y);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_rigid2D.velocity = new Vector2(m_rigid2D.velocity.x, m_jumpForce);
            }
        }
       
       
    }
}
