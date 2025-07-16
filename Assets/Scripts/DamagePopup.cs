using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float lifetime = 1f;         // total seconds before auto‑destroy
    public Vector3 riseSpeed = new Vector3(0, 1f, 0);
    public AnimationCurve fadeOverTime = AnimationCurve.Linear(0, 1, 1, 0);

    private TextMeshProUGUI _text;
    private float _elapsed = 0;

    void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        // move up
        transform.position += riseSpeed * Time.deltaTime;

        // fade out
        _elapsed += Time.deltaTime;
        float alpha = fadeOverTime.Evaluate(_elapsed / lifetime);
        var c = _text.color;
        c.a = alpha;
        _text.color = c;

        // lifetime check
        if (_elapsed >= lifetime)
            Destroy(gameObject);
    }

    /// <summary>
    /// Call this right after instantiating to set text and color.
    /// </summary>
    public void SetText(string msg, Color? color = null)
    {
        _text.text = msg;
        if (color.HasValue)
            _text.color = color.Value;
    }

    /// <summary>
    /// Static helper for convenience.
    /// </summary>
    public static void Create(GameObject prefab, Vector3 worldPos, string msg, Color? color = null)
    {
        var go = Instantiate(prefab, worldPos, Quaternion.identity);
        var popup = go.GetComponent<DamagePopup>();
        popup.SetText(msg, color);
    }
}
