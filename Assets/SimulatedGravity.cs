using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedGravity : MonoBehaviour
{
    private const float m_gravityScale = 9.81f;
    private PolygonCollider2D m_poly2D;
    private Vector2[] m_vertexArray;

    public bool isGravity;

    private void Awake()
    {
        m_poly2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        m_vertexArray = new Vector2[m_poly2D.GetTotalPointCount()];
        m_vertexArray = m_poly2D.points;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGravity)
        {
            ApplyGravity();
        
        }
        
    }
    void ApplyGravity()
    {
        
        for (int i = 0; i < m_vertexArray.Length; i++)
        {
            m_vertexArray[i].y -= m_gravityScale * Time.deltaTime;
        }
        m_poly2D.points = m_vertexArray;
        
        transform.position = new Vector3(transform.position.x, transform.position.y - m_gravityScale * Time.deltaTime, transform.position.z);
    }
}
