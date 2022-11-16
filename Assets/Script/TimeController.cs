using System.Collections;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    static TimeController instance;

    [SerializeField, Range(0f, 5f)] float bulletTimeScale = 5f;

    float defaultFixedDeltaTime;
    float timeScaleBeforePause;
    float t;

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

    public static void Pause()
    {
        instance.timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    public static void Unpause()
    {
        Time.timeScale = instance.timeScaleBeforePause;
    }

    public static void BulletTime(float duration)
    {
        Time.timeScale = instance.bulletTimeScale;
        instance.StartCoroutine(instance.SlowOutCoroutine(duration));
    }

    public static void BulletTime(float inDuration, float outDuration)
    {
        instance.StartCoroutine(instance.SlowInAndOutCoroutine(inDuration, outDuration));
    }

    public static void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        instance.StartCoroutine(instance.SlowInKeepAndOutDuration(inDuration, keepingDuration, outDuration));
    }

    public static void SlowIn(float duration) // 时间慢慢变化
    {
        instance.StartCoroutine(instance.SlowInCoroutine(duration));
    }

    public static void SlowOut(float duration) // 时间慢慢恢复
    {
        instance.StartCoroutine(instance.SlowOutCoroutine(duration));
    }

    // 利用协程实现渐变效果，duration表示渐变持续时间
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;

        while (t < 2f)
        {
            if (GameManager.GameState != GameState.Paused) // 判断游戏状态
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t); // 插值
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }

            yield return null; // 下一帧继续执行
        }
    }

    IEnumerator SlowOutCoroutine(float duration)
    {
        PlayerHealth.AvatarOn();
        t = 0f;
        while (t < 2f)
        {
            if (GameManager.GameState != GameState.Paused) // 判断游戏状态
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t); // 插值
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale; // 修改固定帧时间
            }

            yield return null; // 下一帧继续执行
        }
        PlayerHealth.AvatarOff();
    }

    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)
    {

        // 无敌时间
        PlayerHealth.AvatarOn();
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);

        StartCoroutine(SlowOutCoroutine(outDuration));
        PlayerHealth.AvatarOff();
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        PlayerHealth.AvatarOn();
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        StartCoroutine(SlowOutCoroutine(outDuration));
        PlayerHealth.AvatarOff();
    }

}
