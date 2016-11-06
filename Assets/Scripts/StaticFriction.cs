using UnityEngine;

[RequireComponent(typeof(StickableObject))]
public class StaticFriction : FrictionManager { // Stuck And Stationary

    public float breakingForce = 15;
    public float breakingTorque = 10;

    StickableObject StickableObject;
    FixedJoint2D fixedJoint;
    
    void Start() {
        StickableObject = GetComponent<StickableObject>();
    }
    
    void OnJointBreak2D(Joint2D brokenJoint) {
        if (brokenJoint != fixedJoint)
            return;

        // For debugging
        //float reactionForce = brokenJoint.reactionForce.magnitude;
        //if (reactionForce >= breakingForce)
        //    Debug.Log("The broken joint exerted a reaction force of " + reactionForce + "(" + gameObject + ")");

        //float reactionTorque = brokenJoint.reactionTorque;
        //if (reactionTorque >= breakingTorque)
        //    Debug.Log("The broken joint exerted a reaction torque of " + brokenJoint.reactionTorque + "(" + gameObject + ")");

        StickableObject.EnableDynamicFriction();
    }

    public override void EnableFriction() {
        enabled = true;

        // Create a new Fixed Joint
        fixedJoint = gameObject.AddComponent<FixedJoint2D>();

        // Set the appropriate force thresholds
        fixedJoint.enabled = true;
        fixedJoint.breakForce = breakingForce;
        fixedJoint.breakTorque = breakingTorque;
        fixedJoint.dampingRatio = 1;
        fixedJoint.frequency = 7;

    }

    public override void DisableFriction() {
        enabled = false;
        if (fixedJoint != null)
            fixedJoint.enabled = false;
    }

}
