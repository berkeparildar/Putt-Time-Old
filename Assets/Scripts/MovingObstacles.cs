using DG.Tweening;
using UnityEngine;

public class MovingObstacles : MonoBehaviour
{
    void Start()
    {
        switch (gameObject.tag)
        {
            case "RightLeft":
                var moveSeq = DOTween.Sequence();
                moveSeq.Append(transform.DOMoveX(1, 1).SetRelative().SetEase(Ease.Linear));
                moveSeq.Append(transform.DOMoveX(-1, 1).SetRelative().SetEase(Ease.Linear));
                moveSeq.Append(transform.DOMoveX(-1, 1).SetRelative().SetEase(Ease.Linear));
                moveSeq.Append(transform.DOMoveX(1, 1).SetRelative().SetEase(Ease.Linear));
                moveSeq.SetLoops(-1, LoopType.Restart);
                break;
            case "Rotating":
                var rotateSeq = DOTween.Sequence();
                rotateSeq.Append(transform.DORotate(new Vector3(0, 360, 0), 6).
                    SetEase(Ease.Linear).SetRelative());
                rotateSeq.SetLoops(-1, LoopType.Restart);
                break;
        }
    }
}