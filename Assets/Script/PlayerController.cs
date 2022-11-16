using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator playerAnim;//玩家动画控制
    public float jumpForce = 5; //跳跃力度
    public Collider2D coll;
    private float currentTime = 0f; //跳跃间隔时间
    private bool situationFlag = false; //跳跃许可标志

    public static float maxSuspendTime = 4f; //最大悬浮时间
    public static bool isSuspending = false;
    public static bool canSuspend = true;
    public static float suspendableTime = 4f;
    public float suspendConsumeRate = 2f;
    public float suspendRecoverRate = 1f;
    public float suspendGap = 2f;
    private float suspendWaitTime = 0f;

    // readonly float slowMotionDuration = 1f; // 渐变持续时间
    readonly float RushDuration = 1f; // 加速持续时间

    public Text toolNum;//道具收集数
    public Text ace2Num;////第二关ACE2受体的收集数量
    public int tool;
    public bool ACE2Flag = false;//第二关通关许可标志
    public int ace2 = 0;//第二关ACE2受体的收集数量
    private bool atEnd = false;

    public bool alarmActivated = false;//第三关警报激活标志，默认关闭
    public Transform End;

    void Update()
    {
        Jump(); // 跳跃控制

        Suspend(); // 悬停控制

        FinishCheck();

        //第三关这个浆细胞和巨噬细胞的逻辑需要设置一个条件
        //使其在第三关再运行这个函数
        if (SceneManager.GetActiveScene().name == "Chapter3")
            Chapter3Alarm();
    }


    void FinishCheck()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Chapter1")
        {
            GameManager.LevelCalculator(1);
            if (BgController.isAtEnd || this.transform.position.x > End.position.x)//到达末端
            {
                AudioManager.CloseLevelAudio();
                SceneManager.LoadScene("Chapter1.5");
            }
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Chapter2")
        {
            GameManager.LevelCalculator(2);
            if (ace2 >= 2 && atEnd)//收集完成
            {
                GameManager.PlayReset();
                AudioManager.CloseLevelAudio();
                SceneManager.LoadScene("Chapter2.5");
            }
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Chapter3")
        {
            GameManager.LevelCalculator(3);
            if (BgController.isAtEnd || this.transform.position.x > End.position.x)//到达末端
            {
                AudioManager.CloseLevelAudio();
                SceneManager.LoadScene("Chapter3.5");
            }
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Chapter4")
        {
            GameManager.LevelCalculator(4);
            if (BgController.isAtEnd || this.transform.position.x > End.position.x)//到达末端
            {
                AudioManager.CloseLevelAudio();
                SceneManager.LoadScene("Success");
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Trigger Enter");
        //收集ATP
        if (collision.tag == "ATP")
        {
            Destroy(collision.gameObject);
            tool += 1;
            toolNum.text = tool.ToString();
            AudioManager.GetAtpAudio();
            GameManager.AcheivementCalculator("coin");
        }

        //收集ACE2受体
        if (collision.tag == "ACE2")
        {
            Destroy(collision.gameObject);
            ace2 += 1;
            ace2Num.text = ace2.ToString();
            GameManager.AcheivementCalculator("ace2");
        }

        if (collision.tag == "Macrophage")
        {
            alarmActivated = true; //触碰巨噬细胞后，警报激活
            Debug.Log("已激活警报");
            AudioManager.GetMacroAudio();
        }

        //收集医疗道具
        if (collision.tag == "Med")
        {
            Destroy(collision.gameObject);
            PlayerHealth.HealthUp();
            AudioManager.GetHealthAudio();
        }

        //收集加速道具
        if (collision.tag == "Rush")
        {
            Destroy(collision.gameObject);
            // AudioManager.RushAudio();
            TimeController.BulletTime(RushDuration);
            AudioManager.GetSpeedAudio();
        }

        //收集悬停补充道具
        if (collision.tag == "Suspend")
        {
            Destroy(collision.gameObject);
            // AudioManager.ResusAudio();
            suspendableTime = 4f;
            AudioManager.GetSuspendAudio();
        }
        //受到抗体子弹的攻击
        if (collision.tag == "Antibody")
        {
            Destroy(collision.gameObject);
            PlayerHealth.HealthDown();
            AudioManager.ShootBulletAudio();
        }
        if (collision.tag == "Ch4Virus")
        {
            Debug.Log("碰到病毒");
            Destroy(collision.gameObject);
            PlayerHealth.HealthDown();
        }
        if (collision.tag == "Ch4Macro")
        {
            collision.GetComponent<Ch4Macro>().ClearVirus();
        }
        if (collision.tag == "End")
        {
            atEnd = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //触碰纤毛实现背景向后移动
        if (collision.tag == "Cilium")
        {
            BgController.Backward();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //离开纤毛实现背景向前移动
        if (collision.tag == "Cilium")
        {
            BgController.SuspendStop();
        }
    }
    /*private void Chapter2Finish()//判断第二关能否结束
    {
        if (ace2Num > 20)
        {
            ACE2Flag = true;//这里后期可以直接替换成控制关卡切换
        }
    }*/

    private void Chapter3Alarm()
    {
        GameObject[] macrophage = GameObject.FindGameObjectsWithTag("Macrophage");//此处获得的是gameobject数组
        GameObject[] plasmaCell = GameObject.FindGameObjectsWithTag("Plasma");
        //当警报激活后，所有巨噬细胞隐藏，警报解除后所有巨噬细胞又恢复
        //浆细胞部分可以使用AlarmActivated标志来表示浆细胞出动以及攻击是否结束
        if (alarmActivated)
        {
            //易报错，此处需要让巨噬细胞个数与数组对应
            for (int i = 0; i < 6; i++)
            {
                macrophage[i].GetComponent<Renderer>().enabled = false;// 隐藏全部巨噬细胞
                plasmaCell[i].GetComponent<Renderer>().enabled = true;//显示全部浆细胞
                //这里是根据需求页的只有一个浆细胞所以数组下标直接给到0，如果增加浆细胞在这里改代码即可
            }

        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                macrophage[i].GetComponent<Renderer>().enabled = true;// 显示全部巨噬细胞
                plasmaCell[i].GetComponent<Renderer>().enabled = false;//隐藏全部浆细胞
                plasmaCell[i].GetComponent<AntibodyShot>().shotRound = 0;//重置射击轮次
                plasmaCell[i].GetComponent<AntibodyShot>().shotTimes = 0;//单轮射击次数重置
                plasmaCell[i].GetComponent<AntibodyShot>().timekeeping = 0;//重置计时器
                //同上
            }
        }
    }

    public void Jump()
    {
        /*
            可以在unity主界面通过调整rigbody中的重力大小和PlayControler中的jumpForce（跳跃力度）
            来控制手感，目前是重力2跳跃6。相对之前手感略有优化
         */
        currentTime += Time.deltaTime;
        playerAnim.SetBool("jumping", false);
        if (Input.GetButtonDown("Jump"))
        {
            //最小跳跃间隔
            if (currentTime < 0.05f) return;
            if (situationFlag == false)
            {
                situationFlag = true;
            }
            if (situationFlag == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); //跳跃
                playerAnim.SetBool("jumping", true);//设置跳跃动画为真
                AudioManager.JumpAudio();
                situationFlag = false;
            }
            currentTime = 0;

        }
    }

    public void TouchJump()
    {
        // 单击按钮进行跳跃，无需监听键盘
        currentTime += Time.deltaTime;
        playerAnim.SetBool("jumping", false);
        //最小跳跃间隔
        if (currentTime < 0.05f) return;
        if (situationFlag == false)
        {
            situationFlag = true;
        }
        if (situationFlag == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //跳跃
            playerAnim.SetBool("jumping", true);//设置跳跃动画为真
            AudioManager.JumpAudio();
            situationFlag = false;
        }
        currentTime = 0;
    }

    public void Suspend()
    {
        playerAnim.SetBool("suspending", false);//设置悬停默认值
        /*
            Suspend暂时绑定键盘z触发悬浮
            需要更改inputmanager，路径为edit->ProjectSettings->InputManager
            选择一个按键并更改name为Suspend,并绑定positive button,推荐z,即可使用以下功能
        */
        //ToDo 绑定button至UI
        if (Input.GetButtonDown("Suspend") && canSuspend)//Suspend目前绑定键盘z触发悬浮
        {
            //Time.timeScale = 0;
            AudioManager.SuspendAudio();
            BgController.SuspendStart();
            isSuspending = true;
            playerAnim.SetBool("suspending", true); //设置悬停动画为真
        }

        if (Input.GetButtonUp("Suspend"))//松开时恢复
        {
            //Time.timeScale = 1;
            BgController.SuspendStop();
            isSuspending = false;
            GameManager.AcheivementCalculator("suspend");
        }

        if (isSuspending)
        {
            suspendableTime -= Time.deltaTime * suspendConsumeRate;
            if (suspendableTime <= 0)
            {
                //Time.timeScale = 1;
                BgController.SuspendStop();

                isSuspending = false;
                canSuspend = false;
            }
        }
        else
        {
            if (suspendableTime < maxSuspendTime && canSuspend)
            {
                suspendableTime += Time.deltaTime * suspendRecoverRate;
            }
        }

        if (!canSuspend)
        {
            suspendWaitTime += Time.deltaTime;
            if (suspendWaitTime > suspendGap)
            {
                canSuspend = true;
                suspendWaitTime = 0f;
            }
        }
    }
    public void TouchSuspendDown()
    {
        AudioManager.SuspendAudio();
        BgController.SuspendStart();
        isSuspending = true;
        playerAnim.SetBool("suspending", true); //设置悬停动画为真
    }
    public void TouchSuspendUp()
    {
        BgController.SuspendStop();
        isSuspending = false;
        GameManager.AcheivementCalculator("suspend");
    }

}
