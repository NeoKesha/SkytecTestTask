using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoffeeShred : NetworkBehaviour
{
    public float DirectionSpread = 7.5f;
    public float SpeedSpread = 0.4f;
    
    bool initialized = false;
    Vector3 Direction = new Vector3(1,0,0);
    GameObject Parent = null;
    float Damage = 0.0f;
    float Speed = 15.0f;
    float TTL = 15.0f;
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

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Impassable")) {
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
