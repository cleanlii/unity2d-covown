using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    static PlayerHealth current;

    int trapsLayer;
    int borderLayer;

    Rigidbody2D rg;

    public Text healthNum;
    public int health = 10;
    public SpriteRenderer front1A; //SpriteRenderer of Gameobject "front01-a" 
    public SpriteRenderer front1B; //SpriteRenderer of Gameobject "front01-b" 

    void Start()
    {
        current = this;

        trapsLayer = LayerMask.NameToLayer("Traps");
        borderLayer = LayerMask.NameToLayer("Border");

        rg = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == trapsLayer && health != 0)
        {
            // gameObject.SetActive(false);
            HealthDown();
            AudioManager.TrapAudio();
            GameManager.AcheivementCalculator("barrier");
        }
        else if (other.gameObject.layer == borderLayer)
        {
            // gameObject.SetActive(false);
            AudioManager.BorderAudio();
            HealthDown();
        }
    }

    private void Update()
    {
        if (health == 2) //Front color change while health sub.
        {
            front1A.color = Color.Lerp(front1A.color, Color.magenta, Time.deltaTime);
            front1B.color = Color.Lerp(front1B.color, Color.magenta, Time.deltaTime);
        }
        if (health == 1)
        {
            front1A.color = Color.Lerp(front1A.color, Color.red, Time.deltaTime);
            front1B.color = Color.Lerp(front1B.color, Color.red, Time.deltaTime);
        }
        if (health == 0)
        {
            gameObject.SetActive(false);
            MainMenuManager.RecordCurrentScene();
            MainMenuManager.LoadResultScene();
            GameManager.AcheivementCalculator("death");
        }

        // Leave the screen
        if (transform.position.x < -29)
        {
            health = 0;
        }

    }

    public static void HealthUp()
    {
        if (current.health != 0)
        {
            current.health += 1;
            current.healthNum.text = current.health.ToString();
        }

    }
    public static void HealthDown()
    {
        if (current.health != 0)
        {
            current.health -= 1;
            current.healthNum.text = current.health.ToString();
        }

    }
    public static void AvatarOn() // 无敌时间开始
    {
        current.rg.bodyType = RigidbodyType2D.Static;
        // Physics2D.IgnoreLayerCollision(current.trapsLayer, current.gameObject.layer, true);
    }

    public static void AvatarOff() // 无敌时间结束
    {
        current.rg.bodyType = RigidbodyType2D.Dynamic;
        // Physics2D.IgnoreLayerCollision(current.trapsLayer, current.gameObject.layer, false);
    }
}
