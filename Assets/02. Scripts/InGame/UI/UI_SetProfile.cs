using DG.Tweening;
using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetProfile : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private Slider _affectionSlider;

    [SerializeField] private float _doTweenDuration;
    void Start()
    {
        CatLevel cat = GameManager.Instance.CurrentCat;
        
        Init();
    }

    private void Init()
    {
        CatLevel cat = GameManager.Instance.CurrentCat;

        cat.OnAffectionChanged -= AffectionUpdate;
        cat.OnLevelChanged -= LevelUpdate;
        cat.OnAffectionChanged += AffectionUpdate;
        cat.OnLevelChanged += LevelUpdate;

        _affectionSlider.value = 0;
        _image.sprite = cat.LevelDatabase.Image;
        _nameText.text = cat.LevelDatabase.Name;
    }

    public void LevelUpdate(CatLevelDataSO data)
    {
        _levelText.text = $"Level. {data.Level}";
        _levelNameText.text = $"{data.LevelName}";
    }

    public void AffectionUpdate(float ratio)
    {
        _affectionSlider.DOValue(ratio, _doTweenDuration);
    }

    public void CatUpdate(CatLevel cat)
    {
        _image.sprite = cat.LevelDatabase.Image;
        _nameText.text = cat.LevelDatabase.Name;
        LevelUpdate(cat.CurrentLevelData);
        AffectionUpdate(cat.AffectionRatio);
    }

    public void OnDestroy()
    {
        CatLevel cat = GameManager.Instance.CurrentCat;

        cat.OnAffectionChanged -= AffectionUpdate;
        cat.OnLevelChanged -= LevelUpdate;
    }
}
