using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    ///////////////////////////
    // Variable declarations //
    ///////////////////////////

    public Vector2 force;
    private Rigidbody2D rb;

    // Ground check area points
    public GameObject GgroundPointA, GgroundPointB;
    public Vector2 groundPointA, groundPointB;

    // Left wall check area points
    public GameObject GleftWallPointA, GleftWallPointB;
    public Vector2 leftWallPointA, leftWallPointB;

    // Right wall check area points
    public GameObject GrightWallPointA, GrightWallPointB;
    public Vector2 rightWallPointA, rightWallPointB;

    public Collider2D[] overlaps = new Collider2D[4];
    public LayerMask layer;

    public int grounded;
    public int leftWallHang;
    public int rightWallHang;

    public bool justHung;

    // Start is called before the first frame update
    void Start() {
        force = new Vector2(1, 0);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        CollisionCheck(); // Call collision check function

        if (Input.GetButtonDown("Jump")) { // Jump is here instead of FixedUpdate because GetButtonDown likes it better here? not sure
            if (grounded > 0 || justHung == true || leftWallHang == 1 || rightWallHang == 1) {
                rb.AddRelativeForce(Vector2.up * 1000);
            }
        }

        /*if (leftWallHang == 1 || rightWallHang == 1) {
            rb.gravityScale = 2;
        }*/

        // Check if player just came off wall hang
        if (rb.gravityScale == 2 && Input.GetKey("d") == false && Input.GetKey("d") == false) {
            rb.gravityScale = 10;
            StartCoroutine(PostHangJumpTime());
        }
        // Check if player is midair, resetting previous antigravity if so
        else if (leftWallHang == 0 && rightWallHang == 0 && grounded == 0) {
            rb.gravityScale = 10;
        }
        else if (leftWallHang == 1 || rightWallHang == 1) {
            StartCoroutine(PostHangJumpTime());
        }
    }
    void FixedUpdate() {
        //////////////
        // Controls //
        //////////////
        
        if (Input.GetKey("d")) {
            //Debug.Log(Input.GetAxis("Horizontal"));
            //rb.AddForce(new Vector2(1,0)*10);
            if (rightWallHang == 0) {
                rb.AddRelativeForce(Vector2.right * 75);
            }
            else if (rightWallHang == 1 && grounded == 0) {
                rb.gravityScale = 2;
            }
        }
        else if (Input.GetKey("a")) {
            //Debug.Log(Input.GetAxis("Horizontal"));
            //rb.AddForce(new Vector2(-1,0)*10);
            if (leftWallHang == 0) {
                rb.AddRelativeForce(Vector2.left * 75);
            }
            else if (leftWallHang == 1 && grounded == 0) {
                rb.gravityScale = 2;
            }
        }
    }
    void CollisionCheck() { // Translate GameObject point markers to Vector2, and calculate if overlapping with level
        groundPointA = GgroundPointA.transform.position;
        groundPointB = GgroundPointB.transform.position;

        leftWallPointA = GleftWallPointA.transform.position;
        leftWallPointB = GleftWallPointB.transform.position;

        rightWallPointA = GrightWallPointA.transform.position;
        rightWallPointB = GrightWallPointB.transform.position;

        grounded = Physics2D.OverlapAreaNonAlloc(groundPointA, groundPointB, overlaps, layer);
        leftWallHang = Physics2D.OverlapAreaNonAlloc(leftWallPointA, leftWallPointB, overlaps, layer);
        rightWallHang = Physics2D.OverlapAreaNonAlloc(rightWallPointA, rightWallPointB, overlaps, layer);
    }
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
