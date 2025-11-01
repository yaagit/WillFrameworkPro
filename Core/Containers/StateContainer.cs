// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using System.Text;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Extensions.StateMachine;

namespace WillFrameworkPro.Core.Containers
{
    /// <summary>
    /// State Machine 的 State 容器。
    /// </summary>
    public class StateContainer
    {
        /// <summary>
        /// 字典键值对说明：
        ///     Type：每个 gameObject 所持有的 State Machine 有且仅有一个，因此 State Machine 的 Type 是唯一的。
        ///     HashSet<BaseState>：State Machine 与 state 对象的映射关系为一对多。
        /// </summary>
        private readonly Dictionary<Type, HashSet<BaseState>> _stateMachineMapping = new();
        /// <summary>
        /// 添加 Type 与 State 的映射关系，当不包含 Type 的时候，则创建。
        /// </summary>
        /// <param name="stateMachine"></param>
        /// <param name="state"></param>
        public void AddState(Type stateMachine, BaseState state)
        {
            if (_stateMachineMapping.TryGetValue(stateMachine, out HashSet<BaseState> states))
            {
                states.Add(state);
            }
            else
            {
                states = new();
                states.Add(state);
                _stateMachineMapping.Add(stateMachine, states);
            }
        }
        /// <summary>
        /// 获取 State 集合，获取不到回返回空集合。
        /// </summary>
        /// <param name="stateMachine">State Machine 的 Type</param>
        /// <returns></returns>
        public HashSet<BaseState> GetStates(Type stateMachine)
        {
            if (_stateMachineMapping.TryGetValue(stateMachine, out HashSet<BaseState> states))
            {
                return states;
            }
            return new HashSet<BaseState>();
        }
        /// <summary>
        /// 删除 stateMachine 下面的所有 State。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void ClearStates(Type stateMachine)
        {
            _stateMachineMapping.Remove(stateMachine);
        }
        
        public void Clear()
        {
            _stateMachineMapping.Clear();
        }
        
        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("-------------------------- State Container --------------------------\n");
            foreach (KeyValuePair<Type, HashSet<BaseState>> kv in _stateMachineMapping)
            {
                result.Append($"{kv.Key}:").Append("\n\t");
                foreach (BaseState state in kv.Value)
                {
                    result.Append($"{state.GetType().Name}").Append("，");
                }
                result.Append("\n");
            }
            result.Append("-------------------------------------------------------------------");
            return result.ToString();
        }
    }
}