using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class CatchNonPayerPanelController : MonoBehaviour
{
    public Transform InfoPanel;

    public Sequence catchSequence;
    // Start is called before the first frame update
    private void OnEnable()
    {
        AISpawnController.CatchNonPayer += CatchPanelActive;
    }

    private void OnDisable()
    {
        AISpawnController.CatchNonPayer -= CatchPanelActive;
    }

    public void CatchPanelActive()
    {
        InfoPanel.gameObject.SetActive(true);
        if (catchSequence != null)
        {
            catchSequence.Kill();
        }
        catchSequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one*1.2f, 0.5f))  // 1 saniyede büyüt
            .Append(transform.DOScale(Vector3.one, 0.5f)) // 1 saniyede küçült
            .SetLoops(-1, LoopType.Yoyo); // Sonsuz döngüyle ileri geri devam et
    }

    public void StopCatchPanel()
    {
        if (catchSequence != null)
        {
            catchSequence.Kill();
        }
        InfoPanel.gameObject.SetActive(false);

    }
}
