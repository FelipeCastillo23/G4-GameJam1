using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Movement
    public float speed = 15f;
    public float rotationSpeed = 50f;
    public float horizontalRotationSpeed = 50f;
    private float verticalInput;
    private float horizontalInput;

    // Horizontal rotation
    private float horizontalRotationInput;
    private float horizontalRotationCompensator = 4f;
    private float horizontalRotationCompensatorTime;

    // Starting position
    private Vector3 startPosition;
    private Quaternion startRotation;
    private new Rigidbody rigidbody;

    public bool isAlive = true;
    private bool winCondition = false;

    #endregion
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    void FixedUpdate()
    {
        if (isAlive && !winCondition)
        {
            movePlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && !winCondition)
        {
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
            GetLateralRotation();
        }

        if (Input.GetButtonDown("Cancel") && (winCondition || !isAlive))
        {
            RestartGame();
        }
    }

    private void GetLateralRotation()
    {
        horizontalRotationCompensatorTime = horizontalRotationCompensator * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
        {
            horizontalRotationInput += horizontalRotationCompensatorTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            horizontalRotationInput -= horizontalRotationCompensatorTime;
        }
        else
        {

            if (System.Math.Abs(horizontalRotationInput) < Time.deltaTime)
            {
                horizontalRotationInput = 0;
            }
            else if (horizontalRotationInput > 0)
            {
                horizontalRotationInput -= horizontalRotationCompensatorTime;
            }
            else if (horizontalRotationInput < 0)
            {
                horizontalRotationInput += horizontalRotationCompensatorTime;
            }
        }

        horizontalRotationInput = Mathf.Clamp(horizontalRotationInput, -1f, 1f);
    }

    void movePlayer()
    {
        // move the plane forward at a constant rate
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);

        // tilt the plane up/down based on up/down arrow keys
        transform.Rotate(verticalInput * Vector3.right * rotationSpeed * Time.fixedDeltaTime);
        transform.Rotate(-1 * horizontalInput * Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
        transform.Rotate(-1 * horizontalRotationInput * Vector3.up * horizontalRotationSpeed * Time.fixedDeltaTime);

    }

    private void RestartGame()
    {
        transform.SetPositionAndRotation(startPosition, startRotation);
        rigidbody.isKinematic = false;
        isAlive = true;
        winCondition = false;
    }

    private void GameOver()
    {
        isAlive = false;
        rigidbody.isKinematic = true;
    }

    private void GameWon()
    {
        winCondition = true;
        rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //GameWon();
    }

    private void OnCollisionEnter(Collision other)
    {
        GameOver();
    }
}
