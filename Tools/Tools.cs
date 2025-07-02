using System;
using System.Collections;
using UnityEngine;
using WillFrameworkPro.Core.Attributes.Injection;
using WillFrameworkPro.Core.Attributes.Types;
using Random = UnityEngine.Random;

namespace WillFrameworkPro.Tools
{
    [General]
    public class Anim
    {
        /// <summary>
        /// 让 动画 layer 的权重变化过渡得更自然。
        /// </summary>
        /// <param name="animator">哪个 gameObject 挂载的 animator？</param>
        /// <param name="layerIndex">目标（哪一个动画 layer？）</param>
        /// <param name="targetWeight">目标要达到的权重</param>
        /// <param name="duration">持续时间</param>
        /// <returns></returns>
        public IEnumerator BlendOutAnimatorLayer(Animator animator, int layerIndex, float targetWeight, float duration, Action postprocessing)
        {
            float startWeight = animator.GetLayerWeight(layerIndex);
            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;
                animator.SetLayerWeight(layerIndex, Mathf.Lerp(startWeight, targetWeight, t));
                //游戏中会用 Time.timeScale = 0 暂停时间，所以不用 Time.time
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            animator.SetLayerWeight(layerIndex, targetWeight);
            //执行在状态转换完之后的后处理
            postprocessing();
        }
    }
    [General]
    public class Audio
    {
        /// <summary>
        /// 随机获取一个与之前不同的 clip，设定随机范围的 pitch，播放一次。
        /// </summary>
        public void PlayRandomClip(AudioSource source, AudioClip[] clips, Action<AudioSource, AudioClip> processing, float pitchMin = 1f, float pitchMax = 1f)
        {
            if (clips == null || clips.Length < 2)
            {
                return;
            }
            //注意：Random.Range 的参数为 float 或 int 时，行为各有不同。
            source.pitch = Random.Range(pitchMin, pitchMax);//参数为 float，前后闭合区间。
            // --- 随机获取一个与之前不同的 clip
            // 注意：因为循环的中断条件是获取到一个和之前不同的 clip，因此 clips 数量为 1 的话，会陷入死循环。
            int newIndex = Random.Range(0, clips.Length);//参数为 int，前闭后开区间。
            while (source.clip == clips[newIndex])
            {
                newIndex = Random.Range(0, clips.Length);
            }
            //注：这里用了模板代码，用户自行决定怎么播放音频。
            //      1.audioSource.PlayOneShot（可以叠加每个音频，不会打断原来正在播放的音频）
            //      2.audioSource.Play（每一次播放都会打断原来的音频，然后重新开始播放）
            processing(source, clips[newIndex]);
        }
        /// <summary>
        /// 给予一个 clip，设定随机范围的 pitch，播放一次。
        /// </summary>
        public void PlayClip(AudioSource source, AudioClip clip, Action<AudioSource, AudioClip> processing, float pitchMin = 1f, float pitchMax = 1f)
        {
            if (clip == null)
            {
                return;
            }
            //注意：Random.Range 的参数为 float 或 int 时，行为各有不同。
            source.pitch = Random.Range(pitchMin, pitchMax);//参数为 float，前后闭合区间。
            //播放一次。
            //注：这里用了模板代码，用户自行决定怎么播放音频。
            //      1.audioSource.PlayOneShot（可以叠加每个音频，不会打断原来正在播放的音频）
            //      2.audioSource.Play（每一次播放都会打断原来的音频，然后重新开始播放）
            processing(source, clip);
        }
        /// <summary>
        /// Play：
        ///     带有音量的播放，播放完后还原以前的音量。（这样不会影响到其他声音的播放）
        /// </summary>
        public IEnumerator PlayAndRestoreVolumeCoroutine(AudioSource source, AudioClip clip, float volume)
        {
            float originalVolume = source.volume;
            source.volume = volume;
            source.clip = clip;
            source.Play();
            yield return new WaitForSeconds(clip.length);
            source.volume = originalVolume;
        }
        /// <summary>
        /// Play：
        ///     带有音量控制的延时播放，播放完后还原以前的音量。（这样不会影响到其他声音的播放）
        /// </summary>
        public IEnumerator PlayDelayedWithVolume(AudioSource source, AudioClip clip, float delay, float volume)
        {
            float originalVolume = source.volume;
            source.volume = volume;
            source.clip = clip;
            yield return new WaitForSeconds(delay);
            source.Play();
            source.volume = originalVolume;
        }
        /// <summary>
        /// PlayOneSHot：
        ///     带有音量控制的延时播放，播放完后还原以前的音量。（这样不会影响到其他声音的播放）
        /// </summary>
        public IEnumerator PlayOneShotDelayedWithVolume(AudioSource source, AudioClip clip, float delay, float volume)
        {
            yield return new WaitForSeconds(delay);
            //PlayOneShot 使用临时的 volume，不会改变 volume 的值。
            PlayOneShotAtVolume(source, clip, volume);
        }
        /// <summary>
        /// PlayOneSHot：
        ///     延迟播放 PlayOneSHot。
        /// </summary>
        public IEnumerator PlayOneShotDelayed(AudioSource source, AudioClip clip, float delay)
        {
            yield return new WaitForSeconds(delay);
            source.PlayOneShot(clip);
        }
        /// <summary>
        /// PlayOneShot：
        ///     改进 PlayOneShot 方法，让它只播放设定的音量。
        /// </summary>
        public void PlayOneShotAtVolume(AudioSource source, AudioClip clip, float desiredVolume)
        {
            float scale = desiredVolume / Mathf.Max(source.volume, 0.0001f); // 防止除以 0
            source.PlayOneShot(clip, scale);
        }
    }
    /// <summary>
    /// 主要工具类。
    /// </summary>
    [General]
    public class Tools
    {
        [Inject] 
        public Anim Animator;
        [Inject] 
        public Audio Audio;
    }
}