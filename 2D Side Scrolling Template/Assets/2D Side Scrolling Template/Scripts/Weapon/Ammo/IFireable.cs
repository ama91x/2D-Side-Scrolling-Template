using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float ammoSpeed, bool overrideAmmoMovement = false);

    GameObject GetGameObject();
}
