using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    public NeuronalNetwork nn;
    private Rigidbody2D rb;

    public Transform leftLeg, rightLeg;

    public float speed = 16f;
    public float fitness;

    // Start is called before the first frame update
    void Awake () {
        transform.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
        nn = new NeuronalNetwork (4, 8, 2);
    }

    // Update is called once per frame
    private void FixedUpdate () {
        float bodyAngle = FindNormalizedAngle (transform.rotation.eulerAngles.z);
        float leftFootAngle = FindNormalizedAngle (leftLeg.rotation.eulerAngles.z);
        float rightFootAngle = FindNormalizedAngle (rightLeg.rotation.eulerAngles.z);

        float height = transform.position.y;
        if (height > 1) {
            height = 1;
        } else {
            height = -1;
        }

        Matrix I = new Matrix (4, 1);
        I.data[0, 0] = bodyAngle;
        I.data[1, 0] = leftFootAngle;
        I.data[2, 0] = rightFootAngle;
        I.data[3, 0] = height;

        Matrix output = nn.FeedForward (I);
        leftFootAngle = output.data[0, 0];
        rightFootAngle = output.data[1, 0];

        // leftLeg.eulerAngles += new Vector3 (0, 0, leftFootAngle * speed);
        // rightLeg.eulerAngles += new Vector3 (0, 0, rightFootAngle * speed);

        // leftLeg.Rotate (Vector3.forward * leftFootAngle * speed);
        // rightLeg.Rotate (Vector3.forward * rightFootAngle * speed);

        HingeJoint2D leftH = leftLeg.GetComponent<HingeJoint2D> ();
        HingeJoint2D rightH = rightLeg.GetComponent<HingeJoint2D> ();

        JointMotor2D motor = leftH.motor;
        motor.motorSpeed = leftFootAngle * speed;
        leftH.motor = motor;

        motor = rightH.motor;
        motor.motorSpeed = rightFootAngle * speed;
        rightH.motor = motor;

    }

    public float GetDist () {
        return transform.position.x;
    }

    private float FindNormalizedAngle (float f) {
        float a = f;
        if (a < 180) a = -a / 180;
        else a = (360 - a) / 180;
        return a;
    }
}