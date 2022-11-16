using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MovieController : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer player;
    public string SceneName;
    private bool playstart = false;
    void Start()
    {
        player.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // print(player.isPlaying);
        if (player.isPlaying)
        {
            playstart = true;
        }
        if (!player.isPlaying && playstart)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
            if (SceneName == "Chapter1" || SceneName == "Chapter2" || SceneName == "Chapter3" || SceneName == "Chapter4")
            {
                GameManager.AreYouReady();
                AudioManager.StartLevelAudio();
            }
        }

    }
}
