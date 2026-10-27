using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Transform fill;

    private void OnEnable()
    {
        if (health != null) health.OnHealthChanged += Refresh;
    }

    private void OnDisable()
    {
        if (health != null) health.OnHealthChanged -= Refresh;
    }

    private void Start()
    {
        Refresh(health.Current, health.Max);
    }

    private void Refresh(int current, int max)
    {
        float ratio = max > 0 ? (float)current / max : 0f;
        fill.localScale = new Vector3(ratio, 1f, 1f);
        fill.localPosition = new Vector3((ratio - 1f) * 0.5f, 0f, 0f);
    }
}
