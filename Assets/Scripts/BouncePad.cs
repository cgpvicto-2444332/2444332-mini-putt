using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField, Tooltip("Force du saut")]
    private float forceBounce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balle"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(transform.up * forceBounce, ForceMode.Impulse);
            }
        }
    }
}
