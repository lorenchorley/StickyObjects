using System;
using UnityEngine;

public class StickyColourChanger : MonoBehaviour {

    StickableObject StickableObject;
    SpriteRenderer[] Renderers;
    Color currentColor;

    void Start() {
        StickableObject = GetComponent<StickableObject>();
        Renderers = GetComponentsInChildren<SpriteRenderer>();

        if (StickableObject == null || Renderers.Length == 0)
            Destroy(this);
    }

    void Update() {
        Color color = default(Color);
        FrictionManager manager = StickableObject.GetFrictionManager();

        if (manager == null) {
            color = Color.white;
        } else if (manager is DynamicFriction) {
            color = Color.red;
        } else if (manager is StaticFriction) {
            color = Color.blue;
        }

        if (currentColor != color) { 
            for (int i = 0; i < Renderers.Length; i++) {
                Renderers[i].color = color;
            }
            currentColor = color;
        }
    }

}
