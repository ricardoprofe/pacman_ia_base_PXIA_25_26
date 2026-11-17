using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;
    private InputSystem_Actions _input;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();
    }
    
    private void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    private void Update()
    {
        // Set the new direction based on the current input
        if (_input.Player.Move.ReadValue<Vector2>().y > 0) {
            movement.SetDirection(Vector2.up);
        }
        else if (_input.Player.Move.ReadValue<Vector2>().y < 0) {
            movement.SetDirection(Vector2.down);
        }
        else if (_input.Player.Move.ReadValue<Vector2>().x < 0) {
            movement.SetDirection(Vector2.left);
        }
        else if (_input.Player.Move.ReadValue<Vector2>().x > 0) {
            movement.SetDirection(Vector2.right);
        }

        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }

}
