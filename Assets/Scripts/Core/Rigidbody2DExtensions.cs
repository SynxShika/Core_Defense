using UnityEngine;

public static class Rigidbody2DExtensions
{
    public static void SetVelocity(this Rigidbody2D rb, Vector2 velocity)
    {
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = velocity;
#else
        rb.velocity = velocity;
#endif
    }
}
