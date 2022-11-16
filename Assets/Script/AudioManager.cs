using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("背景音乐")]
    public AudioClip BgClips;
    public AudioClip MenuClips;
    public AudioClip EndingClips;
    public AudioClip ReadyClips;

    [Header("角色音效")]
    public AudioClip SuspendClips;
    public AudioClip JumpClips;
    // public AudioClip DeathClips;
    // public AudioClip HurtClips;

    [Header("环境交互音效")]
    public AudioClip GetAtpClips; // 吃到ATP
    public AudioClip GetReceptorClips; // 吃到受体
    public AudioClip GetMacroClips; // 碰触巨噬细胞
    public AudioClip GetSpeedClips; // 短暂冲刺（加速）
    public AudioClip GetHealthClips; // 获得生命值补充
    public AudioClip GetSuspendClips; // 获得悬停补充
    public AudioClip ShootBulletClips; // 发射子弹
    public AudioClip TrapClips; // 碰撞陷阱（鼻毛）
    public AudioClip BorderClips; // 碰撞边界

    // [Header("UI音效")]
    // public AudioClip ButtonClips;
    // public AudioClip PauseClips;

    // 2 tracks
    AudioSource playerSource;
    AudioSource suspendSource;
    AudioSource musicSource;
    AudioSource interactSource;


    private void Awake()
    {
        if (current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;

        DontDestroyOnLoad(gameObject);

        playerSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        interactSource = gameObject.AddComponent<AudioSource>();
        suspendSource = gameObject.AddComponent<AudioSource>();

        current.suspendSource.volume *= 0.5f;

        MainMenuAudio();
    }
    public static void StartLevelAudio()
    {
        // current.musicSource.volume *= 0.5f;
        current.musicSource.clip = current.BgClips;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }

    public static void CloseLevelAudio()
    {
        current.musicSource.Stop();
    }
    public static void PauseLevelAudio()
    {
        if (current.musicSource.mute == true)
            current.musicSource.mute = false;
        else
            current.musicSource.mute = true;

    }

    public static void MainMenuAudio()
    {
        // current.musicSource.volume *= 0.5f;
        current.musicSource.clip = current.MenuClips;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }
    public static void EndingAudio()
    {
        current.musicSource.volume *= 0.5f;
        current.musicSource.clip = current.EndingClips;
        current.musicSource.Play();
    }

    public static void JumpAudio()
    {
        current.playerSource.clip = current.JumpClips;
        current.playerSource.Play();
    }
    public static void ReadyAudio()
    {
        current.playerSource.clip = current.ReadyClips;
        current.playerSource.Play();
    }

    public static void SuspendAudio()
    {
        current.suspendSource.clip = current.SuspendClips;
        current.suspendSource.Play();
    }

    public static void GetAtpAudio()
    {
        current.interactSource.clip = current.GetAtpClips;
        current.interactSource.Play();
    }
    public static void GetReceptorAudio()
    {
        current.interactSource.clip = current.GetReceptorClips;
        current.interactSource.Play();
    }
    public static void GetMacroAudio()
    {
        current.interactSource.clip = current.GetMacroClips;
        current.interactSource.Play();
    }
    public static void GetSpeedAudio()
    {
        current.interactSource.clip = current.GetSpeedClips;
        current.interactSource.Play();
    }
    public static void GetHealthAudio()
    {
        current.interactSource.clip = current.GetHealthClips;
        current.interactSource.Play();
    }
    public static void GetSuspendAudio()
    {
        current.interactSource.clip = current.GetSuspendClips;
        current.interactSource.Play();
    }
    public static void ShootBulletAudio()
    {
        current.interactSource.clip = current.ShootBulletClips;
        current.interactSource.Play();
    }
    public static void TrapAudio()
    {
        current.interactSource.clip = current.TrapClips;
        current.interactSource.Play();
    }
    public static void BorderAudio()
    {
        current.interactSource.clip = current.BorderClips;
        current.interactSource.Play();
    }

}
