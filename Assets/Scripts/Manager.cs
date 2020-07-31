using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    Lifebar leftBar;
    Lifebar rightBar;

    RectTransform fight;

    RectTransform koText;
    Text win;

    int scoreLeft  = 0;
    int scoreRight = 0;

    Player     player;
    ComputerAI machine;

    Ball ball;

    bool ko = false;
    bool started = false;
    bool playerWin = true;

    void Start() {

        player = FindObjectOfType<Player>();
        machine = FindObjectOfType<ComputerAI>();

        fight = GameObject.Find("Fight").GetComponent<RectTransform>();

        leftBar  = GameObject.Find("LifeBar_left").GetComponent<Lifebar>();
        rightBar = GameObject.Find("LifeBar_right").GetComponent<Lifebar>();

        koText = GameObject.Find("KO").GetComponent<RectTransform>();
        koText.anchoredPosition = Vector2.up * 10000;

        win = GameObject.Find("KO/win").GetComponent<Text>();
     
        ball = FindObjectOfType<Ball>();

        ball.OutLeft  = AddDamageToLeft;
        ball.OutRight = AddDamageToRight;

        StartCoroutine(Starting());
        ball.FreezeBall();

    }

    void Update() {

        if (started) {
            if (!ko) {
                machine._Update();
                player._Update();
                ball._Update();
            } else {
                koText.anchoredPosition = Vector2.up * 127;
                ball.FreezeBall();
                if (Input.GetKeyDown(KeyCode.R)) {
                    // restart game
                    StartCoroutine(Starting());
                }
            }
        }
        
    }

    IEnumerator Starting() {
        started = false;
        fight.anchoredPosition = new Vector2(58, -50);
        koText.anchoredPosition = Vector2.up * 10000;
        yield return new WaitForSeconds(2f);
        fight.anchoredPosition = new Vector2(58, 99999);
        started = true;
        Reset();
    }

    void AddDamageToLeft() {
        leftBar.TakeLife(ball.speed * 10f);
        if(leftBar.Life() <= 0) {
            ko = true;
            win.text = "MACHINE WIN";
        }
    }

    void AddDamageToRight() {
        rightBar.TakeLife(ball.speed * 10f);
        if (rightBar.Life() <= 0) {
            ko        = true;
            win.text = "PLAYER WIN";
        }
    }

    void Reset() {

        ko = false;
        ball.ResetPosition();
        player.Reset();
        machine.Reset();
        leftBar.Reset();
        rightBar.Reset();

    }

}
