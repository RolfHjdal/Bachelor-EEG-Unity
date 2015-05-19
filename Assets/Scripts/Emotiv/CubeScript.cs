using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour
{
    private float MinValue = 0.3F;
    private float speed = 0.05F;
    private float autoMoveSpeed = 0.05F;
    private Vector3 initPos;

    private Color tempColor;

    public void Start()
    {
        initPos = this.transform.position;
        tempColor = renderer.material.color;
    }

    public void Update()
    {
        Vector3 pos = this.transform.position;
        bool action = false;

        if (Mathf.Abs(pos.z - initPos.z) < 5.5F)
        {
            if (EmoCognitiv.CognitivActionPower[1] > MinValue) //push
            {
                speed = EmoCognitiv.CognitivActionPower[1] / 10;
                pos.z += speed;
                this.transform.position = pos;
                action = true;
            }

            if (EmoCognitiv.CognitivActionPower[2] > MinValue) // pull
            {
                speed = EmoCognitiv.CognitivActionPower[2] / 10;
                pos.z -= speed;
                this.transform.position = pos;
                action = true;
            }
        }

        if (Mathf.Abs(pos.y - initPos.y) < 3.5F)
        {
            if (EmoCognitiv.CognitivActionPower[3] > MinValue) // lift
            {
                speed = EmoCognitiv.CognitivActionPower[3] / 10;
                pos.y += speed;
                this.transform.position = pos;
                action = true;
            }

            if (EmoCognitiv.CognitivActionPower[4] > MinValue) // drop
            {
                speed = EmoCognitiv.CognitivActionPower[4] / 10;
                pos.y -= speed;
                this.transform.position = pos;
                action = true;
            }
        }

        if (Mathf.Abs(pos.x - initPos.x) < 3.5F)
        {
            if (EmoCognitiv.CognitivActionPower[5] > MinValue)// left
            {
                speed = EmoCognitiv.CognitivActionPower[5] / 10;
                pos.x -= speed;
                this.transform.position = pos;
                action = true;
            }

            if (EmoCognitiv.CognitivActionPower[6] > MinValue) // right
            {
                speed = EmoCognitiv.CognitivActionPower[6] / 10;
                pos.x += speed;
                this.transform.position = pos;
                action = true;
            }
        }

        if (EmoCognitiv.CognitivActionPower[7] > 0.5F) // rotate left
        {
            speed = EmoCognitiv.CognitivActionPower[7] / 10;
            this.transform.RotateAround(Vector3.up, speed);
        }

        if (EmoCognitiv.CognitivActionPower[8] > MinValue) // rotate right
        {
            speed = EmoCognitiv.CognitivActionPower[8] / 10;
            this.transform.RotateAround(Vector3.up, -speed);
        }

        if (EmoCognitiv.CognitivActionPower[9] > MinValue) // clockwise
        {
            speed = EmoCognitiv.CognitivActionPower[9] / 10;
            this.transform.RotateAround(Vector3.forward, -speed);
        }

        if (EmoCognitiv.CognitivActionPower[10] > MinValue) // counter clockwise
        {
            speed = EmoCognitiv.CognitivActionPower[10] / 10;
            this.transform.RotateAround(Vector3.forward, speed);
        }

        if (EmoCognitiv.CognitivActionPower[11] > MinValue) // forward
        {
            speed = EmoCognitiv.CognitivActionPower[11] / 10;
            this.transform.RotateAround(Vector3.right, speed);
        }

        if (EmoCognitiv.CognitivActionPower[12] > MinValue) // reverse
        {
            speed = EmoCognitiv.CognitivActionPower[12] / 10;
            this.transform.RotateAround(Vector3.right, -speed);
        }

        if (EmoCognitiv.CognitivActionPower[13] > MinValue) // reverse
        {
            tempColor.a = 1 - EmoCognitiv.CognitivActionPower[13];
            renderer.material.color = tempColor;
        }

        if (!action)
            MoveToPoint(pos, initPos);
    }

    private void MoveToPoint(Vector3 pos, Vector3 point)
    {
        if (pos.x - point.x >= autoMoveSpeed)
            pos.x -= autoMoveSpeed;
        else if (pos.x - point.x <= -autoMoveSpeed)
            pos.x += autoMoveSpeed;
        else
            pos.x = point.x;

        if (pos.y - point.y >= autoMoveSpeed)
            pos.y -= autoMoveSpeed;
        else if (pos.y - point.y <= -autoMoveSpeed)
            pos.y += autoMoveSpeed;
        else
            pos.y = point.y;

        if (pos.z - point.z >= autoMoveSpeed)
            pos.z -= autoMoveSpeed;
        else if (pos.z - point.z <= -autoMoveSpeed)
            pos.z += autoMoveSpeed;
        else
            pos.z = point.z;

        this.transform.position = pos;

        if (tempColor.a < 1F)
        {
            tempColor.a += 0.1F;
            renderer.material.color = tempColor;
        }
    }
}
