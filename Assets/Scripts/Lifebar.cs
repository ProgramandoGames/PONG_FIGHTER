using UnityEngine;

public class Lifebar : MonoBehaviour {

    RectTransform fill;

    Vector2 originalFill;
    Vector2 originalPosition;
    public int side = -1;

    void Start() {

        fill = transform.Find("fill").GetComponent<RectTransform>();
        originalFill = fill.sizeDelta;
        originalPosition = fill.anchoredPosition;
    }

    void Update() {
      
    }

    public void TakeLife(float amount) {
        fill.sizeDelta        -= Vector2.right * amount;
        fill.anchoredPosition -= side * (Vector2.right * amount / 2);
    }

    public float Life() {
        return fill.sizeDelta.x;
    }
    public void Reset() {
        fill.sizeDelta = originalFill;
        fill.anchoredPosition = originalPosition;
    }

}
