// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace WillFrameworkPro.Extensions.MobileJoystick 
{
    /// <summary>
    /// 移动设备上的虚拟摇杆
    /// </summary>
    public class MobileJoystick : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private RectTransform joystickOutline;
        [SerializeField] private RectTransform joystickKnob;

        [Header(" Settings ")]
        [SerializeField] private float moveSpeed;
        
        private Vector3 clickedPosition;
        private Vector3 move;
        private bool canControl;
        private RectTransform canvasRect;
        void Start()
        {
            canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            HideJoystick();
        }

        void Update()
        {
            if(canControl)
                ControlJoystick();
        }

        public void ClickedOnJoystickZoneCallback()
        {
            clickedPosition = Input.mousePosition;
            joystickOutline.position = clickedPosition;

            ShowJoystick();
        }

        private void ShowJoystick()
        {
            joystickOutline.gameObject.SetActive(true);
            canControl = true;
        }

        private void HideJoystick()
        {
            joystickOutline.gameObject.SetActive(false);
            canControl = false;

            move = Vector3.zero;
        }

        private void ControlJoystick()
        {
            Vector3 currentPosition = Input.mousePosition;
            Vector3 direction = currentPosition - clickedPosition;
            //画布缩放比例（画布 rectTransform 下面的 scale），不同的手机屏幕，或者重力感应颠倒了，缩放比例都会发生变化。
            float canvasScale = canvasRect.localScale.x;
            //作用：计算摇杆的实际拖拽距离（受 moveSpeed 和 canvasScale 影响）。
            //变量解释：
            //     direction.magnitude：拖拽的原始像素距离。
            //     moveSpeed：灵敏度系数（值越大，相同拖拽距离下摇杆移动越远）。
            //     canvasScale：* canvasScale 是确保摇杆在不同分辨率下行为一致。
            float moveMagnitude = direction.magnitude * moveSpeed * canvasScale;
            //作用：计算摇杆背景的实际半径（考虑 UI 缩放后）。
            float absoluteWidth = joystickOutline.rect.width / 2;
            float realWidth = absoluteWidth * canvasScale;
            //限制摇杆移动不超过背景范围
            moveMagnitude = Mathf.Min(moveMagnitude, realWidth);
            move = direction.normalized * moveMagnitude;
            
            Vector3 targetPosition = clickedPosition + move;
            joystickKnob.position = targetPosition;

            if (Input.GetMouseButtonUp(0))
            {
                HideJoystick();
            }
        }
        //公共方法，能够从别的脚本获取到移动的值。这时候，因为和画布显示无关，所以就需要 / canvasScale 了。
        public Vector3 GetMoveVector()
        {
            float canvasScale = canvasRect.localScale.x;
            return move / canvasScale;
        }
    }
}

