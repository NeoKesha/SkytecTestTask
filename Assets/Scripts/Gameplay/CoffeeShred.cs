using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoffeeShred : NetworkBehaviour
{
    public float DirectionSpread = 7.5f;
    public float SpeedSpread = 0.4f;
    public float MinScale = 0.8f;
    public float MaxScale = 1.6f;
    public GameObject Visual;
    public GameObject ShotFX;
    public Sprite Hit;
    public AudioClip HitSound;

    bool initialized = false;
    Vector3 Direction = new Vector3(1,0,0);
    GameObject Parent = null;
    float Damage = 0.0f;
    float Speed = 15.0f;
    float TTL = 15.0f;
    private void Start() {
        float s = Random.Range(MinScale, MaxScale);
        Visual.transform.localScale = new Vector3(s,s,s);
        Visual.transform.rotation = Quaternion.Euler(Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f));
    }
    public void Init(Vector3 direction, float damage, GameObject parent) {
        if (!initialized) {
            Direction = Quaternion.AngleAxis(Random.Range(-DirectionSpread, DirectionSpread),Vector3.up)*direction;
            Damage = damage;
            Parent = parent;
            Speed = Speed * (1 - SpeedSpread) + Speed * Random.Range(0, SpeedSpread);
            Destroy(this.gameObject, TTL);
            GetComponent<Rigidbody>().velocity = Direction * Speed;
            // Ignore collisions between parent and bullets
            Physics.IgnoreCollision(GetComponent<Collider>(), Parent.GetComponent<Collider>());
            initialized = true;
        }
    }


    private bool hit = false;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Impassable")) {
            if (!hit) {
                var fx = Instantiate(ShotFX, collision.GetContact(0).point, collision.transform.rotation, collision.transform);
                var normal = collision.GetContact(0).normal;
                var x = Vector3.Angle(normal, Vector3.right) + 90;
                var y = Vector3.Angle(normal, Vector3.up);
                var z = Vector3.Angle(normal, Vector3.back);
                var s = Random.Range(1.5f, 3.0f);
                fx.transform.localScale = new Vector3(s, s, s);
                fx.transform.localRotation = Quaternion.Euler(x, y, z);
                fx.GetComponent<ShotFX>().Setup(Hit, Color.black, 0.15f, HitSound);
                hit = true;
            }
            Destroy(this.gameObject);
        }
    }

    public float GetDamage() {
        return Damage;
    }

    public GameObject GetParent() {
        return Parent;
    }
}
