using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgController : MonoBehaviour
{
    Vector3 startPos;

    // private bool isRush = false;

    public static bool isAtEnd = false;

    public static float bgSpeed = 4f;

    public float bgReload = 0f;

    void Start()
    {
        startPos = transform.position;
        isAtEnd = false;
    }

    void Update()
    {
        if (transform.position.x < -bgReload)
        {
            SuspendStart();
            isAtEnd = true;
        }
        transform.Translate(-bgSpeed * Time.deltaTime, 0, 0);

    }

    public static void SuspendStart()
    {
        bgSpeed = 0;
    }

    public static void SuspendStop()
    {
        bgSpeed = 4f;
    }

    public static void Backward()
    {
        bgSpeed = -4f;
    }
}
