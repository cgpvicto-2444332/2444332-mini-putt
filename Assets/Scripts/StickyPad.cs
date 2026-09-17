using UnityEngine;

public class StickyPad : MonoBehaviour
{
    [SerializeField, Tooltip("Force du ralentissement")]
    private float ralentissement;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Balle"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity *= ralentissement;
            }
        }
    }
}