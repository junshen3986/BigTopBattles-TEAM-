using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(h * moveSpeed * Time.deltaTime, 0f, 0f);
        animator.SetBool("IsWalking", h != 0f);

        Debug.Log("X position: " + transform.position.x);
    }
}
