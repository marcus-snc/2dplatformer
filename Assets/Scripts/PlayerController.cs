using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    ///////////////////////////
    // Variable Declarations //
    ///////////////////////////

    private Rigidbody2D rb;

    public int grounded;
    public int leftWallHang;
    public int rightWallHang;

    public bool justHung;

    // Collision Variables //

    // Ground check area points
    public GameObject GgroundPointA, GgroundPointB;
    public Vector2 groundPointA, groundPointB;

    // Left wall check area points
    public GameObject GleftWallPointA, GleftWallPointB;
    public Vector2 leftWallPointA, leftWallPointB;

    // Right wall check area points
    public GameObject GrightWallPointA, GrightWallPointB;
    public Vector2 rightWallPointA, rightWallPointB;

    // Overlap results array
    public Collider2D[] overlaps = new Collider2D[4];
    // Main level layer mask
    public LayerMask layer;

    // Start is called before the first frame update
    void Start() {
        // Get rigidbody of current object
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        CollisionCheck(); // Call collision check function

        //////////
        // Jump //
        //////////
        
        // Jump is here instead of FixedUpdate because GetButtonDown likes it better here? not sure
        if (Input.GetButtonDown("Jump")) {
            // This code is an absolute MESS please ignore
            if (grounded == 1 || justHung == true || leftWallHang == 1 || rightWallHang == 1) {
                // If jumping from left surface
                if (leftWallHang == 1) {
                    rb.AddRelativeForce(Vector2.up * 1000);
                    // Only jump away from surface if not on the ground
                    if (grounded == 0) {
                        rb.AddRelativeForce(Vector2.right * 700);
                    }
                }
                // If jumping from right surface
                else if (rightWallHang == 1) {
                    rb.AddRelativeForce(Vector2.up * 1000);
                    // Only jump away from surface if not on the ground
                    if (grounded == 0) {
                        rb.AddRelativeForce(Vector2.left * 700);
                    }
                }
                // If not jumping from hanging position on left/right surface
                else if (leftWallHang == 0 && rightWallHang == 0) {
                    rb.AddRelativeForce(Vector2.up * 1000);
                }
            }
        }

        // Old anti-gravity hang code
        /*if (leftWallHang == 1 || rightWallHang == 1) {
            rb.gravityScale = 2;
        }*/

        // Check if player just came off wall hang
        if (rb.gravityScale == 2 && Input.GetKey("a") == false && Input.GetKey("d") == false) {
            rb.gravityScale = 10;
            StartCoroutine(PostHangJumpTime()); // Start post-hang jump window timer
        }
        // Check if player is midair, resetting previous antigravity if so
        else if (leftWallHang == 0 && rightWallHang == 0 && grounded == 0) {
            rb.gravityScale = 10;
        }
        // Create extra post-hang jump window for quality of life when briefly touching wall before jumping off
        else if (leftWallHang == 1 || rightWallHang == 1) {
            StartCoroutine(PostHangJumpTime());
        }
    }

    void FixedUpdate() {
        //////////////
        // Controls //
        //////////////
        
        // Couldn't figure out how to use Input Manager value so used plain key instead
        if (Input.GetKey("d")) {
            // Add force to the right if player is in normal state
            if (rightWallHang == 0) {
                rb.AddRelativeForce(Vector2.right * 75);
            }
            // If player is hanging onto a wall while off the ground, slow gravity to simulate "cling" to wall
            else if (rightWallHang == 1 && grounded == 0) {
                rb.gravityScale = 2;
            }
        }
        else if (Input.GetKey("a")) {
            if (leftWallHang == 0) {
                rb.AddRelativeForce(Vector2.left * 75);
            }
            else if (leftWallHang == 1 && grounded == 0) {
                rb.gravityScale = 2;
            }
        }
    }

    void CollisionCheck() {
        // Translate GameObject point markers to Vector2 (probably very slow and inefficient but I couldn't figure it out yet)
        groundPointA = GgroundPointA.transform.position;
        groundPointB = GgroundPointB.transform.position;

        leftWallPointA = GleftWallPointA.transform.position;
        leftWallPointB = GleftWallPointB.transform.position;

        rightWallPointA = GrightWallPointA.transform.position;
        rightWallPointB = GrightWallPointB.transform.position;

        // Check if area created by point A to B (below character cube) overlaps with Layer Mask (level) and throws overlap results into overlaps array
        grounded = Physics2D.OverlapAreaNonAlloc(groundPointA, groundPointB, overlaps, layer);
        leftWallHang = Physics2D.OverlapAreaNonAlloc(leftWallPointA, leftWallPointB, overlaps, layer);
        rightWallHang = Physics2D.OverlapAreaNonAlloc(rightWallPointA, rightWallPointB, overlaps, layer);
    }

    // Creates brief window for player to jump while midair after hanging onto a wall
    IEnumerator PostHangJumpTime() {
        justHung = true;
        yield return new WaitForSeconds(0.25f);
        justHung = false;
    }

    // OLD COLLISION CHECK METHOD
    /*void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Tilemap") {
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.name == "Tilemap") {
            grounded = false;
        }
    }*/
}
