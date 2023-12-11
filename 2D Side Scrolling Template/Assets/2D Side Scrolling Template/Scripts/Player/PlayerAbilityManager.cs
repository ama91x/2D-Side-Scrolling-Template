using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    [SerializeField] private bool _canDash;
    [SerializeField] private bool _canSlide;

    public bool CanDash
    {
        get => _canDash;
        set => _canDash = value;
    }

    public bool CanSlide
    {
        get => _canSlide;
        set => _canSlide = value;
    }
}
