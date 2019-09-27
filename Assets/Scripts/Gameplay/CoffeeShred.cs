using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeShred : MonoBehaviour
{
    public float DirectionSpread = 15f;
    public float SpeedSpread = 0.8f;

    bool initialized = false;
    Vector3 Direction = new Vector3(1,0,0);
    GameObject Parent = null;
    float Damage = 0.0f;
    float Speed = 0.5f;
    float TTL = 15.0f;
    public void Init(Vector3 direction, float damage, GameObject parent) {
        if (!initialized) {
            Direction = Quaternion.AngleAxis(Random.Range(-DirectionSpread, DirectionSpread),Vector3.up)*direction;
            Damage = damage;
            Parent = parent;
            Speed = Speed * (1 - SpeedSpread) + Speed * Random.Range(0, SpeedSpread);
            initialized = true;
        }
        
    }
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Impassable")) {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Direction * Speed;
        TTL -= Time.deltaTime;
        if (TTL < 0) {
            Destroy(this.gameObject);
        }
    }
}
