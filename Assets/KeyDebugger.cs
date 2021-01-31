using UnityEngine;

public class KeyDebugger : MonoBehaviour
{
    void Update()
    {
        if(Input.anyKeyDown)
        {
            string keyboardKey = Input.inputString;
            if (keyboardKey == null || keyboardKey.Equals(""))
            {
                for (int i = 0;i < 20; i++) {
                    if(Input.GetKeyDown("joystick 1 button "+i)){
                        print("joystick 1 button "+i);
                    }
                }
            }
            else
            {
                Debug.Log("Key pressed" + Input.inputString);
            }
        }
    }
}
