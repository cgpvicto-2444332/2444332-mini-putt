using UnityEngine;
using UnityEngine.InputSystem;

public class Balle : MonoBehaviour
{
    [SerializeField, Tooltip("Force de la frappe")]
    private float forceFrappe;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null) 
            return;

        // https://docs.unity3d.com/Packages/com.unity.inputsystem@0.1/manual/HowDoI.html
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // https://discussions.unity.com/t/adding-impulse-based-to-an-object-using-other-objects-orientation/586707
            rb.AddForce(Vector3.right * forceFrappe, ForceMode.Impulse);
        }
    }
}
