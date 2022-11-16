using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    [Header("数据统计")]
    public int deathCount; // 死亡次数
    public int coinCount; // 吃到悬浮(ATP)标记次数
    public int levelCount; // 到达关数
    public int barrierCount; // 撞到障碍物次数
    public int suspendCount; // 使用悬停次数
    public int timeCount; // 游玩时间
    public int aceCount; // 第二关获得受体个数


    [Header("成就状态")]
    public Achievement[] deathCountAchievement;
    public Achievement[] levelCountAchievement;
    public Achievement[] coinCountAchievement;
    public Achievement[] barrierCountAchievement;
    public Achievement[] suspendCountAchievement;
    public Achievement[] timeCountAchievement;

    [Header("第二关弹窗提示")]
    public Achievement[] aceCountAchievement;


    [Header("画廊状态")]
    public bool[] gallery;


    [Header("游戏状态")]
    [SerializeField] GameState gameState = GameState.Playing;

    public event Action DeathCountAction;
    public event Action LevelCountAction;
    public event Action CoinCountAction;
    public event Action BarrierCountAction;
    public event Action SuspendCountAction;
    public event Action TimeCountAction;
    public event Action AceCountAction;

    public Text textReady;

    // 成就弹窗
    public Transform achievementPanel;
    public Text achievementNameText;
    public Text achievementDescriptionText;
    public bool achievementIsShowing = false;

    public int timeReady;

    float timeSpend = 0.0f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FindObjectOfType<GameManager>().DeathCountAction += GetDeathCount;
        FindObjectOfType<GameManager>().CoinCountAction += GetCoinCount;
        FindObjectOfType<GameManager>().LevelCountAction += GetLevelCount;
        FindObjectOfType<GameManager>().BarrierCountAction += GetBarrierCount;
        FindObjectOfType<GameManager>().SuspendCountAction += GetSuspendCount;
        FindObjectOfType<GameManager>().TimeCountAction += GetTimeCount;
        FindObjectOfType<GameManager>().AceCountAction += GetAceCount;
    }

    void Update()
    {
        timeSpend += Time.deltaTime;
        timeCount = (int)timeSpend;
        if (instance.TimeCountAction != null)
        {
            instance.TimeCountAction();
        }
    }

    public static GameState GameState
    {
        // 外界访问私有成员变量的方法
        get => instance.gameState; // setter访问器
        set => instance.gameState = value; // getter访问器
    }
    public static void AcheivementCalculator(string a)
    {
        switch (a)
        {
            case "death":
                instance.deathCount++;
                if (instance.DeathCountAction != null)
                {
                    instance.DeathCountAction();
                }
                break;
            case "coin":
                instance.coinCount++;
                if (instance.CoinCountAction != null)
                {
                    instance.CoinCountAction();
                }
                break;
            case "level":
                instance.levelCount++;
                if (instance.LevelCountAction != null)
                {
                    instance.LevelCountAction();
                }
                break;
            case "barrier":
                instance.barrierCount++;
                if (instance.BarrierCountAction != null)
                {
                    instance.BarrierCountAction();
                }
                break;
            case "suspend":
                instance.suspendCount++;
                if (instance.SuspendCountAction != null)
                {
                    instance.SuspendCountAction();
                }
                break;
            case "ace2":
                instance.aceCount++;
                if (instance.AceCountAction != null)
                {
                    instance.AceCountAction();
                }
                break;
        }
    }

    public static void LevelCalculator(int n)
    {
        instance.levelCount = n;
        if (instance.LevelCountAction != null)
        {
            instance.LevelCountAction();
        }
    }

    // 成就解锁判定
    #region GetCount
    void GetDeathCount()
    {
        if (deathCount == 1)
            PopNewAchievement(deathCountAchievement[0]);
        else if (deathCount == 10)
            PopNewAchievement(deathCountAchievement[1]);
        else if (deathCount == 50)
            PopNewAchievement(deathCountAchievement[2]);
        else if (deathCount == 100)
            PopNewAchievement(deathCountAchievement[3]);
    }
    void GetCoinCount()
    {
        if (coinCount == 1)
            PopNewAchievement(coinCountAchievement[0]);
        else if (coinCount == 5)
            PopNewAchievement(coinCountAchievement[1]);
        else if (coinCount == 10)
            PopNewAchievement(coinCountAchievement[2]);
    }
    void GetLevelCount()
    {
        if (levelCount == 1 && !levelCountAchievement[0].unlocked)
            PopNewAchievement(levelCountAchievement[0]);
        else if (levelCount == 2 && !aceCountAchievement[0].unlocked)
            PopNewAchievement((aceCountAchievement[0]));
        // PopNewAchievement(levelCountAchievement[1]);
        else if (levelCount == 3 && !levelCountAchievement[2].unlocked)
            PopNewAchievement(levelCountAchievement[2]);
        else if (levelCount == 4 && !levelCountAchievement[3].unlocked)
            PopNewAchievement(levelCountAchievement[3]);
    }
    void GetBarrierCount()
    {
        if (barrierCount == 1)
            PopNewAchievement(barrierCountAchievement[0]);
        else if (barrierCount == 10)
            PopNewAchievement(barrierCountAchievement[1]);
        else if (barrierCount == 100)
            PopNewAchievement(barrierCountAchievement[2]);
    }
    void GetSuspendCount()
    {
        if (suspendCount == 10)
            PopNewAchievement(suspendCountAchievement[0]);
        else if (suspendCount == 100)
            PopNewAchievement(suspendCountAchievement[1]);
        else if (suspendCount == 1000)
            PopNewAchievement(suspendCountAchievement[2]);
    }
    void GetTimeCount()
    {
        if (timeCount == 60)
            PopNewAchievement((timeCountAchievement[0]));
        else if (timeCount == 300)
            PopNewAchievement((timeCountAchievement[1]));
        else if (timeCount == 1200)
            PopNewAchievement((timeCountAchievement[2]));
    }
    #endregion

    void GetAceCount()
    {
        if (aceCount == 1)
            PopNewAchievement((aceCountAchievement[1]));
        else if (aceCount == 2)
            PopNewAchievement((aceCountAchievement[2]));
        else if (aceCount == 3)
            PopNewAchievement((aceCountAchievement[3]));
        else if (aceCount == 15)
            PopNewAchievement((aceCountAchievement[4]));
    }

    public static void PlayReset()
    {
        instance.levelCount = 0;
        instance.aceCount = 0;
        for (int i = 0; i < 3; i++)
            instance.aceCountAchievement[i].unlocked = true;
    }

    // 成就解锁弹窗
    void PopNewAchievement(Achievement acm)
    {
        achievementNameText.text = acm.achievementName;
        achievementDescriptionText.text = acm.achievementDescription;

        acm.unlocked = true;

        if (achievementIsShowing == false)
        {
            achievementIsShowing = true;
            StartCoroutine(PopThePanel());
        }
    }

    // 协程控制成就弹窗动画
    IEnumerator PopThePanel()
    {
        float percent = 0;
        float amount = 140f; // 幅度

        while (percent < 1)
        {
            percent += Time.deltaTime / 1f;
            achievementPanel.position += Vector3.up * amount * Time.deltaTime / 1f;

            yield return null;
        }

        yield return new WaitForSeconds(3); // 弹窗持续时间

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / 1f;
            achievementPanel.position += Vector3.down * amount * Time.deltaTime / 1f;

            yield return null;
        }
        achievementIsShowing = false;
    }

    // 协程控制关卡开始倒计时
    public static void AreYouReady()
    {
        BgController.SuspendStart();
        instance.textReady.gameObject.SetActive(true);
        instance.GetReadyTime();
    }

    void GetReadyTime()
    {
        StartCoroutine(StartReadyTime());
        textReady.text = timeReady.ToString();
    }

    IEnumerator StartReadyTime()
    {
        while (timeReady >= 0)
        {
            textReady.text = timeReady.ToString();
            AudioManager.ReadyAudio();
            yield return new WaitForSeconds(1);
            timeReady--;
        }
        instance.timeReady = 3;
        instance.textReady.gameObject.SetActive(false);
        BgController.SuspendStop();
        PlayerHealth.AvatarOff();
    }

    /*

    public static void Defeat()
    {
        if (instance.coinCount < 5)
        {
            instance.BadEnd();
        }
        if (instance.coinCount > 5)
        {
            instance.HappyEnd();
        }
    }

    public static void Victory()
    {
        if (instance.finalBoss == false)
        {
            instance.finalBoss = true;
            instance.HappyEnd();
        }
    }

    private void HappyEnd()
    {
        instance.gameCount++;
        // Scene fades to ...
    }
    private void BadEnd()
    {
        instance.gameCount++;
        // Scene fades to ...
    }

    */

}

[System.Serializable]
public enum GameState
{
    // 枚举
    Playing,
    Paused,
    GameOver
}

[System.Serializable]
public class Achievement
{
    public string achievementName;
    public string achievementDescription;
    public bool unlocked;
}