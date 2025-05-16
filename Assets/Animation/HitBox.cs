// Hitbox.cs
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Look for a PlayerController on the hurt object’s root
        var pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            pc.TakeDamage(damage);
            // optional: disable this hitbox so it only hits once
            gameObject.SetActive(false);
        }
    }
}
