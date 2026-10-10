using UnityEngine;
public class RotatingTrap : MonoBehaviour
{
    public float speed=100f;
    void Update(){ transform.Rotate(Vector3.up,speed*Time.deltaTime,Space.World); }
}
