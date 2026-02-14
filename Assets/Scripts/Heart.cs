// UMD IMDM290 
// Instructor: Myungin Lee
    // [a <-----------> b]
    // Lerp : Linearly interpolates between two points. 
    // https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Vector3.Lerp.html

using UnityEngine;

public class Heart : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 200; 
    float time = 0f;

    Vector3[] startPosition, heartPosition, starPosition;


    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        startPosition = new Vector3[numSphere]; 
        heartPosition = new Vector3[numSphere]; 
        starPosition = new Vector3[numSphere];
       
       float points = Random.Range(5f,8f);
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            float r = 10f; //original
            //float r = Random.Range(1f,10f);
            float eachSphereAngle = i * 2 * Mathf.PI / numSphere; //places each sphere evenly in a circle shape
            //startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f)); 
            //how can I change startPosition to make an interesting shape?    
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f)); //x,y,z

            r = 3f; // radius of the circle
            // Circular end position
            //endPosition[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere)); //made circle shape
            float sin = Mathf.Sin(i * 2 * Mathf.PI / numSphere);
            float cos = Mathf.Cos(i * 2 * Mathf.PI / numSphere);
            heartPosition[i] = new Vector3(Mathf.Sqrt(2f) * sin * sin * sin, -1 * cos * cos * cos - cos * cos + 2 * cos, 10f); //make heart shape
            //updated endPosition[i] follows equation (both x and y equations) on prof. lee's website. changed name to heartPosition

            //make star shape by manipulating radius, by changing distance of (5?) spheres, to be closer to center of shape
            
            float starRadius = 3f + Mathf.Sin(eachSphereAngle * points) * 1.5f;
            float x = Mathf.Cos(eachSphereAngle) * starRadius;
            float y = Mathf.Sin(eachSphereAngle) * starRadius;
            starPosition[i] = new Vector3(x, y, 10f);



        }


        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 

            // Position
            spheres[i].transform.position = startPosition[i];

            // Color. Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Measure Time 
        time += Time.deltaTime; // Time.deltaTime = The interval in seconds from the last frame to the current one
        // what to update over time?
        for (int i =0; i < numSphere; i++){ 
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            // let it oscillate over time using sin function
            float lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;
            //float lerpFraction = Mathf.Sin(time) * 1f + 1f; //what it shows: greater # increases time it stays as a heart before breaking apart 
            // Lerp logic. Update position
            //spheres[i].transform.position = Vector3.Lerp(startPosition[i], heartPosition[i], lerpFraction); 
            // For now, start positions and end positions are fixed. But what if you change it over time?
            // startPosition[i]; endPosition[i];

            float changeShape = Mathf.Sin(time * 0.5f) * 0.5f + 0.5f;
            Vector3 fromHeartToStar = Vector3.Lerp(heartPosition[i], starPosition[i], changeShape); 
            spheres[i].transform.position = Vector3.Lerp(startPosition[i], fromHeartToStar, lerpFraction);

            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1 
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Sin(time)), Mathf.Cos(time), 2f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;
            //if you make hue a public float in start, you can control what color your shape is
        }
    }
}
