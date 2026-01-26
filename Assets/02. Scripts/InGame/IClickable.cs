using Unity.VisualScripting;
using UnityEngine;

public interface IClickable 
{
    bool OnClick(ClickInfo clickInfo);
}
