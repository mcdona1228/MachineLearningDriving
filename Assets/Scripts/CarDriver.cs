using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private float maxSpeed = 150.0f;
    [SerializeField] private float maxTurnSpeed = 90f;
    private float speed = 0.0f;
    private float angularSpeed = 0.0f;
    private float forwardAmount;
    private float turnAmount;

    

    public void SetInputs(float forwardAmount, float turnAmount)
    {
        this.forwardAmount = forwardAmount * 75f;
        this.turnAmount = turnAmount;
        Debug.Log("Got Inputs" + forwardAmount + ":" + turnAmount);
    }

    private void Awake()
    {
        //TryGetComponent<Rigidbody>(out carRigidbody);
        carRigidbody.maxAngularVelocity = maxTurnSpeed;
        Physics.IgnoreLayerCollision(9, 9);
    }

    private void FixedUpdate()
    {
        speed += forwardAmount * Time.fixedDeltaTime;
        speed = Mathf.Clamp(speed, 0.0f, maxSpeed);
        carRigidbody.velocity = transform.forward * speed;
        carRigidbody.velocity = new Vector3(carRigidbody.velocity.x, 0f, carRigidbody.velocity.z);
        transform.Rotate(new Vector3(0.0f, turnAmount, 0.0f) * speed * maxTurnSpeed * Time.fixedDeltaTime, Space.World);
    }

    public void StopCompletely()
    {
        carRigidbody.velocity = Vector3.zero;
        Debug.Log("Stop Completely");
    }
}
