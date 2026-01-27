using UnityEngine;

public class CatAnimationPlayer : MonoBehaviour, IFeedback
{
    private Animator _animator;
    private AnimatorOverrideController overrideController;
    private CatLevel _level;

    [SerializeField] private AnimationClip defaultIdleClip;
    [SerializeField] private AnimationClip defaultPetClip;
    //[SerializeField] private AnimationClip defaultLevelUpClip;

    private readonly int _petHash = Animator.StringToHash("Pet");
    private readonly int _levelUpHash = Animator.StringToHash("LevelUp");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _level = GetComponent<CatLevel>();
        overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = overrideController;

        _level.OnLevelChanged += AnimationInit;
    }
    public void AnimationInit(CatLevelDataSO data)
    {
        overrideController[defaultIdleClip] = data.IdleAnimation;
        overrideController[defaultPetClip] = data.PetAnimation;
        //overrideController[defaultLevelUpClip] = data.LevelUpAnimation;
    }

    public void PetTrigger()
    {
        _animator.SetTrigger(_petHash);
    }

    public void LevelUpTrigger()
    {
        _animator.SetTrigger(_levelUpHash);
    }

    public void Play(ClickInfo clickInfo)
    {
        PetTrigger();
    }

    private void OnDestroy()
    {
        _level.OnLevelChanged -= AnimationInit;
    }
}
