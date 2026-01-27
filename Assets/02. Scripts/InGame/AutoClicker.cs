using UnityEngine;
using UnityEngine.UIElements;

public class AutoClicker : MonoBehaviour
{      
    [SerializeField] private float _interval;   
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval) 
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
                    HeartsAmount = GameManager.Instance.HeartsPerClick,
                };

                clickableScript.OnClick(clickInfo);
            }

        }
    }
}
