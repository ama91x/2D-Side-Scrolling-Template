using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup _cinemachineTargetGroup;

    private void Awake()
    {
        _cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        // SetCinemachineTargetGroup();
    }

    // private void SetCinemachineTargetGroup()
    // {
    //     CinemachineTargetGroup.Target cinemachienTargetGroupeTarget_Player = new CinemachineTargetGroup.Target
    //     {
    //         weight = 1f,
    //         radius = 1f,
    //         target = GameManager.Instance.GetPlayer().transform
    //     };

    //     CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] { cinemachienTargetGroupeTarget_Player };

    //     _cinemachineTargetGroup.m_Targets = cinemachineTargetArray;
    // }
}
