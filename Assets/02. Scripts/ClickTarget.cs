using UnityEngine;

public class ClickTarget : MonoBehaviour, IClickable
{
    public bool OnClick(IClickable clickInfo)
    {
        Debug.Log($"{gameObject.name}: 다음부터는 늦지 않겠습니다.");

        return true;
    }
}
