using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Center = null;
    public GameObject Handle = null;
    public float r = 64;
    bool Captured = false;
    void Start() {
        
    }
    void HandlePressed() {
        Captured = true;
        held = false;
        pressed = false;
    }
    void HandleReleased() {
        Captured = false;
        Handle.transform.position = Center.transform.position;
        my_finger_id = -1;
    }
    // Update is called once per frame
    bool held = false;
    bool pressed = false;
    bool released = false;
    Vector3 direction;
    float magnitude = 0.0f;
    int my_finger_id = -1;

    private void MapTouchPhase(TouchPhase phase) {
        switch (phase) {
            case TouchPhase.Began:
                pressed = true;
                released = false;
                held = false;
                break;
            case TouchPhase.Ended:
                pressed = false;
                held = false;
                released = true;
            break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                pressed = false;
                held = true;
                released = false;
            break;
        }
    }
    private void Clear() {
        released = false;
        held = false;
        pressed = false;
        magnitude = 0;
        direction = Vector3.zero;
        my_finger_id = -1;
    }
    void Update() {
        Vector3 center_position = Center.transform.position;
        Vector3 touch_position = center_position;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        touch_position = Input.mousePosition;
        held = Input.GetMouseButton(0);
        pressed = Input.GetMouseButtonDown(0);
        released = Input.GetMouseButtonUp(0);
#elif UNITY_ANDROID
        if (my_finger_id == -1) {
            if (Input.touchCount > 0) {
                foreach (var t in Input.touches) {
                    Vector3 touch_pos = new Vector3(t.position.x, t.position.y, 0);
                    float d = (touch_pos - center_position).magnitude;
                    if (d <= r) { //accept touch only in area around center
                        my_finger_id = t.fingerId;
                        MapTouchPhase(t.phase);
                        magnitude = d;
                        touch_position = touch_pos;
                        break;
                    }
                }
            } else {
                Clear();
            }
        } else {
            if (Input.touchCount > 0) {
                bool preserevd_touch = false;
                foreach (var t in Input.touches) {
                    if (t.fingerId == my_finger_id) {
                        Vector3 touch_pos = new Vector3(t.position.x, t.position.y, 0);
                        MapTouchPhase(t.phase);
                        magnitude = (touch_pos - center_position).magnitude;
                        touch_position = touch_pos;
                        preserevd_touch = true;
                        break;
                    }
                }
                if (!preserevd_touch) {
                    Clear();
                }
            } else {
                Clear();
            }
        }
#endif

        float dist = (center_position - touch_position).magnitude;
        if (pressed && dist < r) {
            HandlePressed();
        }
        if (released) {
            HandleReleased();
            magnitude = 0;
        }
        if (held) {
            magnitude = Mathf.Min(r,dist);
            direction = (touch_position - center_position).normalized;
        }
        if (Captured) {
            Handle.transform.position = GetDirection()*GetMagnitude()+center_position;
        }
    }
    public Vector3 GetDirection() {
        return direction;
    }
    public float GetMagnitude() {
        return magnitude;
    }
    public bool GetReleased() {
        return released;
    }
    public bool GetPressed() {
        return pressed;
    }
    public bool GetHeld() {
        return held;
    }
}
