using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isCanDraw;
    public GameObject linePrefab;
    private GameObject line;
    private LineRenderer lineRenderer;
    private int linePointCount;
    public float minOffest;


    public Button createLine;
    public Button clearLine;
    public Image maskPanel;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isCanDraw = false;
        createLine.interactable = true;
        clearLine.interactable = false;
    }
    private void Update()
    {
        if(isCanDraw&&Input.GetMouseButtonDown(0))
        {
            line = Instantiate(linePrefab);
            lineRenderer = line.GetComponent<LineRenderer>();
            linePointCount = 0;
        }
        if(isCanDraw&&Input.GetMouseButton(0))
        {
            Vector3 currentVector3 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,10));
            
            if (linePointCount==0||Vector3.Distance(currentVector3, lineRenderer.GetPosition(linePointCount-1))>= minOffest)
            {
                Debug.Log(currentVector3);
                linePointCount++;
                lineRenderer.positionCount = linePointCount;

                lineRenderer.SetPosition(linePointCount - 1, currentVector3);
            }
            
        }
        if(line!=null&&isCanDraw&&Input.GetMouseButtonUp(0))
        {
            isCanDraw = false;
            createLine.interactable = false;
            clearLine.interactable = true;
            maskPanel.enabled = false;

            Rigidbody2D rigid2D=line.AddComponent<Rigidbody2D>();
            rigid2D.mass = 50;

            line.AddComponent<PolygonCollider2D>();
            line.AddComponent<GenerateLinePolygen>();
        }
    }
    void DestoryCurrentLine()
    {
        if (isCanDraw) return;
        else
        {
            Destroy(line);
            createLine.interactable = true;
            clearLine.interactable = false;
        }
    }

    public void ClearLine()
    {
        DestoryCurrentLine();
    }

    public void DrawLine()
    {
        isCanDraw = true;
        createLine.interactable = false;
        clearLine.interactable = false;
    }
}
