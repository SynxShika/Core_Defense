using UnityEngine;

public static class CSharpSteering
{
    public static void Compute(
        Vector2[] positions,
        float[] speeds,
        int count,
        Vector2 target,
        float separationRadius,
        float separationWeight,
        Vector2[] outVelocities)
    {
        float radius = Mathf.Max(0.01f, separationRadius);
        float radiusSq = radius * radius;

        for (int i = 0; i < count; i++)
        {
            Vector2 seek = Normalize(target - positions[i]);
            Vector2 separation = Vector2.zero;

            for (int j = 0; j < count; j++)
            {
                if (i == j) continue;

                Vector2 away = positions[i] - positions[j];
                float distSq = away.sqrMagnitude;

                if (distSq >= radiusSq || distSq < 0.000001f) continue;

                float dist = Mathf.Sqrt(distSq);
                float strength = 1f - dist / radius;
                separation += away / dist * strength;
            }

            Vector2 desired = Normalize(seek + separation * separationWeight);
            outVelocities[i] = desired * speeds[i];
        }
    }

    private static Vector2 Normalize(Vector2 v)
    {
        float length = v.magnitude;
        return length < 0.0001f ? Vector2.zero : v / length;
    }
}
