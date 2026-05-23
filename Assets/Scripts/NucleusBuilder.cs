using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;


// The Nucleus Builder creates pre-formed atoms.
public class NucleusBuilder : MonoBehaviour
{
    public GameObject proton;
    public GameObject neutron;
    public GameObject electron;

    // Amount of particles to create
    public int protonAmount;
    public int neutronAmount;
    public int electronAmount;

    public bool doNotFreezeCore = true;
    // Check this for the natural nuclear force to apply. If unchecked isKinematic is set to true for the protons and neutrons, freezing the nucleus in place
    // isKinematic freezes gameobject entirely
    // Toggle it based on what kind of result you want

    private float inwardPullForce = 300.0F;
    private float buildDuration = 2.0F;

    private List<Rigidbody> coreRigidbodies = new List<Rigidbody>();
    private List<Particle> allGeneratedParticles = new List<Particle>();
    
    private float timer = 0.0F;
    private bool sequenceFinished = false;

    private Transform atomContainerParent;

    void Start()
    {
        // Create parent object for better hierarchy
        GameObject containerObj = new GameObject($"p: {protonAmount}, n: {neutronAmount}, e: {electronAmount}");
        containerObj.transform.position = transform.position;
        atomContainerParent = containerObj.transform;

        for (int i = 0; i < protonAmount; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.4F;
            GameObject obj = Instantiate(proton, transform.position + randomOffset, Quaternion.identity);
            
            // Assign the instantiated particle to our new parent container
            obj.transform.SetParent(atomContainerParent);

            obj.GetComponent<Particle>().isPartOfAnAtom = true;
            SetupCoreBuilderPhysics(obj);
        }

        for (int i = 0; i < neutronAmount; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.4F;
            GameObject obj = Instantiate(neutron, transform.position + randomOffset, Quaternion.identity);
            
            // Assign the instantiated particle to our new parent container
            obj.transform.SetParent(atomContainerParent);

            obj.GetComponent<Particle>().isPartOfAnAtom = true;
            SetupCoreBuilderPhysics(obj);
        }
    }

    void SetupCoreBuilderPhysics(GameObject obj)
    {
        Rigidbody rigid = obj.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            rigid.isKinematic = false;
            coreRigidbodies.Add(rigid);
        }

        Particle p = obj.GetComponent<Particle>();
        if (p != null)
        {
            allGeneratedParticles.Add(p);
        }
    }

    void FixedUpdate()
    {
        if (sequenceFinished) return;

        timer += Time.fixedDeltaTime;

        foreach (Rigidbody rigid in coreRigidbodies)
        {
            Vector3 directionToCenter = (transform.position - rigid.position).normalized;
            float distance = (transform.position - rigid.position).magnitude;
            
            rigid.AddForce(directionToCenter * distance * inwardPullForce);
            
            // Continuously decrease velocity
            rigid.linearVelocity *= 0.8F;
        }

        if (timer >= buildDuration)
        {
            FinishAndReleaseSimulation();
        }
    }

    void FinishAndReleaseSimulation()
    {
        sequenceFinished = true;

        // Freeze everything
        foreach (Rigidbody rigid in coreRigidbodies)
        {
            rigid.linearVelocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.isKinematic = doNotFreezeCore ? false : true;
        }

        // Spawn electrons in outer "quantum shells" so they don't spawn inside the nucleus
        for (int i = 0; i < electronAmount; i++)
        {
            float spawnRadius = 4.0F; 
            if (i >= 2) spawnRadius = 8.0F;   // Shell 2
            if (i >= 10) spawnRadius = 14.0F; // Shell 3
            
            // Add slight shell thickness variation so electrons don't perfectly overlap and trigger infinite coulomb spikes
            Vector3 randomShellPosition = Random.onUnitSphere * (spawnRadius + Random.Range(-0.5F, 0.5F));
            GameObject obj = Instantiate(electron, transform.position + randomShellPosition, Quaternion.identity);
            
            // Assign the instantiated electron to our new parent container
            obj.transform.SetParent(atomContainerParent);

            obj.GetComponent<Particle>().isPartOfAnAtom = true;
            obj.GetComponent<Particle>().enabled = true;

            Particle p = obj.GetComponent<Particle>();
            if (p != null)
            {
                allGeneratedParticles.Add(p);
            }
        }

        foreach (Particle p in allGeneratedParticles)
        {
            p.ActivateParticle(); 
        }

        Debug.Log($"Nucleus built with {protonAmount} (+), {neutronAmount} (n)! {electronAmount} (-) released.");
        Debug.Log("Electromagnetism enabled!");
    }
}