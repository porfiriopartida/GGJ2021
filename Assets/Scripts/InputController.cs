using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    
    public class InputController : MonoBehaviour
    {
        public Plane worldPlane;
        private CharacterController _characterController;
        private void Start()
        {
            this._characterController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            Vector3 direction = Vector3.zero;
            
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            // Vector3 pos = Input.mousePosition;
            // pos = Camera.main.ScreenToWorldPoint(pos);
            // pos.y = _characterController.transform.position.y;
            // Vector3 dir = _characterController.transform.position - pos;
            // float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            // _characterController.LookAt(-angle);
            
            if (horizontal != 0)
            {
                direction.x = horizontal;
            }
            
            if (vertical != 0)
            {
                direction.z = vertical;
            }
            
            _characterController.Move(direction);

            // if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Interact") > 0)
            // {

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {

            _characterController.Interact();
            }
            // if (Input.GetKeyDown(KeyCode.E))
            // {
            //     _characterController.GrabFlag();
            // }
            if (Input.GetAxis("Fire1") > 0 || Input.GetMouseButton(0))
            {
                _characterController.Fire();
            }
        }
    }
}