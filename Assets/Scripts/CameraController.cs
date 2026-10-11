using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Transform cameraTransform;
    public Vector3 cameraOffset;
    public float turnSpeed;
    public float tiltSpeed;
    public float minTilt;
    public float maxTilt;

    [Header("Camera Shake")]
    [SerializeField] private bool shakeCamera = true;
    [SerializeField] private float shakeTimePeriod = 0.25f;

    #region Local Variable
    private float currentTiltAngle = 0;

    private Vector3 shakeVector = Vector3.zero;

    private bool shaking = false;
    private bool isCameraActivated = true;

    private Coroutine cameraShakeCoroutine;
    #endregion

    private void FixedUpdate()
    {
        if (isCameraActivated)
            UpdateCamera();
    }

    public void SetActiveCamera(bool active) => isCameraActivated = active;

    private void UpdateCamera()
    {
        transform.position = player.transform.position + cameraOffset;

#if UNITY_EDITOR
        float turnX = Input.GetAxis("Mouse X");
        float turnY = Input.GetAxis("Mouse Y");
#else
        float turnX = Input.GetAxis("HorizontalR");
        float turnY = Input.GetAxis("VerticalR") * -1;
#endif
        //if (turnX > 0.5f || turnX < -0.5f)
        transform.rotation *= Quaternion.Euler(0, turnX * turnSpeed, 0);

        float desiredTilt = currentTiltAngle - turnY * tiltSpeed;
        currentTiltAngle = Mathf.Clamp(desiredTilt, minTilt, maxTilt);

        // Apply the tilt rotation to the camera
        cameraTransform.transform.localEulerAngles = new Vector3(currentTiltAngle, 0, 0);

        if (shaking)
            transform.position += Mathf.Sin(Time.realtimeSinceStartup * 100.0f) * shakeVector;
    }

    public void ShakeCamera(Vector3 shakeVector)
    {
        if (!shakeCamera)
            return;

        shaking = true;
        this.shakeVector = shakeVector;

        if (cameraShakeCoroutine != null)
            StopCoroutine(cameraShakeCoroutine);
        cameraShakeCoroutine = StartCoroutine(ShakeCameraCountdown());
    }

    private IEnumerator ShakeCameraCountdown()
    {
        yield return new WaitForSeconds(shakeTimePeriod);
        shaking = false;
    }
}
