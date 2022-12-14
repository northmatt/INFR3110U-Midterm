using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
    public float moveForce = 0f;
    public float jumpForce = 0f;
    public float groundedDot = 0.7f;
    public float cursorSensitivy = 1f;
    public float gravMult = 1f;

    public GameObject projectile;
    public Transform projectilePos;

    public static float shootFrequency = 1f;

    private PlayerAction playerInput;
    private Rigidbody rBody;
    private List<GameObject> groundedObjects = new List<GameObject>();
    private Vector2 inputs = Vector2.zero;
    private byte jump = 0;
    private bool grounded = false;
    private float shootCooldown = 0f;

    private void Start() {
        playerInput = GameController.instance.playerInput;

        playerInput.Player.Move.performed += cntxt => inputs = cntxt.ReadValue<Vector2>();
        playerInput.Player.Move.canceled += cntxt => inputs = Vector2.zero;

        playerInput.Player.Jump.performed += cntxt => TryJump();

        playerInput.Player.Shoot.performed += cntxt => Shoot();

        rBody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (shootCooldown > 0f)
            shootCooldown = Mathf.Max(shootCooldown - Time.deltaTime, 0f);
    }

    private void FixedUpdate() {
        grounded = groundedObjects.Count > 0;

        rBody.AddForce(moveForce * Time.fixedDeltaTime * inputs.x * transform.right + moveForce * Time.fixedDeltaTime * inputs.y * transform.forward);

        if (jump == 4) {
            rBody.AddForce(0f, jumpForce, 0f, ForceMode.VelocityChange);
            --jump;
        }

        //reduce jump cooldown if grounded
        if (jump > 0 && grounded)
            --jump;

        //Add downwards force if ungrounded (RB3D has drag)
        if (!grounded)
            rBody.AddForce(Physics.gravity * gravMult, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision) {
        CheckCollisionNormal(collision);
    }

    private void OnCollisionStay(Collision collision) {
        CheckCollisionNormal(collision);
    }

    private void OnCollisionExit(Collision collision) {
        //Is late, takes a few frames to call OnColExit
        groundedObjects.Remove(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Finish"))
            return;

        ++GameController.instance.collectables;
        Destroy(other.gameObject);
    }

    private void CheckCollisionNormal(Collision collision) {
        //upto 64 contact points, unlikely to ever need >20. List has dynamic scaling
        //List<ContactPoint> contactPoints = new List<ContactPoint>();
        ContactPoint[] contactPoints = new ContactPoint[20];
        int contactPointsCount = collision.GetContacts(contactPoints);

        //loop through all contacts and compare contact normal to a specific range
        for (int i = 0; i < contactPointsCount; ++i)
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > groundedDot) {
                if (!groundedObjects.Contains(collision.gameObject)) groundedObjects.Add(collision.gameObject);
                return;
            }

        groundedObjects.Remove(collision.gameObject);
    }

    private void TryJump() {
        if (jump == 0 && grounded)
            jump = 4;
    }

    private void Shoot() {
        //if (EditorManager.instance.editorMode) return;

        if (shootCooldown > 0f) return;

        shootCooldown = 1f / shootFrequency;

        Rigidbody bulletRb = Instantiate(projectile, projectilePos.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * 32f + transform.up * 1f, ForceMode.Impulse);
    }
}
