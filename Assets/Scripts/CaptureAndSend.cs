using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using System;

public class CaptureAndSend : MonoBehaviour
{
    [System.Serializable]
    public class ImageData
    {
        public string image;
    }


    public Camera secondaryCamera;
    private string serverUrl = "https://refined-magnetic-buck.ngrok-free.app/upload";

    public void OnCaptureButtonPress()
    {
        if (secondaryCamera == null)
        {
            Debug.LogError("Secondary camera is not assigned.");
            return;
        }

        // Use the resolution of the camera's current target texture or default to the screen's resolution
        int width = secondaryCamera.targetTexture != null ? secondaryCamera.targetTexture.width/3 : Screen.width/3;
        int height = secondaryCamera.targetTexture != null ? secondaryCamera.targetTexture.height/3 : Screen.height/3;

        RenderTexture rt = new RenderTexture(width, height, 24); // Create a RenderTexture for capturing
        secondaryCamera.targetTexture = rt;
        secondaryCamera.Render();

        // Activate the render texture for reading
        RenderTexture.active = rt;

        // Encode frame as PNG (adjust format as needed)
        Texture2D frame = new Texture2D(width, height, TextureFormat.ARGB32, false);
        frame.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        frame.Apply();

        // Convert texture to byte array
        byte[] bytes = frame.EncodeToPNG();

        // Send captured frame to Node.js server
        StartCoroutine(SendFrameToServer(bytes));

        // Cleanup
        RenderTexture.active = null; // Remove active texture
        secondaryCamera.targetTexture = null; // Reset camera's target texture
        Destroy(rt);
        Destroy(frame);
    }

    [System.Obsolete]
    IEnumerator SendFrameToServer(byte[] frameData)
    {
        Debug.Log("Frame data length: " + frameData.Length);
        string base64Image = Convert.ToBase64String(frameData);
        ImageData imageData = new ImageData { image = base64Image };
        string json = JsonUtility.ToJson(imageData);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        Debug.Log("JSON Payload: " + json);

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, UnityWebRequest.kHttpVerbPOST))
        {
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending frame: " + request.error + ", Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Frame sent successfully, Response: " + request.downloadHandler.text);
            }
        }
    }

}
