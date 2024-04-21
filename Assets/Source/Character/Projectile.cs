using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem ps1;
    [SerializeField] private ParticleSystem ps2;
    [SerializeField] private GameObject graphic;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
            Destroy(gameObject, 20f);
        else
            Destroy(gameObject, 5f);
        StartCoroutine(SpawnCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        if(SceneManager.GetActiveScene().buildIndex == 6)
        {
            graphic.SetActive(false);
            yield return new WaitForSeconds(0.4f);
            graphic.SetActive(true);
            yield return new WaitForSeconds(3f);
            GetComponent<Rigidbody>().velocity = transform.forward * 3f;
        }
        else
        {
            graphic.SetActive(false);
            yield return new WaitForSeconds(0.4f);
            graphic.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }

    private IEnumerator DieCoroutine()
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(graphic);
        ps2.Play();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
