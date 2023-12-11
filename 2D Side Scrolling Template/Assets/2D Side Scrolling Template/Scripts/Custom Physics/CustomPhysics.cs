using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    // Constent
    private const float _skinWidth = 0.015f;

    // Inspector Assigne
    [Header("Physics Propart")]
    [SerializeField][Range(2, 10)] private int _horizontalRayCount = 4;
    [SerializeField][Range(2, 10)] private int _verticalRayCount = 4;

    [Space(2)]
    [SerializeField] private float _gravity = -20;
    [SerializeField] private float _gravityMultiplier = 2f;
    [SerializeField] private float _maxGravity = -20;

    [Space(2)]
    [SerializeField] private float _maxClimbAngle;
    [SerializeField] private float _maxDescendAngle;

    [Space(2)]
    [Tooltip("Used to check if there is any collisions")]
    [SerializeField] private LayerMask _collisionMask;

    [Tooltip("Used to check if there is up collisions if player is sliding or any other usess")]
    [SerializeField] private LayerMask _upCollisionMask;

    // Private
    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;
    private float _hitSizeX;

    private RaycastOrigins _raycastOrigins;
    private CollisionInfo _collisions;

    private Player _player;

    public float Gravity
    {
        get => _gravity;
        set => _gravity = value;
    }

    public float GravityMultiplier
    {
        get => _gravityMultiplier;
    }

    public float MaxGravity
    {
        get => _maxGravity;
        set => _maxClimbAngle = value;
    }

    public float HitSizeX
    {
        get => _hitSizeX;
    }

    public float MaxClimbAngle
    {
        get => _maxClimbAngle;
    }

    public CollisionInfo CollisionInfos
    {
        get => _collisions;
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Start()
    {
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        _collisions.Reset();
        _collisions.VelocityOld = velocity;

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    private void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + _skinWidth;

        for (int i = 0; i < _horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= _maxClimbAngle)
                {
                    if (_collisions.DescendingSlope)
                    {
                        _collisions.DescendingSlope = false;
                        velocity = _collisions.VelocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != _collisions.SlopAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - _skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlop(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!_collisions.ClimbingSlope || slopeAngle > _maxClimbAngle)
                {
                    velocity.x = (hit.distance - _skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (_collisions.ClimbingSlope)
                    {
                        velocity.y = Mathf.Tan(_collisions.SlopeAngel * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    _collisions.Left = directionX == -1;
                    _collisions.Right = directionX == 1;
                }
            }
        }
    }

    private void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + _skinWidth;

        for (int i = 0; i < _verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (_verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, _collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - _skinWidth) * directionY;
                rayLength = hit.distance;

                if (_collisions.ClimbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(_collisions.SlopeAngel * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                _collisions.Below = directionY == -1;
                _collisions.Above = directionY == 1;
            }
        }

        if (_collisions.ClimbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + _skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != _collisions.SlopeAngel)
                {
                    velocity.x = (hit.distance - _skinWidth) * directionX;
                    _collisions.SlopeAngel = slopeAngle;
                }
            }
        }
    }

    public bool CollisionCheckFromUpove()
    {
        float rayLength = 0.5f;
        bool isHit = false;

        for (int i = 0; i < _verticalRayCount; i++)
        {
            Vector2 rayOrigin = _raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (_verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * rayLength, _upCollisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);

            if (hit)
            {
                _hitSizeX = hit.transform.localScale.x;

                Debug.Log(hit.transform.localScale.x.ToString());

                isHit = true;
            }
        }

        return isHit;
    }

    private void ClimbSlop(ref Vector3 velocity, float slopAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);

            _collisions.Below = true;
            _collisions.ClimbingSlope = true;
            _collisions.SlopeAngel = slopAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);

        Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.BottomRight : _raycastOrigins.BottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, _collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            Debug.Log("Direction X: " + directionX + "Normal hit: " + Mathf.Sign(hit.normal.x));

            if (slopeAngle != 0 && slopeAngle <= _maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - _skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        _collisions.SlopeAngel = slopeAngle;
                        _collisions.DescendingSlope = true;
                        _collisions.Below = true;
                    }
                }
            }
        }
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = _player.BoxCollider.bounds;
        bounds.Expand(_skinWidth * -2);

        _horizontalRayCount = Mathf.Clamp(_horizontalRayCount, 2, int.MaxValue);
        _verticalRayCount = Mathf.Clamp(_verticalRayCount, 2, int.MaxValue);

        _horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = _player.BoxCollider.bounds;
        bounds.Expand(_skinWidth * -2);

        _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
    }

    public struct CollisionInfo
    {
        public bool Above, Below;
        public bool Left, Right;

        public bool ClimbingSlope;
        public bool DescendingSlope;

        public float SlopeAngel, SlopAngleOld;

        public Vector3 VelocityOld;

        public void Reset()
        {
            Above = Below = false;
            Left = Right = false;

            ClimbingSlope = false;
            DescendingSlope = false;

            SlopAngleOld = SlopeAngel;
            SlopeAngel = 0;
        }
    }
}
