using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 15f;
    private bool isTraveling;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;

    public int minSwipeRecognition = 500;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;
    private Color solveColor;

    private AudioSource playerAudio;

    public ParticleSystem ballParticle;
    public AudioClip moveSound;

    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //rb.velocity = speed * Vector3.forward;
        if (isTraveling)
        {
            rb.velocity = speed * travelDirection;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        int i = 0;
        while(i < hitColliders.Length)
        {
            GroundPieceController ground = hitColliders[i].transform.GetComponent<GroundPieceController>();
            if(ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if(nextCollisionPosition != Vector3.zero)
        {
            if(Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }
        if (isTraveling) return;
        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
            if(swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;
                if(currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }
                currentSwipe.Normalize();
                //UP/DOWN
                if(currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    ballParticle.Play();
                    playerAudio.PlayOneShot(moveSound, 1.0f);
                    //GO UP
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);

                }
                if(currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    ballParticle.Play();
                    playerAudio.PlayOneShot(moveSound, 1.0f);
                    //GO Left/Right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }

            }
            swipePosLastFrame = swipePosCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,direction,out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }
        isTraveling = true;
    }
}
