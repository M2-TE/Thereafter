using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraCullingManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private float speed = 1.0f, minDistance = 0.02f, maxDistance = 120f;

    public bool increaseCulling = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCam.farClipPlane = minDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (increaseCulling && mainCam.farClipPlane <= maxDistance)
        {
            mainCam.farClipPlane = Mathf.Min(mainCam.farClipPlane + Time.deltaTime * speed, maxDistance);
        }
        else
        {
            mainCam.farClipPlane = Mathf.Max(mainCam.farClipPlane - Time.deltaTime * speed,minDistance);
        }
    }


    public IEnumerator LoadSceneOnTarget(int sceneID, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        increaseCulling = false;
        while (mainCam.farClipPlane < .1f)
        {
            yield return null;
        }

        var asyncLoad = SceneManager.LoadSceneAsync(sceneID);
        asyncLoad.allowSceneActivation = true;
    }
}
