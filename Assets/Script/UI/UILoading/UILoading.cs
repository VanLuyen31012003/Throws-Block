using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [SerializeField] private Image ImageSlide;

    public void PlayLoadSlide(Action actionCb=null)
    {
        this.gameObject.SetActive(true);
        ImageSlide.fillAmount = 0;
        StartCoroutine(FakeLoading(actionCb));
    }
    IEnumerator FakeLoading(Action actionCb = null)
    {
        float current = 0;

        while (current < 1)
        {
            float random = UnityEngine.Random.Range(0.3f, 0.6f);
            current += Time.deltaTime * random;
            ImageSlide.fillAmount = current;

            yield return null;
        }
        this.gameObject.SetActive(false);
        actionCb?.Invoke();
    }
}
