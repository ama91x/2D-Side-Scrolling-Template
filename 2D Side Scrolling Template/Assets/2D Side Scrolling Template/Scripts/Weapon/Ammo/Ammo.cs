using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ammo : MonoBehaviour, IFireable
{
    [SerializeField] private TrailRenderer _trailRenderer;

    private float _ammoRange;
    private float _ammoSpeed;
    private float _ammoChargeTime;

    private bool _isMaterialSet = false;
    private bool _overrideAmmoMovement;

    private SpriteRenderer _spriteRenderer;
    private AmmoDetailsSO _ammoDetails;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_ammoChargeTime > 0.0f)
        {
            _ammoChargeTime -= Time.deltaTime;
            return;
        }
        else if (!_isMaterialSet)
        {
            SetAmmoMaterial(_ammoDetails.AmmoMaterial);
            _isMaterialSet = true;
        }

        Vector3 distanceVector = new Vector3(_ammoSpeed * Time.deltaTime, 0, 0);

        transform.position += distanceVector;

        _ammoRange -= distanceVector.magnitude;

        if (_ammoRange < 0)
        {
            DisableAmmo();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DisableAmmo();
    }

    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float ammoSpeed, bool overrideAmmoMovement = false)
    {
        // Initialise Ammo
        this._ammoDetails = ammoDetails;

        _spriteRenderer.sprite = ammoDetails.AmmoSprite;

        if (ammoDetails.AmmoChargeTime > 0.0f)
        {
            _ammoChargeTime = ammoDetails.AmmoChargeTime;
            SetAmmoMaterial(ammoDetails.AmmoChargeMaterial);
            _isMaterialSet = false;
        }
        else
        {
            _ammoChargeTime = 0.0f;
            SetAmmoMaterial(ammoDetails.AmmoChargeMaterial);
            _isMaterialSet = true;
        }

        _ammoRange = ammoDetails.AmmoRange;
        this._ammoSpeed = ammoSpeed;

        this._overrideAmmoMovement = overrideAmmoMovement;

        gameObject.SetActive(true);

        // Initialise Trail

        if (ammoDetails.IsAmmoTrail)
        {
            _trailRenderer.gameObject.SetActive(true);
            _trailRenderer.emitting = true;
            _trailRenderer.material = ammoDetails.AmmoTrailsMaterial;
            _trailRenderer.startWidth = ammoDetails.AmmoTrailsStartWidth;
            _trailRenderer.endWidth = ammoDetails.AmmoTrailsEndWidth;
            _trailRenderer.time = ammoDetails.AmmoTrailTime;
        }
        else
        {
            _trailRenderer.emitting = false;
            _trailRenderer.gameObject.SetActive(false);
        }

    }

    private void SetAmmoMaterial(Material material)
    {
        _spriteRenderer.material = material;
    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
