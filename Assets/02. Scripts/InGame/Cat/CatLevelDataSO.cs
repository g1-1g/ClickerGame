using UnityEngine;

[CreateAssetMenu(fileName = "CatLevelData", menuName = "ScriptableObjects/CatLevelData", order = 0)]
public class CatLevelDataSO : ScriptableObject
{
    [Header("레벨 정보")]
    [Tooltip("레벨 Index")]
    public int Level;

    [Tooltip("이 레벨에 도달하기 위해 필요한 총 친밀도")]
    public int RequiredAffection;

    [Header("애니메이션")]
    [Tooltip("기본 Idle 애니메이션")]
    public AnimationClip IdleAnimation;

    [Tooltip("친밀도 증가 시 재생할 애니메이션")]
    public AnimationClip PetAnimation;

    [Tooltip("레벨업 시 재생할 애니메이션")]
    public AnimationClip LevelUpAnimation;

    [Header("레벨의 이름")]
    public string LevelName;
}
