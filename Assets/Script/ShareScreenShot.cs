using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.XR.ARFoundation;

public class ShareScreenShot : MonoBehaviour
{
	[SerializeField] private GameObject mainMenuCanvas;
	private ARPointCloudManager aRPointCloudManager;

    private void Start()
    {
		Init();
    }
    private void Init()
    {
		aRPointCloudManager = FindObjectOfType<ARPointCloudManager>();
    }

	public void TakeScreenShot()
    {
		TurnOnOffARContents();
		StartCoroutine(TakeScreenshotAndShare());
    }

	private void TurnOnOffARContents()
    {
		var points = aRPointCloudManager.trackables;
        foreach (var point in points)
        {
			point.gameObject.SetActive(!point.gameObject.activeSelf);
        }
		mainMenuCanvas.SetActive(!mainMenuCanvas.activeSelf);
    }

	private IEnumerator TakeScreenshotAndShare()
	{
		yield return new WaitForEndOfFrame();

		Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		screenShot.Apply();

		string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		File.WriteAllBytes(filePath, screenShot.EncodeToPNG());

		// To avoid memory leaks
		Destroy(screenShot);

		new NativeShare().AddFile(filePath)
			.SetSubject("Subject goes here").SetText("Hello world!").SetUrl("https://github.com/yasirkula/UnityNativeShare")
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();

		TurnOnOffARContents();
		// Share on WhatsApp only, if installed (Android only)
		//if( NativeShare.TargetExists( "com.whatsapp" ) )
		//	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
	}
}
