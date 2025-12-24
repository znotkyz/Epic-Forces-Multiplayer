using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManagerScript : MonoBehaviour
{
    InputManagerScript inputManager;
    PlayerMovementScript playerMovement;
    CameraManagerScript cameraManager;

    Animator animator;

    public bool isInteracting;

    PhotonView view;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManagerScript>();
        playerMovement = GetComponent<PlayerMovementScript>();
        cameraManager = FindObjectOfType<CameraManagerScript>();
    }

    private void Start()
    {
        if (!view.IsMine)
        {
            Destroy(GetComponentInChildren<CameraManagerScript>().gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine)
            return;

        inputManager.HandleAllInputs();
    }

    void FixedUpdate()
    {
        if (!view.IsMine)
            return;

        playerMovement.HandleAllMovement();
    }

    void LateUpdate()
    {
        if (!view.IsMine)
            return;

        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}
