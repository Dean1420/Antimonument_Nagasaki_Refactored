using UnityEngine;
using PublisherSubscriber;


public class PublishSelectedPose : MonoBehaviour
{
    private bool canPress = true;

    public void OnButtonPressed()
    {
        if (!canPress) return;
        
        canPress = false;
        
        Debug.Log("STATUE >>> selected: " + this.name);
        SelectPoseEventHandler.Instance.PublishSelectedPose(this.name);
        
        Invoke(nameof(ResetPress), 1f);
    }
    
    private void ResetPress()
    {
        canPress = true;
    }

}
