using UnityEngine;

[RequireComponent(typeof(Health))]
public class CoreObjective : MonoBehaviour
{
    public Health Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<Health>();
        GameManager.Instance.RegisterCore(this);
    }
}
