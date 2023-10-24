using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float moveSpeed = 0.1f;
    //xd
    [SerializeField]
    [Space(10)]
    private float jumpForce = 6.0f;

    int score = 0;

    //private BigInteger scoreBig;

    private bool isWalking = false;

    private bool isFacingRight = true;

    private Rigidbody2D rigidBody;

    private Animator animator;

    public LayerMask groundLayer;

    public const float rayLenght = 1.3f;

    Vector3 theScale;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        isWalking = false;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            if (isFacingRight == false)
                Flip();
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            if (isFacingRight == true)
                Flip();
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
        }

        if (isGrounded() && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))) {
            isWalking = false;
            Jump();
        }

        animator.SetBool("isGrounded", isGrounded());
        animator.SetBool("isWalking", isWalking);

    }

    //Boy, the sound that whip makes sure is sweet. It's like Jesus gently snapping his fingers

    private void Jump() => rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private bool isGrounded() => 
        Physics2D.Raycast(transform.position, Vector2.down, rayLenght, groundLayer.value);

    private void Flip() {
        isFacingRight ^= true;
        theScale = transform.localScale; 
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bonus"))
        {
            other.CompareTag("Bonus");
            score += 10;
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        
        if(other.CompareTag("Finish"))
		{
            Debug.Log("You have finished the game with a score of " + score);
        }
        
    }
}