using UnityEngine;
using UnityEngine.UIElements;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] private int _damage;           // 대미지
    [SerializeField] private float _interval;       // 시간 간격
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)  // 1. 시간 간격마다.
        {
            _timer = 0f;


            // 2. Clickable 게임 오브젝트를 모두 찾아와서 (여러분들은 캐싱하세요.)
            GameObject[] clickables = GameObject.FindGameObjectsWithTag("ClickTarget");

            foreach (GameObject clickable in clickables)
            {
                // 3. 클릭한다.
                IClickable clickableScript = clickable.GetComponent<IClickable>();
                ClickInfo clickInfo = new ClickInfo
                {
                    Type = EClickType.Auto,
                    Damage = _damage,
                };

                clickableScript.OnClick(clickInfo);
            }

        }
    }
}
