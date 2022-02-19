using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    public RectTransform start, end;
    public float speed = 5f;

    private RectTransform rT;

    // Start is called before the first frame update
    void Start()
    {
        rT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(rT.position, end.position, speed);

        if (rT.position == end.position)
        {
            RectTransform tmp = start;
            start = end;
            end = tmp;
        }
    }
}
