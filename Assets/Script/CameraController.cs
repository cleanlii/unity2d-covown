using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing = 0.1f;
    public bool Y_changeable = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        if (target != null)
        {
            if (transform.position != target.position)
            {
                Vector3 targetPos = transform.position;
                targetPos.x = target.position.x;
                if (Y_changeable)
                {
                    if(target.position.y > -5.86f && target.position.y < 11.14f)
                    {
                        targetPos.y = target.position.y;
                    }
                    else
                    {
                        if(target.position.y < -5.86f)
                        {
                            targetPos.y = -5.86f;
                        }

                        if(target.position.y > 11.14f)
                        {
                            targetPos.y = 11.14f;
                        }
                    }
                    
                }
                    
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            }
        }
    }
}
