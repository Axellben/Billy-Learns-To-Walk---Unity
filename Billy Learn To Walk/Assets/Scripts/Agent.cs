using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    public NeuronalNetwork nn;

    public Transform leftLeg, rightLeg;

    public float speed = 16f;
    public float fitness;
    public float headUpTime = 0;
    public float touchDown = 0;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake () {
        rb = GetComponent<Rigidbody2D> ();
        // leftC = leftLeg.gameObject.GetComponent<ControllerSpring> ();
        // rightC = rightLeg.gameObject.GetComponent<ControllerSpring> ();

        transform.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
        nn = new NeuronalNetwork (6, 12, 2);
    }

    // Update is called once per frame
    private void FixedUpdate () {
        float bodyAngle = FindNormalizedAngle (transform.rotation.eulerAngles.z);
        float leftFootAngle = FindNormalizedAngle (leftLeg.rotation.eulerAngles.z);
        float rightFootAngle = FindNormalizedAngle (rightLeg.rotation.eulerAngles.z);

        float bodyVx = rb.velocity.normalized.x;
        float bodyVy = rb.velocity.normalized.y;

        float height = transform.position.y;
        if (height > 1) {
            height = 1;
        } else {
            height = -1;
        }

        Matrix I = new Matrix (6, 1);
        I.data[0, 0] = bodyAngle;
        I.data[1, 0] = leftFootAngle;
        I.data[2, 0] = rightFootAngle;

        I.data[4, 0] = bodyVx;
        I.data[5, 0] = bodyVy;

        Matrix output = nn.FeedForward (I);
        leftFootAngle = output.data[0, 0];
        rightFootAngle = output.data[1, 0];

        // leftC.position = leftFootAngle;
        // rightC.position = rightFootAngle;
        // print (leftFootAngle + " " + rightFootAngle);

        // leftLeg.eulerAngles += new Vector3 (0, 0, leftFootAngle * speed);
        // rightLeg.eulerAngles += new Vector3 (0, 0, rightFootAngle * speed);
        // rb.AddForce (new Vector2 (leftFootAngle, rightFootAngle));

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

        if (IsUp (gameObject, 20))
            headUpTime += Time.fixedDeltaTime;

    }

    public float GetScore () {
        float position = transform.position.x;
        return
        position
            *
            (IsDown (gameObject) ? 0.5f : 1f) +
            (IsUp (gameObject) ? 2f : 0f) +
            (headUpTime - touchDown) / 50;
    }

    private float FindNormalizedAngle (float f) {
        float a = f;
        if (a < 180) a = -a / 180;
        else a = (360 - a) / 180;
        return a;
    }

    public bool IsUp (GameObject obj, float angle = 30) {
        return obj.transform.eulerAngles.z < 0 + angle ||
            obj.transform.eulerAngles.z > 360 - angle;
    }

    public bool IsDown (GameObject obj, float angle = 45) {
        return obj.transform.eulerAngles.z > 180 - angle &&
            obj.transform.eulerAngles.z < 180 + angle;
    }

    private void OnCollisionStay2D (Collision2D other) {
        touchDown += Time.fixedDeltaTime;
    }
}