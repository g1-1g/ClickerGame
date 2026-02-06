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

    private CatManager _catManager;

    void Start()
    {
        _catManager = CatManager.Instance;
        
        Init();
    }

    private void Init()
    {
        _catManager.OnCatChanged += CatUpdate;
        _catManager.OnAffectionChanged += AffectionUpdate;
        _catManager.OnLevelChanged += LevelUpdate;
        _catManager.OnNameChanged += CatNameUpdate;
    }

    public void LevelUpdate(CatLevelSpecData data)
    {
        _levelText.text = $"Level. {data.Level}";
        _levelNameText.text = $"{data.LevelName}";
    }

    public void AffectionUpdate(float ratio)
    {
        _affectionSlider.DOComplete();

        _affectionSlider.DOValue(ratio, _doTweenDuration);
    }

    public void CatUpdate()
    {
        _affectionSlider.value = 0;
        _image.sprite = _catManager.Image;
        _nameText.text = _catManager.CurrentCat.Name;

        LevelUpdate(_catManager.CurrentLevelData);
        AffectionUpdate(CatManager.Instance.AffectionRatio);
    }

    public void CatNameUpdate(string name)
    {
        _nameText.text = name;
    }

    public void OnDestroy()
    {
        _catManager.OnAffectionChanged -= AffectionUpdate;
        _catManager.OnLevelChanged -= LevelUpdate;
        _catManager.OnNameChanged -= CatNameUpdate;
        _catManager.OnCatChanged -= CatUpdate;
    }
}
