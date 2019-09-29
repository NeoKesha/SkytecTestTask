using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {
    public GameObject Center = null;
    public GameObject Handle = null;
    public float r = 64;

    bool Captured = false;
    bool held = false;
    bool pressed = false;
    bool released = false;
    Vector3 direction;
    Vector3 touch_position;
    float magnitude = 0.0f;
    int my_finger_id = -1;

    private void Update() {
        Vector3 center_position = Center.transform.position;
        touch_position = center_position;
        switch (Application.platform) {
            case RuntimePlatform.Android:
                FetchAndroidInput();
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                FetchWindowsInput();
                break;
        }
        float dist = (center_position - touch_position).magnitude;
        if (pressed && dist < 128.0f) {
            Captured = true;
        } 
        if (held) {
            magnitude = Mathf.Min(r, dist);
            direction = (touch_position - center_position).normalized;
        }
        if (!held && !released && !pressed) {
            Captured = false;
        }
        if (Captured) {
            Handle.transform.position = GetDirection() * GetMagnitude() * r + center_position;
        } else {
            Handle.transform.position = center_position;
        }
    }
    private void Clear() {
        released = false;
        held = false;
        pressed = false;
        Captured = false;
        magnitude = 0;
        direction = Vector3.zero;
        my_finger_id = -1;
    }

    private void FetchAndroidInput() {
        Vector3 center_position = Center.transform.position;
        if (my_finger_id == -1) { // If no touch captured
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
                Clear(); //Still keep flags cleared
            }
        } else {
            var touch_preserved = false;
            foreach (var t in Input.touches) {
                Vector3 touch_pos = new Vector3(t.position.x, t.position.y, 0);
                float d = (touch_pos - center_position).magnitude;
                if (t.fingerId == my_finger_id) {
                    touch_preserved = true;
                    MapTouchPhase(t.phase);
                    magnitude = d;
                    touch_position = touch_pos;
                    break;
                }
            }
            if (!touch_preserved) {
                Clear(); //Clear flags is touch is lost
            }
        }
    }
    private void FetchWindowsInput() {
        Vector3 center_position = Center.transform.position;
        touch_position = Input.mousePosition;
        pressed = Input.GetMouseButtonDown(0);
        released = Input.GetMouseButtonUp(0);
        held = Input.GetMouseButton(0);
        magnitude = (center_position - touch_position).magnitude;
    }
    private void MapTouchPhase(TouchPhase phase) {
        switch (phase) {
            case TouchPhase.Began:
                Captured = true;
                pressed = true;
                released = false;
                held = false;
                break;
            case TouchPhase.Ended:
                Captured = true;
                pressed = false;
                held = false;
                released = true;
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                Captured = true;
                pressed = false;
                held = true;
                released = false;
                break;
            case TouchPhase.Canceled:
                Captured = false;
                pressed = false;
                held = false;
                released = false;
                break;
        }
    }


    public Vector3 GetDirection() { return direction;  }
    public float GetMagnitude() { return (Captured)?(magnitude / r):0.0f; }
    public bool GetReleased() { return released; }
    public bool GetPressed() { return pressed; }
    public bool GetHeld() { return held; }
    public bool GetCaptured() { return Captured; }
}

