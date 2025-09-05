using System.Collections;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject three;
    [SerializeField] private GameObject two;
    [SerializeField] private GameObject one;

    [SerializeField] private LevelController levelController;

    [SerializeField] private AudioSource audioSource;

    private IEnumerator Start()
    {
        DesactiveAll();
        yield return new WaitForSeconds(1f);
        audioSource.Play();
        three.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DesactiveAll();
        yield return new WaitForSeconds(0.15f);
        audioSource.Play();
        two.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DesactiveAll();
        yield return new WaitForSeconds(0.15f);
        audioSource.Play();
        one.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DesactiveAll();
        levelController.StartGame();
    }

    private void DesactiveAll()
    {
        three.SetActive(false);
        two.SetActive(false);
        one.SetActive(false);
    }
}
