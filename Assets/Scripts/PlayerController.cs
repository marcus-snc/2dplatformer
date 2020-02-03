using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public Rigidbody rb;
    public Vector2 force;
    private Rigidbody2D rb;
    public bool grounded = true;
    // Start is called before the first frame update
    void Start() {
        force = new Vector2(1, 0);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {

    }
    void FixedUpdate() {
        if (Input.GetKey("d")) {
            //Debug.Log(Input.GetAxis("Horizontal"));
            //rb.AddForce(new Vector2(1,0)*10);
            rb.AddRelativeForce(Vector2.right * 75);
        }
        else if (Input.GetKey("a")) {
            //Debug.Log(Input.GetAxis("Horizontal"));
            //rb.AddForce(new Vector2(-1,0)*10);
            rb.AddRelativeForce(Vector2.left * 75);
        }

        if (Input.GetButton("Jump")) {
            if (grounded == true) {
                rb.AddRelativeForce(Vector2.up * 500);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Tilemap") {
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.name == "Tilemap") {
            grounded = false;
        }
    }
}
