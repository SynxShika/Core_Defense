using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    [Header("Turrets")]
    [SerializeField] private Turret[] turretPrefabs;

    [Header("Grid")]
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 buildAreaMin = new Vector2(-11f, -5f);
    [SerializeField] private Vector2 buildAreaMax = new Vector2(11f, 5f);
    [SerializeField] private Vector2 occupancyCheckSize = new Vector2(0.85f, 0.85f);

    [Header("Preview")]
    [SerializeField] private SpriteRenderer ghost;
    [SerializeField] private Color validColor = new Color(0f, 1f, 0f, 0.4f);
    [SerializeField] private Color invalidColor = new Color(1f, 0f, 0f, 0.4f);

    private Camera _cam;
    private int _selectedIndex = -1;

    public Turret SelectedPrefab => _selectedIndex >= 0 ? turretPrefabs[_selectedIndex] : null;
    public bool CanBuild => GameManager.Instance.State == GameState.Build;

    private void Awake()
    {
        _cam = Camera.main;
        ghost.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!CanBuild || SelectedPrefab == null)
        {
            ghost.gameObject.SetActive(false);
            return;
        }

        Vector2 cell = GetMouseCell();
        bool valid = IsCellBuildable(cell) && GameManager.Instance.Scrap >= SelectedPrefab.Cost;

        ghost.gameObject.SetActive(true);
        ghost.transform.position = cell;
        ghost.color = valid ? validColor : invalidColor;
    }

    public void SelectTurret(int index)
    {
        if (index < 0 || index >= turretPrefabs.Length) return;
        _selectedIndex = _selectedIndex == index ? -1 : index;
    }

    public void ClearSelection()
    {
        _selectedIndex = -1;
    }

    public bool TryGetPlacementAtMouse(out Turret prefab, out Vector2 cell)
    {
        prefab = SelectedPrefab;
        cell = GetMouseCell();

        return CanBuild
            && prefab != null
            && IsCellBuildable(cell)
            && GameManager.Instance.Scrap >= prefab.Cost;
    }

    public Turret GetTurretAtMouse()
    {
        if (!CanBuild) return null;

        foreach (Collider2D hit in Physics2D.OverlapBoxAll(GetMouseCell(), occupancyCheckSize, 0f))
        {
            Turret turret = hit.GetComponentInParent<Turret>();
            if (turret != null) return turret;
        }

        return null;
    }

    public Turret PlaceTurret(Turret prefab, Vector2 cell)
    {
        if (!IsCellBuildable(cell)) return null;
        if (!GameManager.Instance.TrySpendScrap(prefab.Cost)) return null;

        Turret turret = Instantiate(prefab, cell, Quaternion.identity);
        turret.SourcePrefab = prefab;
        return turret;
    }

    public void RemoveTurret(Turret turret, bool refund)
    {
        if (turret == null) return;

        if (refund) GameManager.Instance.AddScrap(turret.Cost);
        turret.Remove();
    }

    public bool IsCellBuildable(Vector2 cell)
    {
        if (cell.x < buildAreaMin.x || cell.x > buildAreaMax.x) return false;
        if (cell.y < buildAreaMin.y || cell.y > buildAreaMax.y) return false;

        foreach (Collider2D hit in Physics2D.OverlapBoxAll(cell, occupancyCheckSize, 0f))
        {
            if (!hit.isTrigger) return false;
        }

        return true;
    }

    private Vector2 GetMouseCell()
    {
        Vector2 mouse = _cam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(
            Mathf.Round(mouse.x / cellSize) * cellSize,
            Mathf.Round(mouse.y / cellSize) * cellSize);
    }
}
