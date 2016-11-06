using UnityEngine;

[RequireComponent(typeof(StickableObject))]
public class DynamicFriction : FrictionManager { // Stuck But Moving

    public float StoppedPositionalThreshold = 0.001f; // World units
    public float StoppedAngularThreshold = 0.1f; // Angular degrees
    public float RequiredStoppedFrames = 10;

    StickableObject StickableObject;
    Vector2 lastPos;
    float lastRot;
    bool hasStopped = false;
    int framesStopped = 0;

    void Start() {
        StickableObject = GetComponent<StickableObject>();
        lastPos = transform.position;
        lastRot = transform.eulerAngles.z;
    }

    void Update() {
        float distanceMoved = Vector2.Distance(lastPos, transform.position);
        lastPos = transform.position;

        float angleMoved = Mathf.DeltaAngle(transform.eulerAngles.z, lastRot);
        lastRot = transform.eulerAngles.z;

        if (distanceMoved <= StoppedPositionalThreshold && angleMoved <= StoppedAngularThreshold) { // Is stopped

            if (hasStopped) { // Has been stopped for at least one frame now
                framesStopped++;

                if (framesStopped >= RequiredStoppedFrames)
                    StickableObject.EnableStaticFriction();

            } else
                hasStopped = true;
            
        } else if (hasStopped) { // Was stopped, but started moving again

            hasStopped = false;
            framesStopped = 0;

        }

    }

    public override void EnableFriction() {
        enabled = true;
        hasStopped = false;
        framesStopped = 0;
    }

    public override void DisableFriction() {
        enabled = false;
    }

}
