using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntibodyMove : MonoBehaviour
{
    public Rigidbody2D RB;
    public float shotSpeed = 10;
    public float suspendSpeed = 0f;//悬停时的子弹移动速度
    public float maxLifeTime = 25.0f;//子弹最大存在时间
    private float lifeTime;//子弹已存在的时间
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //让子弹朝向设定的目标进行移动
        //设置目标点坐标值
        //悬停时，子弹减速发射（或者可以设置为零直接停下来）
        if (PlayerController.isSuspending)
        {  
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(-163, -2, 0), suspendSpeed*shotSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(-163, -2, 0),shotSpeed * Time.deltaTime);
        }
        
        

        //时间控制
        lifeTime += Time.deltaTime;
        if (lifeTime>maxLifeTime)
        {
            Destroy(gameObject);//到时销毁
        }
    }
}
