using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLinePolygen : MonoBehaviour
{
    //首先获取中间部分顶点信息，构建矩形Mesh
    private Vector3[] m_vertexList;
    private LineRenderer m_lineRenderer;
    private PolygonCollider2D m_poly2D;

    private Vector3[] m_originLineRendererPoint;

    private Vector3 lastPosition;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_poly2D = GetComponent<PolygonCollider2D>();
    }
    void Start()
    {
        GeneratePolygon();
        lastPosition = transform.position;
    }


    private void Update()
    {
       
    }

    void GeneratePolygon()
    {
        m_vertexList = new Vector3[m_lineRenderer.positionCount * 2];
        for(int i=0;i<m_lineRenderer.positionCount;i++)
        {
            m_lineRenderer.SetPosition(i, new Vector3(m_lineRenderer.GetPosition(i).x, m_lineRenderer.GetPosition(i).y, 0));
        }
        m_lineRenderer.GetPositions(m_vertexList);
        m_originLineRendererPoint = m_vertexList;


        Vector3 LastPoint = m_vertexList[0];
        Vector3 CurrentPoint;
        Vector3 NextPoint;
        #region 矩形绘制
        //记录第二个点和倒数第二个点
        Vector3 theSecondPoint = m_vertexList[1];
        Vector3 theSecondToLastPoint = m_vertexList[m_lineRenderer.positionCount - 2];

        for (int i = 1; i < m_lineRenderer.positionCount - 1; i++)
        {
            CurrentPoint = m_vertexList[i];
            NextPoint = m_vertexList[i + 1];
            //计算向量a
            Vector3 VectorNormalizedA = (LastPoint - m_vertexList[i]).normalized;
            //计算向量b
            Vector3 VectorNormalizedB = (NextPoint - m_vertexList[i]).normalized; ;

            //单位向量相加获得垂线向量
            Vector3 VectorNormalizedPerpendicular = (VectorNormalizedA + VectorNormalizedB).normalized;

            //计算上面一点的位置
            Vector3 newUpVector3;
            Vector3 newDownVector3;
            if (VectorNormalizedPerpendicular.y >= 0)
            {
                newUpVector3 = CurrentPoint + VectorNormalizedPerpendicular * m_lineRenderer.startWidth / 2;
                newDownVector3 = CurrentPoint - VectorNormalizedPerpendicular * m_lineRenderer.startWidth / 2;
            }
            else
            {
                newUpVector3 = CurrentPoint - VectorNormalizedPerpendicular * m_lineRenderer.startWidth / 2;
                newDownVector3 = CurrentPoint + VectorNormalizedPerpendicular * m_lineRenderer.startWidth / 2;
            }
            newUpVector3.z = 0;
            newDownVector3.z = 0;
            m_vertexList[i] = newUpVector3;
            m_vertexList[m_lineRenderer.positionCount + i] = newDownVector3;
            //更新Last,Next
            LastPoint = CurrentPoint;
        }
        //解决两端
        //左端
        Vector3 currentPos = m_vertexList[0];
        Vector3 Vector3NormalA = (theSecondPoint - m_vertexList[0]).normalized;
        Vector3 VectorNormalizedPerpendicularLeft = (new Vector3(-Vector3NormalA.y / Vector3NormalA.x, 1, 0)).normalized;

        m_vertexList[0] = (VectorNormalizedPerpendicularLeft * m_lineRenderer.startWidth / 2) + currentPos;

        m_vertexList[m_lineRenderer.positionCount] = -(VectorNormalizedPerpendicularLeft * m_lineRenderer.startWidth / 2) + currentPos;

        //右端
        currentPos = m_vertexList[m_lineRenderer.positionCount - 1];
        Vector3NormalA = (m_vertexList[m_lineRenderer.positionCount - 1] - theSecondToLastPoint).normalized;
        VectorNormalizedPerpendicularLeft = (new Vector3(-Vector3NormalA.y / Vector3NormalA.x, 1, 0)).normalized;
        m_vertexList[m_lineRenderer.positionCount - 1] = (VectorNormalizedPerpendicularLeft * m_lineRenderer.startWidth / 2) + currentPos;
        m_vertexList[m_lineRenderer.positionCount - 1 + m_lineRenderer.positionCount] = -(VectorNormalizedPerpendicularLeft * m_lineRenderer.startWidth / 2) + currentPos;

        #endregion
        #region 圆角绘制

        #endregion

        Vector2[] newVervex2D = new Vector2[m_vertexList.Length];


        int tmpIndex = 0;
        for (int i = 0; i < m_lineRenderer.positionCount; i++)
        {
            newVervex2D[tmpIndex++] = new Vector2(m_vertexList[i].x, m_vertexList[i].y);
        }
        for (int i = m_lineRenderer.positionCount-1; i >=0; i--)
        {
            newVervex2D[tmpIndex++] = new Vector2(m_vertexList[m_lineRenderer.positionCount + i].x, m_vertexList[m_lineRenderer.positionCount + i].y);
        }
        m_poly2D.points = newVervex2D;
    }
    
}
