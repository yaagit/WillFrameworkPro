using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.StateMachine;

namespace WillFrameworkPro.Core.Tools.Animator
{
    [General]
    public class AnimatorTool
    {
        public Dictionary<GameObject, Dictionary<UnityEngine.Animator, BaseState>> _dic = new();

        // 通用模板方法
        private void ExecuteIfUnlocked(GameObject go, Action<UnityEngine.Animator, BaseState> animatorAction)
        {
            if (_dic.TryGetValue(go, out var innerDic))
            {
                var kv = innerDic.First(); // 假设每个 GameObject 只关联一个 Animator
                var animator = kv.Key;
                //当前的 animator 是否已经上锁，上锁了就不能设置新的动画状态了
                var host = kv.Value;

                animatorAction(animator, host);
            }
            else
            {
                if (go != null)
                {
                    var animator = go.GetComponent<UnityEngine.Animator>();
                    if (animator != null)
                    {
                        var newInnerDic = new Dictionary<UnityEngine.Animator, BaseState>();
                        newInnerDic.Add(animator, null);
                        _dic.Add(go, newInnerDic);
                        
                        animatorAction(animator, null);
                    }
                    else
                    {
                        Debug.LogError($"{go.name} 找不到 Animator 组件。");
                    }
                }
            }
        }
        // 对于锁状态的切换（模板方法）
        private void SwitchLockState(GameObject go, Action<Dictionary<UnityEngine.Animator, BaseState>, UnityEngine.Animator> animatorAction)
        {
            if (_dic.TryGetValue(go, out var innerDic))
            {
                var kv = innerDic.First(); // 假设每个 GameObject 只关联一个 Animator
                var animator = kv.Key;
                animatorAction(innerDic, animator);
            }
        }
        
        //对 animator 解锁
        public void ReleaseLock(GameObject go)
        {
            SwitchLockState(go, (innerDic, animator) => innerDic[animator] = null);
        }
        
        //对 animator 上锁
        public void Lock(GameObject go, BaseState host)
        {
            SwitchLockState(go, (innerDic, animator) => innerDic[animator] = host);
        }

        // ---------------------- 
        
        public void SetInteger(GameObject go, BaseState curState, string name, int value)
        {
            ExecuteIfUnlocked(go, (animator, host) =>
            {
                if (curState == host || host == null)
                {
                    animator.SetInteger(name, value);
                }
            });
        }

        public void SetFloat(GameObject go, BaseState curState, string name, float value)
        {
            ExecuteIfUnlocked(go, (animator, host) =>
            {
                if (curState == host || host == null)
                {
                    animator.SetFloat(name, value);
                }
            });
        }
        
        public void SetFloat(GameObject go, BaseState curState, string name, float value, float dampTime, float deltaTime)
        {
            ExecuteIfUnlocked(go, (animator, host) =>
            {
                if (curState == host || host == null)
                {
                    animator.SetFloat(name, value, dampTime, deltaTime);
                }
            });
        }

        public void SetBool(GameObject go, BaseState curState, string name, bool value)
        {
            ExecuteIfUnlocked(go, (animator, host) =>
            {
                if (curState == host || host == null)
                {
                    animator.SetBool(name, value);
                }
            });
        }

        public void SetTrigger(GameObject go, BaseState curState, string name)
        {
            ExecuteIfUnlocked(go, (animator, host) =>
            {
                if (curState == host || host == null)
                {
                    animator.SetTrigger(name);
                }
            });
        }

        public void ResetTrigger(GameObject go, BaseState curState, string name)
        {
            ExecuteIfUnlocked(go, (animator, host) =>
            {
                if (curState == host || host == null)
                {
                    animator.ResetTrigger(name);
                }
            });
        }
    }
}