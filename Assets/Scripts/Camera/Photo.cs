/*
* Author: Hui Hui
* Date: 15/2/2025
* Description: Handles the behavior of a photo, including ejecting over time and setting images dynamically.
* */
using System.Collections;
using UnityEngine;

public class Photo : MonoBehaviour
{
    /// <summary>
    /// The MeshRenderer used to display the photo's texture.
    /// </summary>
    [SerializeField] private MeshRenderer imageRenderer;

    /// <summary>
    /// The collider attached to the photo.
    /// </summary>
    private Collider photoCollider;

    /// <summary>
    /// A reference to the ApplyPhysics component to enable/disable physics.
    /// </summary>
    private ApplyPhysics applyPhysics;

    /// <summary>
    /// Initializes references to the collider and ApplyPhysics component.
    /// </summary>
    private void Awake()
    {
        photoCollider = GetComponent<Collider>();
        applyPhysics = GetComponent<ApplyPhysics>();
    }

    /// <summary>
    /// Starts the ejecting process of the photo after a delay.
    /// </summary>
    private void Start()
    {
        StartCoroutine(EjectOverTime(1.5f));
    }


    /// <summary>
    /// Moves the photo over time, simulating an ejection effect.
    /// </summary>
    /// <param name="duration">The duration over which the ejection happens.</param>
    /// <returns>Returns an IEnumerator for use in a coroutine.</returns>
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

    /// <summary>
    /// Sets the texture of the photo's material.
    /// </summary>
    /// <param name="texture">The Texture2D to set as the photo's image.</param>
    public void SetImage(Texture2D texture)
    {
        if (imageRenderer != null && texture != null)
        {
            imageRenderer.material.color = Color.white;
            imageRenderer.material.mainTexture = texture;
        }
    }

    /// <summary>
    /// Enables physics for the photo and detaches it from its parent.
    /// </summary>
    public void EnablePhysics()
    {
        applyPhysics.EnablePhysics();
        transform.parent = null;
    }
}