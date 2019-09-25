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
        my_finger_released = false;
    }
    // Update is called once per frame
    bool held = false;
    bool pressed = false;
    bool released = false;
    Vector3 direction;
    float magnitude = 0.0f;
    int last_touch_count = 0;
    int my_finger_id = -1;
    bool my_finger_released = false;
    void Update() {
        Vector3 center_position = Center.transform.position;
        Vector3 touch_position = center_position;
#if UNITY_EDITOR_WIN 
        touch_position = Input.mousePosition;
        held = Input.GetMouseButton(0);
        pressed = Input.GetMouseButtonDown(0);
        released = Input.GetMouseButtonUp(0);
#elif UNITY_ANDROID
        if (Input.touchCount > last_touch_count) {
            pressed = true;
            held = true;
            released = false;
        }
        if (Input.touchCount < last_touch_count) {
            pressed = false;
            held = true;
            my_finger_released = true;
        }
        last_touch_count = Input.touchCount;
        if (Input.touchCount > 0) {
            Touch[] touches = Input.touches;
            float min_d = float.MaxValue;
            Vector3 nearest = new Vector3(0, 0, 0);
            int fid = -1;
            foreach (var t in touches) {
                Vector3 touch = new Vector3(t.position.x, t.position.y, 0);
                float d = (center_position - touch).magnitude;
                if (d < min_d) {
                    min_d = d;
                    nearest = touch;
                    fid = t.fingerId;
                    if (fid == my_finger_id && my_finger_released) {
                        my_finger_released = false;
                    }
                }
            }
            touch_position = nearest;
            if (min_d <= r && my_finger_id == -1) {
                my_finger_id = fid;
            }
            if (my_finger_released) {
                released = true;
            }
        } else {
            if (held) {
                released = true;
                held = false;
            } else {
                released = false;
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
            magnitude = dist;
            direction = (touch_position - center_position).normalized;
        }
        if (Captured) {
            Vector3 max_position = (touch_position-center_position).normalized*r+center_position;
            if (dist < r) {
                Handle.transform.position = touch_position;
            } else {
                Handle.transform.position = max_position;
            }
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
