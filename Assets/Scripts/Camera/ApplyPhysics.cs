/*
* Author: Hui Hui
* Date: 15/2/2025
* Description: Physics application onto gameobjects
* */
using UnityEngine;

public class ApplyPhysics : MonoBehaviour
{
    /// <summary>
    /// The Rigidbody component of the GameObject, used to apply physics.
    /// </summary>
    private Rigidbody rigidBody = null;

    /// <summary>
    /// Stores the original collision detection mode of the Rigidbody before any changes.
    /// </summary>
    private CollisionDetectionMode originalMode = CollisionDetectionMode.Discrete;

    /// <summary>
    /// Initializes the Rigidbody and stores the original collision detection mode.
    /// </summary>
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        originalMode = rigidBody.collisionDetectionMode;
    }

    /// <summary>
    /// Enables physics on the GameObject by restoring its original collision detection mode, enabling gravity, and disabling kinematic mode.
    /// </summary>
    public void EnablePhysics()
    {
        rigidBody.collisionDetectionMode = originalMode;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
    }

    /// <summary>
    /// Disables physics on the GameObject by setting the collision detection mode to Discrete, disabling gravity, and enabling kinematic mode.
    /// </summary>
    public void DisablePhysics()
    {
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }
}