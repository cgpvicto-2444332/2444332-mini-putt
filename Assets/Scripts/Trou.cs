using UnityEngine;

public class Trou : MonoBehaviour
{
    [Header("Récompense & Réinitialisation")]
    [SerializeField, Tooltip("Point de départ où la balle sera téléportée")]
    private Transform pointDepart;

    [Header("Gestion du Drapeau")]
    [SerializeField, Tooltip("Le GameObject du drapeau à faire disparaître")]
    private GameObject drapeau;

    [SerializeField, Tooltip("Distance à laquelle le drapeau disparaît")]
    private float distanceMasquerDrapeau = 3.0f;

    private Transform balleTransform;

    private void Update()
    {
        GérerVisibilitéDrapeau();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Détecter si l'objet qui entre dans le trigger est la balle
        if (other.CompareTag("Balle") || other.GetComponent<Rigidbody>() != null)
        {
            TeleporterBalle(other.gameObject);
        }
    }

    private void TeleporterBalle(GameObject balle)
    {
        // 2. Réinitialiser la vitesse physique de la balle
        Rigidbody rb = balle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 3. Téléporter la balle au point de départ
        if (pointDepart != null)
        {
            balle.transform.position = pointDepart.position;
            balle.transform.rotation = pointDepart.rotation;
        }

        // Réafficher le drapeau pour le prochain essai
        if (drapeau != null)
        {
            drapeau.SetActive(true);
        }
    }

    private void GérerVisibilitéDrapeau()
    {
        if (drapeau == null) return;

        // Si la balle n'est pas encore trouvée dans la scène, on la cherche
        if (balleTransform == null)
        {
            GameObject balleObj = GameObject.FindWithTag("Balle");
            if (balleObj != null) balleTransform = balleObj.transform;
            return;
        }

        // 4. Calcul de la distance entre la balle et le trou
        float distance = Vector3.Distance(transform.position, balleTransform.position);

        // 5. Faire disparaître le drapeau si la balle est proche
        if (distance <= distanceMasquerDrapeau)
        {
            if (drapeau.activeSelf) drapeau.SetActive(false);
        }
        else
        {
            if (!drapeau.activeSelf) drapeau.SetActive(true);
        }
    }
}