using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EagleController : MonoBehaviour
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

    public float timeRemaining;

    // Food
    private GameObject[] foodList;
    private int collectedFood = 0;

    // UI
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public GameObject pauseObj;
    public GameObject gameOverScreen;
    public GameObject winScreen;

    private bool isPaused = false;

    private float startingTime = 30.0f;

    // Game conditions
    public bool isAlive = true;
    private bool winCondition = false;

    #endregion
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;

        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);

        foodList = GameObject.FindGameObjectsWithTag("Food");

        timeRemaining = startingTime;
        pauseObj.SetActive(false);

        scoreText.text = "Score: " + collectedFood + "/" + foodList.Length;
        timeText.text = "Time: 30";
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



        if (timeRemaining <= 0)
        {
            GameOver();
        }
        else
        {
            timeRemaining -= Time.deltaTime;
            timeText.text = "Time: " + timeRemaining.ToString("f0");
        }

        if (isAlive && !winCondition)
        {
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
            GetLateralRotation();
        }

        if (Input.GetButtonDown("Cancel") && (winCondition || !isAlive))
        {
            //Debug.Log("Game Paused");
            //PauseGame();
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

    private void PauseGame()
    {
        isPaused = !isPaused;
        pauseObj.SetActive(true);
        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        timeRemaining = startingTime;
        transform.SetPositionAndRotation(startPosition, startRotation);
        rigidbody.isKinematic = false;
        isAlive = true;
        winCondition = false;
        collectedFood = 0;
        scoreText.text = "Score: " + collectedFood + "/" + foodList.Length;
        timeText.text = "Time: " + timeRemaining.ToString("f0");
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);
        foreach (GameObject food in foodList)
        {
            food.SetActive(true);
        }
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        isAlive = false;
        rigidbody.isKinematic = true;
        gameOverScreen.SetActive(true);
    }

    private void GameWon()
    {
        winCondition = true;
        rigidbody.isKinematic = true;
        winScreen.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            collectedFood++;
            scoreText.text = "Score: " + collectedFood + "/" + foodList.Length;
            if (collectedFood == foodList.Length)
            {
                GameWon();
            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            GameOver();
        }
    }
}
