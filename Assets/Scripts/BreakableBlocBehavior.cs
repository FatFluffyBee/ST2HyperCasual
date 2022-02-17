using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlocBehavior : MonoBehaviour
{
    public float velocityMinRequired, velocityRemoved;

    private BoxCollider colCube;
    private float minSpeed;
    private Rigidbody playerRb;

    private void Start()
    {
        colCube = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (GameManager.instance.playerCurrentBall != null)
        {
            minSpeed = GameManager.instance.playerCurrentBall.GetComponent<PlayerInput>().minSpeed;
            playerRb = GameManager.instance.playerCurrentBall.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (playerRb.velocity.magnitude > velocityMinRequired + minSpeed)
            {
                DestroyBloc();
                playerRb.velocity -= playerRb.velocity.normalized * velocityRemoved;
            }
            else
            {
                NotEnoughSpeedFb();
            }
        }
    }

    private void DestroyBloc()
    {
        Destroy(gameObject);
    }

    private void NotEnoughSpeedFb()
    {

    }
}
