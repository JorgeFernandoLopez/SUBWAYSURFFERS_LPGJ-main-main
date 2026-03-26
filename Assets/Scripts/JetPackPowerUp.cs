using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

public class JetPackPowerUp : MonoBehaviour
{
    [SerializeField]
    private float flyingHeight = 4.5f;
      [SerializeField]
      private Character character;
        [SerializeField]
        private GameObject jetpackAsset;
          [SerializeField]
         private float flyDuration = 5f;
           [SerializeField]
           private UnityEvent onJetpackActivated;
           private Coroutine flyCoroutine;
         private void Awake()
    {
        jetpackAsset.SetActive(false);

    }
    public void Activate()
    {
        if ( !character.IsActive || character.IsFlying) return;
        character.IsFlying = true;
        jetpackAsset.SetActive(true);
        onJetpackActivated?.Invoke();
        character.CharacterRigidbody.isKinematic = true;
        character.transform.DOMoveY(flyingHeight, 1f).SetEase(Ease.OutQuad);
        flyCoroutine = StartCoroutine(DeactivateJetPack());
    }
    private IEnumerator DeactivateJetPack()
    {
        yield return new WaitForSeconds(flyDuration);
        Deactivate();
    }
    public void Deactivate()
    {
        if (flyCoroutine != null)
        {
            StopCoroutine(flyCoroutine);
            flyCoroutine = null;
        }
        character.IsFlying = false;
        jetpackAsset.SetActive(false);
        character.CharacterRigidbody.isKinematic = false;
        character.CharacterAnimator.Play(character.CharacterData.jumpAnimationName);
    }


}
