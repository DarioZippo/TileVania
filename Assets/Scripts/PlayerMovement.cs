using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] Vector2 deathkick = new Vector2(10f, 10f);
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    private CinemachineImpulseSource myImpulseSource;
    float gravityScaleAtStart;

    bool isAlive = true;

    // Start is called before the first frame update
    void Start() {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myImpulseSource = GetComponent<CinemachineImpulseSource>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update() {
        if(!isAlive) {
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void OnMove(InputValue value) {
        if(!isAlive) {
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) {
        if(!isAlive) {
            return;
        }
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }
        if(value.isPressed) {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value) {
        if(!isAlive) {
            return;
        }
        if(value.isPressed) {
            myAnimator.SetBool("isShootingArrow", true);
            Quaternion arrowAngle = transform.rotation;
            arrowAngle.z = transform.localScale.x == -1 ? 180 : 0;
            Instantiate(arrow, bow.position, arrowAngle);
        }
    }

    void EndingShoot() {
        Debug.Log("Ending shoot");
        myAnimator.SetBool("isShootingArrow", false);
    }

    void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = (playerVelocity);

        myAnimator.SetBool("isRunning", Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon);
    }

    private void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder() {
        if(!myFeetCollider
            .IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = (climbVelocity);
        myRigidbody.gravityScale = 0;

        myAnimator.SetBool("isClimbing", Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon);
    }

    void Die() {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
            myImpulseSource.GenerateImpulse(1);
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = new Vector2(-Mathf.Sign(myRigidbody.velocity.x) * deathkick.x, deathkick.y);

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
