using UnityEngine;

public class LoadingAnimationIcon : MonoBehaviour
{
    public Material material;

    [Min(0.01f)]
    public float minSpeed = 0.1f;

    [Min(0.01f)]
    public float maxSpeed = 2f;

    [Min(0.01f)]
    public float changeSpeed = 0.5f;

    private float angle;
    private int rotationID;

    void Start()
    {
        if(material == null) {
            Debug.LogError("Assign the CircleAnimation material.", this);
            enabled = false;
            return;
        }

        rotationID = Shader.PropertyToID("_Rotation");

        if(!material.HasProperty(rotationID)) {
            Debug.LogError("Shader needs a Float property with Reference _Rotation.", this);
            enabled = false;
            return;
        }

        angle = material.GetFloat(rotationID);
    }

    void Update()
    {
        // Smooth speed oscillation between minSpeed and maxSpeed
        float t = (Mathf.Sin(Time.time * changeSpeed) + 1f) * 0.5f;
        float speed = Mathf.Lerp(minSpeed, maxSpeed, t);

        // Accumulate angle: it always increases, never reverses
        angle += speed * Time.deltaTime;

        material.SetFloat(rotationID, angle);
    }

}