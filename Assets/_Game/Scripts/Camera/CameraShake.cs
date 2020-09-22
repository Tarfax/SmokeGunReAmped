//Grabbed from Asset Store:
// https://assetstore.unity.com/packages/tools/particles-effects/camera-shake-fx-146554

using MC_Utility;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    private float shakeAmount;

    private Vector3 lastPosition;
    private Vector3 lastRotation;

    [Tooltip("Exponent for calculating the shake factor. Useful for creating different effect fade outs")]
    [SerializeField] private float shakeExponent = 1;

    [Tooltip("Maximum angle that the gameobject can shake. In euler angles.")]
    [SerializeField] private Vector3 maximumAngularShake = Vector3.one * 5;

    [Tooltip("Maximum translation that the gameobject can receive when applying the shake effect.")]
    [SerializeField] private Vector3 maximumTranslationShake = Vector3.one * .75f;

    public void Start() {
        EventSystem<ShakeCameraEvent>.RegisterListener(OnShakeCameraEvent);
    }

    private void OnShakeCameraEvent(ShakeCameraEvent data) {
        shakeAmount = Mathf.Clamp01(data.ShakeAmount);
    }

    private void Update() {
        if (shakeAmount > 0f) {
            ShakeCamera();
        }
    }

    private void ShakeCamera() {
        float shake = Mathf.Pow(shakeAmount, shakeExponent);
        /* Only apply this when there is active trauma */
        if (shake > 0) {
            var previousRotation = lastRotation;
            var previousPosition = lastPosition;
            /* In order to avoid affecting the transform current position and rotation each frame we substract the previous translation and rotation */
            lastPosition = new Vector3(
                maximumTranslationShake.x * (Mathf.PerlinNoise(0, Time.time * 25) * 2 - 1),
                maximumTranslationShake.y * (Mathf.PerlinNoise(1, Time.time * 25) * 2 - 1),
                maximumTranslationShake.z * (Mathf.PerlinNoise(2, Time.time * 25) * 2 - 1)
            ) * shake;

            lastRotation = new Vector3(
                maximumAngularShake.x * (Mathf.PerlinNoise(3, Time.time * 25) * 2 - 1),
                maximumAngularShake.y * (Mathf.PerlinNoise(4, Time.time * 25) * 2 - 1),
                maximumAngularShake.z * (Mathf.PerlinNoise(5, Time.time * 25) * 2 - 1)
            ) * shake;

            transform.localPosition += lastPosition - previousPosition;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + lastRotation - previousRotation);
            shakeAmount = Mathf.Clamp01(shakeAmount - Time.deltaTime);
        }
        else {
            if (lastPosition == Vector3.zero && lastRotation == Vector3.zero) return;
            /* Clear the transform of any left over translation and rotations */
            transform.localPosition -= lastPosition;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles - lastRotation);
            lastPosition = Vector3.zero;
            lastRotation = Vector3.zero;
        }
    }

    private void OnDisable() {
        EventSystem<ShakeCameraEvent>.UnregisterListener(OnShakeCameraEvent);
    }

}