using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
    }

    void Update()
    {
        // 📌 Déplacement avec l’accéléromètre
        Vector3 movement = new Vector3(Input.acceleration.x * speed, rb.velocity.y, 0);
        rb.velocity = movement;

        // 📌 Vérification du toucher sur la balle
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            DetectTouch();
        }

        // 📌 Saut avec ESPACE sur PC
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void DetectTouch()
    {
        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Touch détecté sur : " + hit.collider.gameObject.name);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("🟢 Balle au sol !");
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y <= 0)
        {
            isGrounded = true;
        }
    }
}
