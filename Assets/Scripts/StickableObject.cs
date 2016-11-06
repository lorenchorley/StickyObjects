using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class StickableObject : MonoBehaviour {

    HashSet<StickyObject> StuckInObjects;

    Rigidbody2D rb;
    DynamicFriction dynamicFriction;
    StaticFriction staticFriction;

    FrictionManager currentFriction;

    float dragOriginal;
    float angularDragOriginal;
    float gravityOriginal;

    void Awake() {
        StuckInObjects = new HashSet<StickyObject>();
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        dynamicFriction = GetComponent<DynamicFriction>();
        staticFriction = GetComponent<StaticFriction>();

        if (dynamicFriction == null)
            dynamicFriction = gameObject.AddComponent<DynamicFriction>();
        if (staticFriction == null)
            staticFriction = gameObject.AddComponent<StaticFriction>();

        // Disable both types of friction
        dynamicFriction.enabled = false;
        staticFriction.enabled = false;

        // Save normal values
        dragOriginal = rb.drag;
        angularDragOriginal = rb.angularDrag;
        gravityOriginal = rb.gravityScale;

    }

    public FrictionManager GetFrictionManager() {
        if (dynamicFriction.enabled)
            return dynamicFriction;
        else if (staticFriction.enabled)
            return staticFriction;
        else
            return null;
    }

    public void EnterObject(StickyObject obj) {
        if (StuckInObjects.Contains(obj))
            return;

        StuckInObjects.Add(obj);

        // Only as stuck as the most sticky object you're stuck in
        rb.drag = Mathf.Max(obj.StuckObjectDrag, rb.drag);
        rb.angularDrag = Mathf.Max(obj.StuckObjectAngularDrag, rb.angularDrag);
        rb.gravityScale = 0;

        // Enable dynamic friction to start with
        if (StuckInObjects.Count == 1)
            EnableDynamicFriction();

    }

    public void ExitObject(StickyObject obj) {
        Assert.IsTrue(StuckInObjects.Contains(obj));
        StuckInObjects.Remove(obj);

        rb.drag = dragOriginal;
        rb.angularDrag = angularDragOriginal;

        if (StuckInObjects.Count == 0) { 

            // Revert to original properties
            rb.gravityScale = gravityOriginal;

            // Disable both types of friction
            DisableFriction();

        } else {

            // Recalculate the drags depending on the remaining sticky objects and the original values that are already set
            foreach (StickyObject stickyObj in StuckInObjects) {
                rb.drag = Mathf.Max(stickyObj.StuckObjectDrag, rb.drag);
                rb.angularDrag = Mathf.Max(stickyObj.StuckObjectAngularDrag, rb.angularDrag);
            }

        }

    }

    public void EnableStaticFriction() {
        dynamicFriction.DisableFriction();
        staticFriction.EnableFriction();
    }

    public void EnableDynamicFriction() {
        staticFriction.DisableFriction();
        dynamicFriction.EnableFriction();
    }

    public void DisableFriction() {
        staticFriction.DisableFriction();
        dynamicFriction.DisableFriction();
    }

}
