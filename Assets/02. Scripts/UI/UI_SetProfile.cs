using DG.Tweening;
using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_SetProfile : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private UnityEngine.UI.Slider _affectionSlider;

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
        _catManager.OnAffectionUp += AffectionUpdate;
    }

    public void LevelUpdate(CatLevelSpecData data)
    {
        _levelText.text = $"Level. {data.Level}";
        _levelNameText.text = $"{data.LevelName}";

        _affectionSlider.DOValue(1, _doTweenDuration).OnComplete(() =>
        _affectionSlider.DOValue(_catManager.CurrentCat.AffectionRatio, _doTweenDuration));
    }


    public void AffectionUpdate(bool isLevelUp)
    {
        _affectionSlider.DOKill();

        if (isLevelUp)
        {
            LevelUpdate(_catManager.CurrentCat.GetLevelData());
        }
        _affectionSlider.DOValue(_catManager.CurrentCat.AffectionRatio, _doTweenDuration);
    }

    public void CatUpdate()
    {
        _affectionSlider.value = 0;
        _image.sprite = _catManager.CurrentCat.CatSpecData.Image;
        _nameText.text = _catManager.CurrentCat.Name;

        LevelUpdate(_catManager.CurrentCat.GetLevelData());
        AffectionUpdate(false);
    }

    public void CatNameUpdate(string name)
    {
        _nameText.text = name;
    }

    public void OnDestroy()
    {
        _catManager.OnAffectionUp -= AffectionUpdate;
        _catManager.OnCatChanged -= CatUpdate;
    }
}
