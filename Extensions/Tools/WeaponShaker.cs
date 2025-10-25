using UnityEngine;
using WillFrameworkPro.Core.Views;

namespace WillFrameworkPro.Tools
{
    /// <summary>
    /// 为武器加入轻微的抖动效果，模拟“角色手握武器时微微颤动”的真实感。
    /// </summary>
    public class WeaponShaker : BaseView
    {
        [Header("Shaking Parameters")]
        public float shakeAmount = 0.2f;          // 抖动幅度（角度）
        public float shakeSpeed = 2.0f;           // 抖动速度（频率）

        private Quaternion _originalLocalRotation;

        void Start()
        {
            _originalLocalRotation = transform.localRotation;
        }

        void Update()
        {
            float time = Time.time * shakeSpeed;

            // 使用 PerlinNoise 生成 [-1, 1] 范围的平滑值
            float noiseX = Mathf.PerlinNoise(time, 0f) * 2f - 1f;
            float noiseY = Mathf.PerlinNoise(0f, time) * 2f - 1f;

            // 构造抖动角度
            Quaternion shakeRotation = Quaternion.Euler(
                noiseX * shakeAmount,
                noiseY * shakeAmount,
                0f // 不抖 Z，避免奇怪的侧倾
            );
            transform.localRotation = _originalLocalRotation * shakeRotation;
        }
    }
}