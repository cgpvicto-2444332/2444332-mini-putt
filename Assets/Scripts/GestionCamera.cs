using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestionnaireCameraJeu : MonoBehaviour
{
    [Header("Caméras")]
    [SerializeField, Tooltip("Caméra libre contrôlée par le joueur (celle pilotée par CibleCamera)")]
    private CinemachineCamera cameraLibre;

    [SerializeField, Tooltip("Caméra fixe qui suit la balle une fois placée")]
    private CinemachineCamera cameraSuiviBalle;

    [SerializeField, Tooltip("Script qui gère les déplacements de la caméra libre (sur le même objet que cameraLibre)")]
    private CibleCamera controleCameraLibre;

    [Header("Priorités")]
    [SerializeField, Tooltip("Priorité de la caméra active")]
    private int prioriteActive = 20;

    [SerializeField, Tooltip("Priorité de la caméra inactive")]
    private int prioriteInactive = 0;

    [SerializeField]
    private Transform balle;

    private void Start()
    {
        ActiverCameraLibre();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            ActiverCameraSuiviBalle(balle);
        }
    }

    public void ActiverCameraLibre()
    {
        cameraLibre.Priority = prioriteActive;
        cameraSuiviBalle.Priority = prioriteInactive;

        if (controleCameraLibre != null)
        {
            controleCameraLibre.enabled = true;
        }
    }

    public void ActiverCameraSuiviBalle(Transform balle)
    {
        cameraSuiviBalle.Follow = balle;
        cameraSuiviBalle.LookAt = balle;

        cameraSuiviBalle.Priority = prioriteActive;
        cameraLibre.Priority = prioriteInactive;

        if (controleCameraLibre != null)
        {
            controleCameraLibre.enabled = false;
        }
    }
}