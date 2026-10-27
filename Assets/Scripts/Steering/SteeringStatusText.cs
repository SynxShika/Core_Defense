using TMPro;
using UnityEngine;

public class SteeringStatusText : MonoBehaviour
{
    [SerializeField] private SteeringManager steeringManager;
    [SerializeField] private TMP_Text statusText;

    private void OnEnable()
    {
        steeringManager.OnBackendChanged += Refresh;
    }

    private void OnDisable()
    {
        steeringManager.OnBackendChanged -= Refresh;
    }

    private void Start()
    {
        Refresh(steeringManager.ActiveBackend);
    }

    private void Refresh(SteeringManager.SteeringBackend backend)
    {
        statusText.text = backend == SteeringManager.SteeringBackend.NativePlugin
            ? "Steering: C++ Plugin (F2)"
            : "Steering: C# (F2)";
    }
}
