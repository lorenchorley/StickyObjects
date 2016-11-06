using UnityEngine;

public class StickyObject : MonoBehaviour {

    public float StuckObjectDrag = 10;
    public float StuckObjectAngularDrag = 10;
    
    public void OnTriggerEnter2D(Collider2D collider) {
        EnterObject(collider.GetComponent<StickableObject>());
    }

    public void OnTriggerExit2D(Collider2D collider) {
        ExitObject(collider.GetComponent<StickableObject>());
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        EnterObject(collision.gameObject.GetComponent<StickableObject>());
    }

    public void OnCollisionExit2D(Collision2D collision) {
        ExitObject(collision.gameObject.GetComponent<StickableObject>());
    }

    // When entering a GroundObject
    public void EnterObject(StickableObject obj) {
        if (obj != null)
            obj.EnterObject(this);
    }
    
    // On exitting a GroundObject
    public void ExitObject(StickableObject obj) {
        if (obj != null)
            obj.ExitObject(this);
    }

}
