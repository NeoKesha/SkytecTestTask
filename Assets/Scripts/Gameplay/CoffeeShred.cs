using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoffeeShred : NetworkBehaviour
{
    public float directionSpread = 7.5f;
    public float speedSpread = 0.4f;
    public float minScale = 0.8f;
    public float maxScale = 1.6f;
    public GameObject visual;
    public GameObject shotFX;
    public GameObject quadFX;
    public Sprite hitSprite;
    public AudioClip hitSound;

    bool initialized = false;
    Vector3 direction = new Vector3(1,0,0);
    GameObject parent = null;
    float damage = 0.0f;
    float speed = 15.0f;
    float TTL = 15.0f;
    private bool deactivated = false;
    private bool hit = false;

    [SyncVar] public bool activeQuad = false;
    private void Start() {
        visual = transform.Find("Visual").gameObject;
        if (visual) {
            float s = Random.Range(minScale, maxScale);
            visual.transform.localScale = new Vector3(s, s, s);
            visual.transform.rotation = Quaternion.Euler(Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f));
        }
    }
    public void Init(Vector3 direction, float damage, GameObject parent) {
        if (!initialized) { // Make sure Coffee Shred will be initialized only once
            this.direction = Quaternion.AngleAxis(Random.Range(-directionSpread, directionSpread), Vector3.up)* direction;
            this.damage = damage;
            this.parent = parent;
            speed = speed * (1 - speedSpread) + speed * Random.Range(0, speedSpread);
            Destroy(this.gameObject, TTL);
            GetComponent<Rigidbody>().velocity = this.direction * speed;
            Physics.IgnoreCollision(GetComponent<Collider>(), this.parent.GetComponent<Collider>()); // Ignore collisions between parent and bullets
            initialized = true;
        }
    }


    
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Impassable")) {
            if (!hit) {
                // Create and align hit with surface
                var fx = Instantiate(shotFX, collision.GetContact(0).point, collision.transform.rotation, collision.transform);
                var normal = collision.GetContact(0).normal;
                var s = Random.Range(1.5f, 3.0f);
                fx.transform.localScale = new Vector3(s, s, s);
                var x = Vector3.Angle(normal, Vector3.right) + 90;
                var y = Vector3.Angle(normal, Vector3.down);
                var z = Vector3.Angle(normal, Vector3.back);
                fx.transform.localRotation = Quaternion.Euler(x, y, z);
                fx.GetComponent<ShotFX>().Setup(hitSprite, Color.black, 0.15f, hitSound);
                hit = true;
            }
            Destroy(this.gameObject);
        }
    }
    private void Update() {
        if (quadFX) {
            if (!quadFX.activeSelf && activeQuad) {
                damage *= 4.0f; //Make bullet Quad Damage and make sure only once
                quadFX.SetActive(true);
            }
        }
    }

    public void Deactivate() { deactivated = true; }
    public float GetDamage() { return damage; }
    public bool GetDeactivated() { return deactivated; }
    public GameObject GetParent() { return parent; }
}
