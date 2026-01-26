using UnityEngine;

public class Clicker : MonoBehaviour
{

    [SerializeField] private int _damage = 10;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            TryClick(mousePos);
        }
    }
    

    private void TryClick(Vector2 mousePos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();

            ClickInfo clickInfo = new ClickInfo
            {
                Type = EClickType.Manual,
                Damage = _damage,
                Position = hit.point,
            };

            clickable?.OnClick(clickInfo);
        }
    }
}


