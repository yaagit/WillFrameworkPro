// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using WillFrameworkPro.Core.Attributes.Types;

namespace WillFrameworkPro.Extensions.Tools.TagManager
{
    /// <summary>
    /// 对 Unity 标签的管理器。拥有缓存系统，可以节约频繁查找带来的性能损耗。
    /// </summary>
    [General]
    public class TagManager
    {
        private static Dictionary<string, List<GameObject>> _taggedObjects = new Dictionary<string, List<GameObject>>();
        /// <summary>
        /// 若 GameObject 存在 tag，则注册。
        /// 这个方法已在项目启动的时候被调用，因此不建议用户继续调用这个方法。用户只需要使用查询方法即可。
        /// </summary>
        /// <param name="obj">Unity 的 gameObject</param>
        public static void RegisterObject(GameObject obj)
        {
            if (!String.IsNullOrEmpty(obj.tag))
            {
                RegisterObject(obj, obj.tag);
            }
        }
        /// <summary>
        /// 注册对象，保存到字典中。每个 tag 下面可以存在多个对象。
        /// </summary>
        private static void RegisterObject(GameObject obj, string tag)
        {
            if (!_taggedObjects.ContainsKey(tag))
            {
                _taggedObjects[tag] = new List<GameObject>();
            }
            _taggedObjects[tag].Add(obj);
        }
        /// <summary>
        /// 清空容器
        /// </summary>
        public static void Clear()
        {
            _taggedObjects.Clear();
        }
        /// <summary>
        /// 根据 tag 查找对象列表。
        /// </summary>
        /// <param name="tag">gameObject 所属的 tag</param>
        /// <returns></returns>
        public GameObject[] FindObjectsWithTag(string tag)
        {
            if (_taggedObjects.TryGetValue(tag, out var objects))
            {
                return objects.ToArray();
            }
            return Array.Empty<GameObject>();
        }
        /// <summary>
        /// 根据 tag 查找单个对象。
        /// </summary>
        /// <param name="tag">gameObject 所属的 tag</param>
        /// <returns></returns>
        public GameObject FindObjectWithTag(string tag)
        {
            var objects = FindObjectsWithTag(tag);
            if (objects.Length > 0)
            {
                return objects[0];
            }
            return null;
        }
        
        /// <summary>
        /// 重写 ToString 方法，格式化打印。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(10);
            foreach (KeyValuePair<string, List<GameObject>> outerKv in _taggedObjects)
            {
                result.Append($"{outerKv.Key}:").Append("    ").Append($"({outerKv.Value.Count})");
                result.Append("\n");
            }
            return result.ToString();
        }
        
    }
}