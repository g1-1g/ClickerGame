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
        Cat cat = GameManager.Instance.CurrentCat;
        
        Init();
    }

    private void Init()
    {
        Cat cat = GameManager.Instance.CurrentCat;

        cat.OnAffectionChanged -= AffectionUpdate;
        cat.OnLevelChanged -= LevelUpdate;
        cat.OnAffectionChanged += AffectionUpdate;
        cat.OnLevelChanged += LevelUpdate;
        
        cat.OnNameChanged -= CatNameUpdate;
        cat.OnNameChanged += CatNameUpdate;

        _affectionSlider.value = 0;
        _image.sprite = cat.Image;
        _nameText.text = cat.Name;
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

    public void CatUpdate(Cat cat)
    {
        _image.sprite = cat.Image;
        _nameText.text = cat.Name;
        LevelUpdate(cat.CurrentLevelData);
        AffectionUpdate(cat.AffectionRatio);
    }

    public void CatNameUpdate(string name)
    {
        _nameText.text = name;
    }

    public void OnDestroy()
    {
        Cat cat = GameManager.Instance.CurrentCat;

        cat.OnAffectionChanged -= AffectionUpdate;
        cat.OnLevelChanged -= LevelUpdate;
    }
}
