using System;
using UnityEngine;
using UnityEngine.Serialization;
using WillFrameworkPro.Core.Views;

namespace WillFrameworkPro.Extension.Sound
{
    /// <summary>
    /// 本脚本说明：
    ///     当 Audio Source 设置了 spatial blend 为 (0, 1) 之间的数值时，实际的效果为 3d音效混合 2d。这个时候，设置音量距离衰减就有点 “失效” 了。因为始终参杂了一点
    /// 2d 效果，而 2d 音效是始终存在，不随距离的变化而衰减的。如果把 spatial blend 配置为 1，这时候是一个完全的 3d 音效，音量按距离衰减完美，但是带出了另外一个问题：左右声道的变化非常突兀，有时候一边耳道有声音，另一边是完全的无声，这是不符合真实世界的。
    ///     因此，为了彻底解决音量随着距离衰减这个问题，spatial blend 可以设置为一个 0.x 的数值（左右声道转换自然），不依赖 3d sound settings 的 distance 来计算音量衰减，用本脚本来控制音量衰减。
    /// 附 unity 配置说明：
    ///     3d sound settings:
    ///         min distance: 在这个距离范围内音量始终是 1.
    ///         max distance: 在这个距离范围内音量从 1 衰减至 0.
    ///         曲线：
    ///             x 轴：距离。
    ///             y 轴：音量大小。
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class StereoDistanceControl : BaseView
    {
        public Transform player;
        public float minDistance = 1f;
        public float maxDistance = 3f;

        private AudioSource source;
        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        void Update()
        {
            float distance = Vector3.Distance(player.position, source.transform.position);
            float volume;
            if(distance <= minDistance)
                volume = 1f;             // 最小距离音量最大
            else if(distance >= maxDistance)
                volume = 0f;             // 超过最大距离音量归零
            else
                volume = 1f - (distance - minDistance) / (maxDistance - minDistance); // 线性衰减

            source.volume = Mathf.Lerp(source.volume, volume, Time.deltaTime * 5f);//让音量变化更自然，防止“跳变”
        }
    }
}

