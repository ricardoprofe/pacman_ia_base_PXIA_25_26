using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostMovement : MonoBehaviour
{
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;

    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }
    
    public Transform target;
    public float nextWaypointDistance = 3f;
    
    private Path path;
    private Seeker seeker;
    private int currentWaypoint = 0;
    private int nextWayPoint = 1;
    private bool reachedEndOfPath = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        
        ResetState();
    }
    
    public void OnPathComplete(Path p)
    {
        // The path is now calculated!
        
        //Check for errors
        if (p.error)
        {
            Debug.LogError("Error calculating path: " + p.errorLog);
            return;
        }
        
        path = p; // Store the calculated path
        currentWaypoint = 0; // Reset waypoint index
        nextWayPoint = 1;
        
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rb.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
        
        
        //If no path yet, return
        if (path == null)
            return;
        
        //Check if we reached the end of the path and return if so
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        
        // Get the next waypoint from the path
        
        //Move to the next waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
        





    }

    private void FixedUpdate()
    {
        
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Only set the direction if the tile in that direction is available
        // otherwise we set it as the next direction so it'll automatically be
        // set when it does become available
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        // If no collider is hit then there is no obstacle in that direction
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }

}
