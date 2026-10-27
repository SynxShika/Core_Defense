using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class TempPlayerInput : MonoBehaviour
{
    private PlayerController _player;
    private Camera _cam;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _cam = Camera.main;
    }

    private void Update()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _player.Move(move);

        _player.AimAt(_cam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0)) _player.Shoot();
        if (Input.GetKeyDown(KeyCode.Space)) _player.Dash();
        if (Input.GetKeyDown(KeyCode.Return)) GameManager.Instance.SkipBuildPhase();
    }
}
