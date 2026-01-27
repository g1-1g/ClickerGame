using UnityEngine;

public class CatAnimationPlayer : MonoBehaviour, IFeedback
{
    private Animator _animator;
    private AnimatorOverrideController overrideController;
    private Cat _cat;
    private int _petCount;

    [SerializeField] private AnimationClip defaultIdleClip;
    [SerializeField] private AnimationClip defaultPetClip;
    //[SerializeField] private AnimationClip defaultLevelUpClip;

    private readonly int _petHash = Animator.StringToHash("Pet");
    private readonly int _levelUpHash = Animator.StringToHash("LevelUp");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _cat = GetComponent<Cat>();
        overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = overrideController;

        _cat.OnLevelChanged += AnimationInit;
    }
    public void AnimationInit(CatLevelDataSO data)
    {
        overrideController[defaultIdleClip] = data.IdleAnimation;
        overrideController[defaultPetClip] = data.PetAnimation;
        //overrideController[defaultLevelUpClip] = data.LevelUpAnimation;
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
        _cat.OnLevelChanged -= AnimationInit;
    }
}
