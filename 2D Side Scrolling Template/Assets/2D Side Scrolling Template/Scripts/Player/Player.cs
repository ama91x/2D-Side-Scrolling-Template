using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#region REQUIRED COMPONENTS
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]

[DisallowMultipleComponent]
#endregion
public class Player : MonoBehaviour
{
    // Private
    private Health _health;
    private PlayerDetailsSO _playerDetails;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Accessors & Mutators
    public Health Healths
    {
        get => _health;
    }

    public PlayerDetailsSO PlayerDetails
    {
        get => _playerDetails;
    }

    private void Awake()
    {
        _health = GetComponent<Health>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this._playerDetails = playerDetails;

        SetPlayerHealth();
    }

    private void SetPlayerHealth()
    {
        _health.SetStartingHealth(_playerDetails.PlayerHealthAmount);
    }
}
