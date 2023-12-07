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

    [SerializeField] private LayerMask _collisionMask;

    // Private
    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;

    private RaycastOrigins _raycastOrigins;
    private CollisionInfo _collisions;

    private Player _player;

    public float Gravity
    {
        get => _gravity;
        set => _gravity = value;
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

        _collisions.Reser();

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
                velocity.x = (hit.distance - _skinWidth) * directionX;
                rayLength = hit.distance;

                _collisions.Left = directionX == -1;
                _collisions.Right = directionX == 1;
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

                _collisions.Below = directionY == -1;
                _collisions.Above = directionY == 1;
            }
        }
    }

    private void CalculateRaySpacing()
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

        public void Reser()
        {
            Above = Below = false;
            Left = Right = false;
        }
    }
}
