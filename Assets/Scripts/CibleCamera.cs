// Code emprunté de Alexandre Ouellet via : https://cours-alexandre-ouellet.github.io/jeux-3d/abc/gestion-camera/
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gère les déplacement de la cible de caméra. La caméra est une Cinemachine qui a comme cible ce gameObject.
/// </summary>
public class CibleCamera : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    [SerializeField, Tooltip("Vitesse de déplacement en m/s")]
    private float vitesseDeplacement;

    [SerializeField, Tooltip("Vitesse de rotation en deg/sec")]
    private float vitesseRotation;

    [SerializeField, Tooltip("Vitesse inclinaison en deg/sec")]
    private float vitesseInclinaison;

    [SerializeField, Tooltip("Angles d'inclinaison limites de la caméra. Doit être plus petit que le premier angle ou plus grand que le second")]
    private Vector2 limitesInclinaison;

    [SerializeField, Tooltip("Vitesse zoom de la caméra")]
    private float vitesseZoom;

    [SerializeField, Tooltip("Limites du zoom de la caméra")]
    private Vector2 limitesZoom;

    [Header("Références aux objets de jeu")]
    [SerializeField, Tooltip("Le PlayerInput qui gère les actions de la personne qui joue")]
    private PlayerInput controles;

    [SerializeField, Tooltip("Zone de confinement de la caméra")]
    private BoxCollider volumeCamera;

    [SerializeField, Tooltip("La caméra qui suit la cible")]
    private CinemachineCamera cameraGeree;

    private Vector2 deplacement;

    private float rotation;

    private float inclinaison;

    private float zoom;

    private void Start()
    {
        InputAction actionDeplacement = controles.actions.FindAction("player/DeplacerCamera");
        actionDeplacement.performed += CommencerDeplacement;
        actionDeplacement.canceled += TerminerDeplacement;

        InputAction actionRotation = controles.actions.FindAction("player/TournerCamera");
        actionRotation.performed += CommencerRotation;
        actionRotation.canceled += TerminerRotation;

        InputAction actionInclinaison = controles.actions.FindAction("player/InclinerCamera");
        actionInclinaison.performed += CommencerInclinaison;
        actionInclinaison.canceled += TerminerInclinaison;

        InputAction actionZoom = controles.actions.FindAction("player/ZoomerCamera");
        actionZoom.performed += CommencerZoom;
        actionZoom.canceled += TerminerZoom;
    }

    private void Update()
    {
        DeplacerCamera();
        TournerCamera();
        InclinerCamera();
        ZoomerCamera();
    }

    private void OnDestroy()
    {
        if (controles == null || controles.actions == null) { return; }

        InputAction actionDeplacement = controles.actions.FindAction("player/DeplacerCamera");
        actionDeplacement.performed -= CommencerDeplacement;
        actionDeplacement.canceled -= TerminerDeplacement;

        InputAction actionRotation = controles.actions.FindAction("player/TournerCamera");
        actionRotation.performed -= CommencerRotation;
        actionRotation.canceled -= TerminerRotation;

        InputAction actionInclinaison = controles.actions.FindAction("player/InclinerCamera");
        actionInclinaison.performed -= CommencerInclinaison;
        actionInclinaison.canceled -= TerminerInclinaison;

        InputAction actionZoom = controles.actions.FindAction("player/ZoomerCamera");
        actionZoom.performed -= CommencerZoom;
        actionZoom.canceled -= TerminerZoom;
    }

    private void CommencerDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = vitesseDeplacement * contexte.ReadValue<Vector2>();
    }

    private void TerminerDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = Vector2.zero;
    }

    private void DeplacerCamera()
    {
        if (deplacement.sqrMagnitude > 0.0f)
        {
            Vector3 prochainePosition = transform.position -
                transform.right * deplacement.x * Time.deltaTime -
                transform.forward * deplacement.y * Time.deltaTime;
            // Changé ici pour évité que la camera se tp vers le bas quand je la déplace pour la première fois.
            prochainePosition.y = transform.position.y;

            if (volumeCamera.bounds.Contains(prochainePosition))
            {
                transform.position = prochainePosition;
            }
        }
    }

    private void CommencerRotation(InputAction.CallbackContext contexte)
    {
        rotation = vitesseRotation * contexte.ReadValue<float>();
    }

    private void TerminerRotation(InputAction.CallbackContext contexte)
    {
        rotation = 0.0f;
    }

    private void TournerCamera()
    {
        transform.Rotate(new Vector3(0.0f, rotation * Time.deltaTime, 0.0f), Space.World);
    }

    private void CommencerInclinaison(InputAction.CallbackContext contexte)
    {
        inclinaison = vitesseInclinaison * contexte.ReadValue<float>();
    }

    private void TerminerInclinaison(InputAction.CallbackContext contexte)
    {
        inclinaison = 0.0f;
    }

    private void InclinerCamera()
    {
        float angle = (transform.localEulerAngles.x + inclinaison * Time.deltaTime) % 360;

        if (angle < limitesInclinaison.x || angle > limitesInclinaison.y)
        {
            transform.Rotate(new Vector3(inclinaison * Time.deltaTime, 0.0f, 0.0f), Space.Self);
        }
    }

    private void CommencerZoom(InputAction.CallbackContext contexte)
    {
        zoom = vitesseZoom * contexte.ReadValue<float>();
    }

    private void TerminerZoom(InputAction.CallbackContext contexte)
    {
        zoom = 0.0f;
    }

    private void ZoomerCamera()
    {
        CinemachinePositionComposer positionComposer = cameraGeree.GetComponent<CinemachinePositionComposer>();
        Vector3 offsetCamera = positionComposer.TargetOffset + positionComposer.TargetOffset.normalized * zoom;
        float distanceCamera = offsetCamera.magnitude;

        if (distanceCamera >= limitesZoom.x && distanceCamera <= limitesZoom.y)
        {
            positionComposer.TargetOffset = offsetCamera;
        }
    }
}