using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntibodyShot : MonoBehaviour
{
    private float shotRate = 0.1f;//���ʱ��
    public float timekeeping;//��ʱ
    public int shotTimes = 0; //�������
    public int shotRound = 0;//����ִ�
    private float roundTime = 2f;
    public GameObject antiBody;//��ȡ�ӵ�����
    public GameObject cov;//��ȡalarmActivated���ڵĶ���
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (cov.GetComponent<PlayerController>().alarmActivated)//����ϸ�����������
        {
            Shot();
        }
    }
    void Shot()
    {
        timekeeping += Time.deltaTime;
        if (timekeeping > shotRate && shotTimes < 1)//����ʱ�����ڵ���������ʱ�������3����
        {
            //ʵ����һ���ӵ�
            Instantiate(antiBody, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Quaternion.identity);
            shotTimes += 1;//�����������
            timekeeping = 0;//���ü�ʱ��
        }
        if (shotTimes==1 && timekeeping>roundTime)//�������3���Ҽ�ʱ������3�����������ִμ��
        {
            timekeeping = 0;//���ü�ʱ��
            shotRound += 1;//����ִ�����
            shotTimes = 0;//���������������
            //Debug.Log("round over");
        }
        if (shotRound > 1)//����ڶ��ֺ�
        {
            shotRound = 0;//��������ִ�
            shotTimes = 0;//���������������
            timekeeping = 0;//���ü�ʱ��
            cov.GetComponent<PlayerController>().alarmActivated = false;//�������ر�
            //Debug.Log("�����ѽ��");
        }
    }
}
