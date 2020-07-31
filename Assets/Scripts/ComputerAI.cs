using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerAI : MonoBehaviour {

    enum STATES { WAITING, REASONING, ACTING }

    Transform trajectoryMarkPrefab;

    List<Vector3> trajectoryPoints = new List<Vector3>();

    STATES state;

    Ball ball;

    float topLimit = 0;
    float bottomLimit = 0;
    float rightLimit = 0;

    float currentPosition;
    float targetPosition;

    float distanceToReact     = 0f;
    float uncertaintyPosition = 0.8f;
    float speed = 10f;

    float randomMove = 0;
    float randomMoveInterval = 1.0f;

    public int side = 1;

    bool showTrajectory = false;

    void Start() {

        trajectoryMarkPrefab = Resources.Load<Transform>("trajectoryMark");

        state = STATES.WAITING;

        ball = FindObjectOfType<Ball>();

        topLimit = Camera.main.orthographicSize;
        bottomLimit = -Camera.main.orthographicSize;

        rightLimit = Camera.main.orthographicSize * Camera.main.aspect + transform.localScale.y / 2;

        randomMove = Time.time + Random.Range(0, 1.5f);


    }

    public void _Update() {

        switch (state) {

            case STATES.WAITING:

                // Se a bolinha está se movendo pra direita (1° condição) e a posição dela ultrapassou o 
                // limite pra IA reagir (2° condição), então, muda o estado pra "REAGINDO"
                if (side == 1) {
                    if (Mathf.Sign(ball.velocity.x) > 0 && ball.transform.position.x >=  distanceToReact) {
                        state = STATES.REASONING;
                    }
                } else {
                    if (Mathf.Sign(ball.velocity.x) < 0 && ball.transform.position.x <= -distanceToReact) {
                        state = STATES.REASONING;
                    }
                }
                // Move a raquete para a posição da bolinha caso o tempo ultrapassou o delay de movimento (1° condição)
                // e se a posição da bola é menor que a distancia para reagir
                if (side == 1) {
                    if (Time.time >= randomMove && ball.transform.position.x <= distanceToReact) {
                        targetPosition = ball.transform.position.y;
                        randomMove = Time.time + randomMoveInterval;
                    }
                } else {
                    if (Time.time >= randomMove && ball.transform.position.x >= -distanceToReact) {
                        targetPosition = ball.transform.position.y;
                        randomMove = Time.time + randomMoveInterval;
                    }
                }
               // Aplica o movimento
               GoToPosition();
               break;

            case STATES.REASONING:

               // calcula a trajetória
               SimulateBallTrajectory();

               currentPosition = transform.position.y;
               
               // Adiciona uma incerteza na posição final. Essa incerteza faz com que a raquete
               // não rebata a bolinha sempre no centro, adicionando mais natualidade
               float targetUp = targetPosition + uncertaintyPosition;
               float targetDown = targetPosition - uncertaintyPosition;
               targetPosition = Random.Range(targetDown, targetUp);

               // após tudo terminar, muda o estado pra AGINDO
               state = STATES.ACTING;
               break;

            case STATES.ACTING:
               // move-se para a posição final calculada na simulação 
               GoToPosition();

                // caso a bolinha seja rebativa (1° condição) ou ultrapasse o limite da tela, volta
                // pro estado ESPERANDO
                if (side == 1) {
                    if (Mathf.Sign(ball.velocity.x) < 0 || ball.transform.position.x >= 8.5f) {
                        state = STATES.WAITING;
                    }
                } else {
                    if (Mathf.Sign(ball.velocity.x) > 0 || ball.transform.position.x <= -8.5f) {
                        state = STATES.WAITING;
                    }
                }
               break;
        }

    }
    
    // Faz a raquete se mover suavemente para a posição alvo, respeitando os limites da tela
    void GoToPosition() {

        currentPosition = Mathf.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, currentPosition);

        transform.position = new Vector3(transform.position.x,
                                Mathf.Clamp(transform.position.y,
                                bottomLimit + transform.localScale.y / 2,
                                topLimit - transform.localScale.y / 2));

    }

    void SimulateBallTrajectory() {

        int iterations = 400;
        float step     = 0.01f;

        Vector2 position = ball.transform.position;
        Vector2 velocity = ball.velocity;

        while (iterations > 0) {

            position += velocity * step;

            if(position.y >= topLimit || position.y <= bottomLimit) {
                velocity.y *= -1;
            }

            if (side == 1) {
                if (position.x >= 8.2f) {
                    targetPosition = position.y;
                    break;
                }
            } else {
                if (position.x <= -8.2f) {
                    targetPosition = position.y;
                    break;
                }
            }

            --iterations;

            if (showTrajectory)
                trajectoryPoints.Add(new Vector3(position.x, position.y, 1f));
            
        }
        StartCoroutine(DrawTrajectory());
    }

    // Desenha a trajetória simulada do movimento da bola
    IEnumerator DrawTrajectory() {

        for (int i = 0; i < trajectoryPoints.Count; ++i) {
            Instantiate(trajectoryMarkPrefab, trajectoryPoints[i], Quaternion.identity);
            yield return null;
        }
        trajectoryPoints.Clear();

    }

    public void Reset() {
        transform.position = new Vector3(transform.position.x, 0);
    }

}
