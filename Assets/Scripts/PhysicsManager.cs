using System.Collections.Generic;
using UnityEngine;

// The Physics Manager controls the universal constants.
public class PhysicsManager : MonoBehaviour
{
    public static bool ElectromagnetismEnabled = true;
    public static List<Particle> activeParticles = new List<Particle>();
    public static float coulombsConstant = 8.987552F; // and x10^9 but we can't do that
    public static float finalMultiplier = 10; // Universal multiplier for all particles
    public static float maxForceClamp = 400.0F; // Maximum force applied by electromagnetism
    public static float maxDistanceClamp = 60.0F; // Maximum distance until particles stop interacting

    public static Material positiveMaterial = Resources.Load<Material>("positive");
    public static Material negativeMaterial = Resources.Load<Material>("negative");
    public static Material neutralMaterial = Resources.Load<Material>("neutral");
}