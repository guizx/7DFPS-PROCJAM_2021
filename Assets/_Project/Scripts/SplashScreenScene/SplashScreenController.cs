using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudomancerStudio
{
    public class SplashScreenController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup splashScreenLogo;
                [SerializeField] private float delayShow = 2f;

        [SerializeField] private float durationOffset;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private AudioSource splashScreenAudioSource;
        [SerializeField] private string nextSceneName;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(delayShow);
            splashScreenLogo.gameObject.SetActive(true);
            splashScreenAudioSource.Play();

            yield return new WaitForSeconds(splashScreenAudioSource.clip.length + durationOffset);
            yield return StartCoroutine(FadeOut(splashScreenLogo, 1f));
            yield return new WaitForSeconds(1f);

            loadingPanel.SetActive(true);
            SceneManager.LoadScene(nextSceneName);
        }


        private IEnumerator FadeOut(CanvasGroup canvasGroup, float fadeDuration)
        {
            float elapsed = 0f;
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / fadeDuration));
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }
    }
}
