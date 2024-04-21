using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 3f;
    [SerializeField] private Boundary boundaryX;
    [SerializeField] private Boundary boundaryZ;
    [SerializeField] private Transform dice;
    [SerializeField] private ParticleSystem ps;

    private bool isMoving = false;
    public bool canMove = true;

    private void Update()
    {
        if (!canMove) return;
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.W) & transform.position.z < boundaryZ.max)
            Roll(Vector3.forward);

        else if (Input.GetKeyDown(KeyCode.S) & transform.position.z > boundaryZ.min)
            Roll(Vector3.back);

        else if (Input.GetKeyDown(KeyCode.D) & transform.position.x < boundaryX.max)
            Roll(Vector3.right);

        else if (Input.GetKeyDown(KeyCode.A) & transform.position.x > boundaryX.min)
            Roll(Vector3.left);
    }

    private void Roll(Vector3 dir)
    {
        transform.position += dir;
        dice.position -= dir;

        Vector3 anchor = dice.position + (Vector3.down + dir) * dice.localScale.x * 0.5f;
        Vector3 axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(RollCoroutine(anchor, axis));
    }

    private IEnumerator RollCoroutine(Vector3 anchor, Vector3 axis)
    {
        isMoving = true;

        for(int i = 0; i < (90 / rollSpeed); i++)
        {
            dice.RotateAround(anchor, axis, rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }

        AudioManager.Instance.PlaySound("StoneHit");

        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(SceneManager.GetActiveScene().buildIndex != 6)
        {
            if (canMove)
            {
                if (other.TryGetComponent(out Projectile projectile))
                {
                    FindObjectOfType<UIManager>().ShowDeathScreen();

                    canMove = false;

                    AudioManager.Instance.PlaySound("Explosion");
                    ps.Play();
                    Destroy(dice.gameObject);
                    Destroy(GetComponent<Collider>());
                }
            }
        }
    }
}