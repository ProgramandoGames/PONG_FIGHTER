using System;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Action OutLeft;
    public Action OutRight;

    float leftLimit  = 0;
    float rightLimit = 0; 

    int collisionMask;

    public Vector2 velocity;
    Vector2 currentVelocity;
    Vector2 direction;

    public float speed = 12;
    float minSpeed = 12;
    float maxSpeed = 20;

    float increaseSpeedRate = 1.8f;

    float maxAngleChange = 15;
    float minAngleChange = 10;

    bool started = false;

    void Start() {

       leftLimit = -Camera.main.orthographicSize * Camera.main.aspect - transform.localScale.y / 2;
       rightLimit = Camera.main.orthographicSize * Camera.main.aspect + transform.localScale.y / 2;

       collisionMask = LayerMask.GetMask("Solid");

    }

    public void _Update() {

        speed += increaseSpeedRate * Time.deltaTime;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        CheckCollision(velocity.normalized);
        CheckCollision(Mathf.Sign(velocity.y) * Vector2.up * 0.2f);

        transform.position += (Vector3)velocity * Time.deltaTime;

        if (transform.position.x <= leftLimit) {
            OutLeft?.Invoke();
            ResetPosition();
        }
        if(transform.position.x >= rightLimit) {
            OutRight?.Invoke();
            ResetPosition();
        }

    }

    void CheckCollision(Vector2 directionToCheck) {

        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, directionToCheck, 0.2f, collisionMask);

        if (hit.collider != null) {

            velocity = Vector2.Reflect(velocity, hit.normal);

            IMovement movement = hit.collider.GetComponent<IMovement>();
            if (movement != null) {

                int direction = hit.collider.GetComponent<IMovement>().MovementDirection();

                Vector2 newDirection = velocity;

                float angle = Vector2.Angle(velocity, Vector2.right);

                if (direction != 0) {

                    var sign = Mathf.Sign(velocity.y);
                    var angleChange = angle + direction * sign * 30;

                    angleChange = Mathf.Clamp(angleChange, 10, 50);
                    newDirection = new Vector2(Mathf.Cos(angleChange * Mathf.Deg2Rad),
                                               Mathf.Sin(angleChange * Mathf.Deg2Rad));

                    newDirection.y *= sign;

                }

                velocity = newDirection.normalized * speed;

            }

        }
    }

    public void ResetPosition() {

       transform.position = Vector3.zero;

       float angle = UnityEngine.Random.Range(15, 45);

       direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),
                               Mathf.Sin(angle * Mathf.Deg2Rad));

       if (UnityEngine.Random.Range(0, 2) == 0) direction.x *= -1;
       if (UnityEngine.Random.Range(0, 2) == 0) direction.y *= -1;

       speed = minSpeed;
       velocity = direction * speed;
            
    }

    public void FreezeBall() {
        currentVelocity = velocity;
        velocity        = Vector2.zero;
        transform.position = Vector3.up * 1000;
    }

    public void FreeBall() {
        velocity = currentVelocity;
    }

}
