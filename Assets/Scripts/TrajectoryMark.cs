using System.Collections;
using UnityEngine;

public class TrajectoryMark : MonoBehaviour {
    
    void Start() {
        transform.localScale = Vector3.zero;
        StartCoroutine(StartLife());
    }

    IEnumerator StartLife() {
        for(float t = 0; t <= 1.0f; t += 5 * Time.deltaTime) {
            transform.localScale = Vector2.Lerp(Vector3.zero, 0.1f * Vector3.one, t);
                yield return null;
        }
        yield return new WaitForSeconds(2f);
        for (float t = 0; t <= 1.0f; t += 5 * Time.deltaTime) {
            transform.localScale = Vector2.Lerp(0.1f * Vector3.one, Vector3.zero, t);
            yield return null;
        }
        Destroy(gameObject);
    }

}
