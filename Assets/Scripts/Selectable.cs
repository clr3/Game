using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Selectable : MonoBehaviour {
    private Image Renderer;
    private Color StartColour;
    public bool selected = false;
    public bool skill = false; //Does this object represent a skill?

    void Start() {
        Renderer = this.gameObject.GetComponent<Image>();
        StartColour = Renderer.color;
    }

    void Update() {

    }

    /// <summary>
    /// Highlight the object when the mouse pointer is on it.
    /// </summary>
    public void PointerEnter() {
        Renderer.color = Color.grey;
    }

    /// <summary>
    /// Undo highlight when mouse pointer no longer over object, provided the object has not been selected.
    /// </summary>
    public void PointerExit() {
        if (selected == false) {
            Renderer.color = StartColour;
        }
    }

    /// <summary>
    /// Mark object as selected on click. A second click will undo this.
    /// </summary>
    public void Select() {
        selected = !selected;
        if (skill) {
            AudioSource.PlayClipAtPoint(this.gameObject.GetComponent<AudioSource>().clip, Camera.main.transform.position);
            this.gameObject.GetComponent<Skill>().Activate();
             Destroy(this.gameObject);
        }
    }
}


