using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntibodyMove : MonoBehaviour
{
    public Rigidbody2D RB;
    public float shotSpeed = 10;
    public float suspendSpeed = 0f;//��ͣʱ���ӵ��ƶ��ٶ�
    public float maxLifeTime = 25.0f;//�ӵ�������ʱ��
    private float lifeTime;//�ӵ��Ѵ��ڵ�ʱ��
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���ӵ������趨��Ŀ������ƶ�
        //����Ŀ�������ֵ
        //��ͣʱ���ӵ����ٷ��䣨���߿�������Ϊ��ֱ��ͣ������
        if (PlayerController.isSuspending)
        {  
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(-163, -2, 0), suspendSpeed*shotSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(-163, -2, 0),shotSpeed * Time.deltaTime);
        }
        
        

        //ʱ�����
        lifeTime += Time.deltaTime;
        if (lifeTime>maxLifeTime)
        {
            Destroy(gameObject);//��ʱ����
        }
    }
}
