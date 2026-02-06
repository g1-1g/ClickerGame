using UnityEngine;

public class CatAnimationPlayer : MonoBehaviour, IFeedback
{
    private Animator _animator;
    private AnimatorOverrideController overrideController;
    private int _petCount;

    [SerializeField] private AnimationClip defaultIdleClip;
    [SerializeField] private AnimationClip defaultPetClip;
    //[SerializeField] private AnimationClip defaultLevelUpClip;

    private readonly int _petHash = Animator.StringToHash("Pet");
    private readonly int _levelUpHash = Animator.StringToHash("LevelUp");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        
        overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = overrideController;

    }

    void Start()
    {
        CatManager.Instance.OnAffectionUp += PlayPetAnimation;
        CatManager.Instance.OnCatChanged += AnimationInit;
    }
    public void AnimationInit()
    {
        overrideController[defaultIdleClip] = CatManager.Instance.CurrentCat.GetLevelData().IdleAnimation;
        overrideController[defaultPetClip] = CatManager.Instance.CurrentCat.GetLevelData().PetAnimation;
        //overrideController[defaultLevelUpClip] = data.LevelUpAnimation;
    }

    public void PlayPetAnimation(bool isLevelUp)
    {
        if (isLevelUp)
        {
            LevelUpTrigger();
            AnimationInit();
        }
        else
        {
            SetPetBool(true);
        }
    }

    public void SetPetBool(bool value)
    {
        _animator.SetBool(_petHash, value);
    }

    public void LevelUpTrigger()
    {
        _animator.SetTrigger(_levelUpHash);
    }

    public void Play(ClickInfo clickInfo)
    {
        _petCount++;
        SetPetBool(true);
    }

    public void OnPlayEnd()
    {
        if (_petCount <= 1)
        {
            SetPetBool(false);
        }
        _petCount = 0;
    }

    private void OnDestroy()
    {
        CatManager.Instance.OnAffectionUp -= PlayPetAnimation;
    }
}
