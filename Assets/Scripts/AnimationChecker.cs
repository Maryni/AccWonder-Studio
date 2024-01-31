using UnityEngine;

public class AnimationChecker : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        this.gameObject.SetActive(false);
    }
}
