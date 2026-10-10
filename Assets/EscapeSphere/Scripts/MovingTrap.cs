using UnityEngine;
public class MovingTrap : MonoBehaviour
{
    public Vector3 travel = new Vector3(0,0,5);
    public float speed = 1.4f;
    Vector3 origin;
    void Start(){ origin=transform.position; }
    void Update(){ transform.position=origin+travel*((Mathf.Sin(Time.time*speed)+1f)*0.5f); }
}
