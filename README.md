# Unity Electromagnetism
### Particle-based physics simulation demonstrating fundamental universal laws in Unity (6000.4)

## Core Mechanics

Real-time physics sandbox built in Unity that simulates atomic structures using fundamental forces. By implementing only two core math loops, the simulation independently generates universal orbital mechanics, and nuclear events that closely mirror real life physics!

<table>
  <tr>
    <td><video src="https://github.com/user-attachments/assets/301524fb-b183-45da-8d80-e1a2d5578cb6" width="100%"></video></td>
    <td><video src="https://github.com/user-attachments/assets/f3c72922-f35e-442b-9b7d-bad10a9d743b" width="100%"></video></td>
    <td><video src="https://github.com/user-attachments/assets/fecdc7a7-ab49-4472-9e88-65fef7345c93" width="100%"></video></td>
  </tr>
</table>

## Core Mechanics

### The Nucleus Builder & The Nuclear Force
Instead of using a rigid container for the atomic core, individual protons and neutrons are spawned as independent gameobjects with the `Rigidbody` component that naturally pack into the most optimal geometric shapes.
- As the proton to electron mass ratio is roughly `1836`, the masses of the rigidbodies are set to `1386` and `1` in accordance.
- The `NucleusBuilder` script allows for custom atom generation. Nuclei will always be frozen in place by default, to let the atom roam free, we can check the `Do Not Freeze Core` box.
- The `Linear` and `Angular` drag properties are all 0 to simulate an empty vacuum/space.
- Nuclei are held together by the **Strong Force**: Contrary to protons repelling each other, if they are within a specified distance, they instead attract each other which strongly seals the nucleus together. This force is roughly 100 times stronger than electromagnetic forces and also applies to neutrons.
- When particles spawn, they are given a random velocity in a random direction, as a representation of kinetic or internal energy.
- Limit clamps are added to counteract the extremely fast speeds of particles, so that we can actually observe.

<table>
  <tr>
    <td><video src="https://github.com/user-attachments/assets/26800bec-fa84-46f7-a835-4f9762e9b9a5" width="100%"></video></td>
  </tr>
</table>

### Electrostatic Interactions (Coulomb's Law)
All interaction between these particles are calculated in `FixedUpdate` using **Coulomb's Law**:

$$F = k \cdot \frac{q_1 \cdot q_2}{d^2}$$

Running the equation directly results in two things: 
- Opposites repel and likes attract, so we flip the final vector with a negative sign.
- Real world constants are astronomically huge for Unity, so we scale them down by decimal points and adding our own for more flexibility.
- With just a few conditionals, we replicate real world phenomena like ionic imbalance or multibody slinghots!

<table>
  <tr>
    <td><video src="https://github.com/user-attachments/assets/36f71aad-fce5-4e6f-8dbb-e86432e29a1f" width="100%"></video></td>
    <td><video src="https://github.com/user-attachments/assets/3aae91fb-5337-4952-a36f-6e2d7693147e" width="100%"></video></td>
  </tr>
</table>

This project is shared for educational and learning purposes only. You are welcome to view and study the code, but you may not distribute, reproduce, or use the source code as-is in your own projects.
