using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntibodyShot : MonoBehaviour
{
    private float shotRate = 0.1f;//间隔时间
    public float timekeeping;//计时
    public int shotTimes = 0; //射击次数
    public int shotRound = 0;//射击轮次
    private float roundTime = 2f;
    public GameObject antiBody;//获取子弹对象
    public GameObject cov;//获取alarmActivated所在的对象
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (cov.GetComponent<PlayerController>().alarmActivated)//巨噬细胞警报激活后
        {
            Shot();
        }
    }
    void Shot()
    {
        timekeeping += Time.deltaTime;
        if (timekeeping > shotRate && shotTimes < 1)//当计时器大于单次射击间隔时间且射击3次内
        {
            //实例化一个子弹
            Instantiate(antiBody, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Quaternion.identity);
            shotTimes += 1;//射击次数增加
            timekeeping = 0;//重置计时器
        }
        if (shotTimes==1 && timekeeping>roundTime)//当射击满3次且计时器大于3次射击间隔加轮次间隔
        {
            timekeeping = 0;//重置计时器
            shotRound += 1;//射击轮次增加
            shotTimes = 0;//单轮射击次数重置
            //Debug.Log("round over");
        }
        if (shotRound > 1)//射击第二轮后
        {
            shotRound = 0;//重置射击轮次
            shotTimes = 0;//单轮射击次数重置
            timekeeping = 0;//重置计时器
            cov.GetComponent<PlayerController>().alarmActivated = false;//将警报关闭
            //Debug.Log("警报已解除");
        }
    }
}
