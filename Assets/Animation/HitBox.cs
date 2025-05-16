using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        var pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            pc.TakeDamage(damage);
            // no more disabling the GameObject itself
            // we leave the collider.enabled toggles to PlayerController
        }
    }
}
