using System.Collections;
using UnityEngine;

public class Photo : MonoBehaviour
{
    [SerializeField] private MeshRenderer imageRenderer;
    private Collider photoCollider;
    private ApplyPhysics applyPhysics;

    private void Awake()
    {
        photoCollider = GetComponent<Collider>();
        applyPhysics = GetComponent<ApplyPhysics>();
    }

    private void Start()
    {
        StartCoroutine(EjectOverTime(1.5f));
    }

    private IEnumerator EjectOverTime(float duration)
    {
        applyPhysics.DisablePhysics();
        photoCollider.enabled = false;

        float elapsedTime = 0f;
        Vector3 movementDirection = transform.forward * 0.1f;

        while (elapsedTime < duration)
        {
            transform.position += movementDirection * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        photoCollider.enabled = true;
    }

    public void SetImage(Texture2D texture)
    {
        if (imageRenderer != null && texture != null)
        {
            imageRenderer.material.color = Color.white;
            imageRenderer.material.mainTexture = texture;
        }
    }

    public void EnablePhysics()
    {
        applyPhysics.EnablePhysics();
        transform.parent = null;
    }
}