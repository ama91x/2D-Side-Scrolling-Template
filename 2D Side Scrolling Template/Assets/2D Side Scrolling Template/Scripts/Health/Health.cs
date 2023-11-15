using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Private
    private int _startingHealth;
    private int _currentHealth;

    public void SetStartingHealth(int startingHealth)
    {
        this._startingHealth = startingHealth;
        _currentHealth = startingHealth;
    }

    public int GetStartingHealth()
    {
        return _startingHealth;
    }
}
