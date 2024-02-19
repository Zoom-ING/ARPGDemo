using System;
using UnityEngine;

public class Skill_Weapon : MonoBehaviour
{
    [SerializeField] private Collider detectionCollider;
    private LayerMask attackDetectionLayer;
    private Action<Collider> onDetection;
    public void Init(LayerMask attackDetectionLayer, Action<Collider> onDetection)
    {
        detectionCollider.enabled = false;
        this.attackDetectionLayer = attackDetectionLayer;
        this.onDetection = onDetection;
    }
    public void StartDetection()
    {
        detectionCollider.enabled = true;
    }

    public void StopDetection()
    {
        detectionCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // 判断是否在LayerMask的范围内
        if ((attackDetectionLayer & 1 << other.gameObject.layer) > 0)
        {
            onDetection?.Invoke(other);
        }
    }
}
