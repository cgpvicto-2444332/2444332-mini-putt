using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField, Tooltip("Force de l'impulsion")]
    private float forceBoost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balle"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(-transform.forward * forceBoost, ForceMode.Impulse);
            }
        }
    }
}
