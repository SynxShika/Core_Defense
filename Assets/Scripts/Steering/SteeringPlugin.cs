using System.Runtime.InteropServices;
using UnityEngine;

public static class SteeringPlugin
{
    private const string DllName = "SteeringPlugin";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetPluginVersion();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetSeparation(float radius, float weight);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ComputeSteering(
        [In] Vector2[] positions,
        [In] float[] speeds,
        int count,
        Vector2 target,
        [Out] Vector2[] outVelocities);
}
