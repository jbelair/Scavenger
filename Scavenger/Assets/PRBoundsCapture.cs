using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PRBoundsCapture : MonoBehaviour
{
    public string ship = "";
    public Camera mainCamera;
    public Renderer renderer;

    public void Start()
    {
        StartCoroutine(Snap());
    }

    IEnumerator Snap()
    {
        int i = 0;
        mainCamera.transform.position = renderer.bounds.center - renderer.bounds.extents.XOO() * 3;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center - renderer.bounds.extents.OYO() * 3;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center - renderer.bounds.extents.OOZ() * 3;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center - renderer.bounds.extents.XYO() * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center - renderer.bounds.extents.XOZ() * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center - renderer.bounds.extents.OYZ() * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center - renderer.bounds.extents * 1.5f;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();

        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.XOO() * 3;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.OYO() * 3;
        mainCamera.transform.LookAt(renderer.transform, Vector3.up);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.OOZ() * 3;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.XYO() * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.XOZ() * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.OYZ() * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents * 1.5f;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();

        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(-1, 1, 0)) * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(-1, 0, 1)) * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(0, -1, 1)) * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(-1, 1, 1)) * 1.5f;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();

        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(1, -1, 0)) * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(1, 0, -1)) * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(0, 1, -1)) * 2;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(1, -1, 1)) * 1.5f;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(1, 1, -1)) * 1.5f;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(-1, -1, 1)) * 1.5f;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
        mainCamera.transform.position = renderer.bounds.center + renderer.bounds.extents.Multiply(new Vector3(1, -1, -1)) * 1.5f;
        mainCamera.transform.LookAt(renderer.transform, Vector3.down);
        Snapshot(i);
        i++;
        yield return new WaitForEndOfFrame();
    }

    void Snapshot(int num)
    {
        string name = "_PR_" + ship + "_" + num.ToString() + ".png";
        ScreenCapture.CaptureScreenshot("Captures/Screenshots/" + name, 1);
    }
}
