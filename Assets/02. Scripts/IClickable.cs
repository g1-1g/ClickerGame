using Unity.VisualScripting;
using UnityEngine;

public interface IClickable 
{
    bool OnClick(IClickable clickInfo);
}
