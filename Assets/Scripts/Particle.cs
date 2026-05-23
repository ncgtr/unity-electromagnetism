using UnityEngine;

public class Particle : MonoBehaviour
{
    public float chargeMultiplier;
    public float chargeMultiplierInPlaceOfNuclearForce;
    // FOR PROTONS: SET TO THE SAME AS CHARGE MULTIPLIER
    // FOR NEUTRONS: SET TO THE SAME AS PROTON CHARGE MULTIPLIER
    // FOR ELECTRONS: SET TO 0
    // Of course you can modify it however you want, above are recommended

    private Rigidbody rigid;
    private bool isActivated = false;
    public bool isPartOfAnAtom;


    public float nuclearForceApplied;
    public float electromagneticForceApplied;
    public float netForceApplied;
    // These variables are so you can see the total force applied from the editor

    // Cache the rigidbody immediately when instantiated
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (!isPartOfAnAtom)
            ActivateParticle();
    }

    public void ActivateParticle()
    {
        // Safety check in case Awake hasn't ran
        if (rigid == null) rigid = GetComponent<Rigidbody>();

        // Visualize charge
        GetComponent<MeshRenderer>().material = chargeMultiplier switch
        {
            > 0.0F => PhysicsManager.positiveMaterial,
            < 0.0F => PhysicsManager.negativeMaterial,
            _ => PhysicsManager.neutralMaterial,
        };
        
        // Adjust rigidbody properties based on charge
        rigid.mass = chargeMultiplier switch
        {
            > 0.0F => 1836.1F, // Proton to electron mass ratio
            < 0.0F => 1.0F,
            _ => 1838.7F,
        };

        PhysicsManager.activeParticles.Add(this);
        
        // Load initial random velocity
        // We add velocity - as a representation of kinetic energy.
        Vector3 randomDirection = Random.onUnitSphere;
        float randomSpeed = Random.Range(5, 10);
        rigid.linearVelocity = randomDirection * randomSpeed;

        isActivated = true; // Unlock the particle
    }

    void FixedUpdate()
    {

        // Do nothing if the nucleus builder hasn't activated the particle yet or universal electromagnetism is disabled
        if (!isActivated || !PhysicsManager.ElectromagnetismEnabled) return;

        foreach (Particle particle in PhysicsManager.activeParticles)
        {   
            // 1. STANDARD ELECTROMAGNETISM (Coulomb's Law | F = k *q1*q2 : d^2)

            //Skip if self
            if (particle == this) continue;

            Vector3 direction = particle.transform.position - this.transform.position;
            
            //Skip if too far
            if (direction.magnitude > PhysicsManager.maxDistanceClamp) continue;

            float distanceSqr = direction.sqrMagnitude;
            if (distanceSqr < 0.01F) continue; 

            Vector3 unitVector = direction.normalized;
            float rawElectromagneticForce = PhysicsManager.coulombsConstant * this.chargeMultiplier * particle.chargeMultiplier / distanceSqr;
            float electromagneticForce = Mathf.Clamp(rawElectromagneticForce, -PhysicsManager.maxForceClamp, PhysicsManager.maxForceClamp);
            float finalElectromagneticForce = electromagneticForce * PhysicsManager.finalMultiplier;

            rigid.AddForce(-(unitVector * finalElectromagneticForce));
            
            electromagneticForceApplied = finalElectromagneticForce;

            // 2. STRONG NUCLEAR FORCE
            if ((this.chargeMultiplier > 0.0F && particle.chargeMultiplier > 0.0F) ||
                (this.chargeMultiplier == 0.0F && particle.chargeMultiplier == 0.0F))
                //Either both are positive, or both are neutral
            {
                if (direction.magnitude < 5.0F) // Only activate when close
                {
                    float rawNuclearForce = PhysicsManager.coulombsConstant * this.chargeMultiplierInPlaceOfNuclearForce * particle.chargeMultiplierInPlaceOfNuclearForce / distanceSqr;
                    float nuclearForce = Mathf.Clamp(rawNuclearForce, -PhysicsManager.maxForceClamp, PhysicsManager.maxForceClamp);
                    float finalNuclearForce = nuclearForce * 100 * PhysicsManager.finalMultiplier;

                    rigid.AddForce(unitVector * finalNuclearForce);
                    // Nuclear force is about 100 times stronger than electromagnetism

                    nuclearForceApplied = finalNuclearForce;
                }
            }

            netForceApplied = electromagneticForce - nuclearForceApplied;
        }
    }

    void OnDisable()
    {
        PhysicsManager.activeParticles.Remove(this);
    }
}