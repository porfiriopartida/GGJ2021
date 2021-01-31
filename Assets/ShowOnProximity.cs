using UnityEngine;

public class ShowOnProximity : MonoBehaviour
{
    public GameObject renderer;

    private void Start()
    {
        renderer.SetActive(false);
    }

    public bool IsRunning = true;

    public void SetIsRunning(bool IsRunning)
    {
        this.IsRunning = IsRunning;
        if (!IsRunning)
        {
            renderer.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsRunning)
        {
            renderer.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsRunning)
        {
            renderer.SetActive(true);
        }
    }
}
