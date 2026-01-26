using UnityEngine;

public class TriggerReset : StateMachineBehaviour
{
    [SerializeField]
    private string _triggerName; 
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(_triggerName);
    }
}
