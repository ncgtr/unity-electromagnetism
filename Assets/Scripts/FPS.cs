using UnityEngine;

public class FPS : MonoBehaviour
{
    private float deltaTime = 0.0F;
    private float slowTimer = 0.0F;
    private string slowFPSText = "";

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1F;

        slowTimer += Time.unscaledDeltaTime;
        if (slowTimer >= 0.5F)
        {
            float slowMs = deltaTime * 1000.0F;
            float slowFps = 1.0F / deltaTime;
            slowFPSText = string.Format("{0:0.0} ms ({1:0.} fps)", slowMs, slowFps);
            
            slowTimer = 0.0F;
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        // Instant
        Rect rectFast = new Rect(10, 10, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = Color.green;

        float ms = deltaTime * 1000.0F;
        float fps = 1.0F / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", ms, fps);

        GUI.Label(rectFast, text, style);



        // Slower
        float verticalOffset = h * 2 / 50; 
        Rect rectSlow = new Rect(10, 10 + verticalOffset, w, h * 2 / 100);
        
        style.normal.textColor = Color.lightSeaGreen;

        if (string.IsNullOrEmpty(slowFPSText))
        {
            slowFPSText = string.Format("{0:0.0} ms ({1:0.} fps)", ms, fps);
        }

        GUI.Label(rectSlow, slowFPSText, style);
    }
}