using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlocBehavior : MonoBehaviour
{
    public GameObject prefabBlocSplitted;
    public float velocityMinRequired, velocityRemoved, durationFeedback = 0.5f;
    public AnimationCurve curve;
    public Material matIdle, matTouch, matDestroyed;

    private MeshRenderer mR;
    private BoxCollider colCube;
    private float minSpeed, timerCount;
    private Rigidbody playerRb;

    private void Start()
    {
        colCube = GetComponent<BoxCollider>();
        mR = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (GameManager.instance.playerCurrentBall != null)
        {
            minSpeed = GameManager.instance.playerCurrentBall.GetComponent<PlayerInput>().minSpeed;
            playerRb = GameManager.instance.playerCurrentBall.GetComponent<Rigidbody>();
        }

        if (timerCount < durationFeedback)
        {
            timerCount += Time.deltaTime;
            mR.materials[1].Lerp(matTouch, matIdle, timerCount / durationFeedback);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (playerRb.velocity.magnitude > velocityRemoved + minSpeed)
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
        GetComponent<Collider>().isTrigger = true;
        Instantiate(prefabBlocSplitted, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

    private void NotEnoughSpeedFb()
    {
        timerCount = 0;
    }
}
