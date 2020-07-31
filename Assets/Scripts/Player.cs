using UnityEngine;

public class Player : MonoBehaviour, IMovement {

    float topLimit    = 0;
    float bottomLimit = 0;

    float speed = 10;

    float input;

    void Start() {

        topLimit    =  Camera.main.orthographicSize - transform.localScale.y/2;
        bottomLimit = -Camera.main.orthographicSize + transform.localScale.y/2;
        
    }

    public void _Update() {

        input = Input.GetAxisRaw("Vertical");

        transform.position += input * speed * Vector3.up * Time.deltaTime;

        transform.position = new Vector3(transform.position.x,
                                         Mathf.Clamp(transform.position.y, bottomLimit, topLimit));

    }

    public int MovementDirection() {
        return (int)input;
    }

    public void Reset() {
        transform.position = new Vector3(transform.position.x, 0);
    }


}
