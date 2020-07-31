
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    RectTransform[] options;

    Image insertCoin;

    float speed = 4;
    bool started = false;

    void Start() {

        insertCoin = GameObject.Find("insertCoin").GetComponent<Image>();

    }

    void Update() {

       insertCoin.color = new Color(1, 1, 1, Mathf.Round(Mathf.PingPong(speed * Time.time, 1)));

       if(Input.GetKeyDown(KeyCode.Space) && !started) {
            speed = 7;
            StartCoroutine(Started());
            started = true;
       }

    }

    IEnumerator Started() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }



}
