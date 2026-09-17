using UnityEngine;

public class SuiviPositionBalle : MonoBehaviour
{
    [SerializeField, Tooltip("La balle à suivre")]
    private Transform balle;

    private void LateUpdate()
    {
        if (balle != null)
            transform.position = balle.position;
    }
}