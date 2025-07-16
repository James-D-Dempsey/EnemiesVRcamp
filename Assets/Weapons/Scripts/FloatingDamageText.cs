using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 0.5f;
    public float duration = 2f;

    private TextMeshPro text;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        Destroy(gameObject, duration);
    }

    public void SetDamage(int amount)
    {
        if (text != null)
        {
            text.text = amount.ToString();
        }
    }

    void Update()
    {
        // Move upward over time
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Billboard toward camera
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180f, 0); // flip text if it's backward
        }
    }
}