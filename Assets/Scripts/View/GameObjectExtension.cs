using System;
using UnityEngine;

namespace Bomberman
{
    public class MemberNotFoundException: Exception
    {
        public MemberNotFoundException(string name, UnityEngine.Object parent)
            : base($"{name} member not found")
        {
            Debug.LogError($"{name} member not found", parent);
        }

        public MemberNotFoundException(string message)
            : base(message)
        {
        }

        public MemberNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public static class MonoBehaviourExtension
    {
        public static T GetComponentEx<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return monoBehaviour.gameObject.GetComponentEx<T>();
        }

        public static void CheckMember(this MonoBehaviour monoBehaviour, System.Object member, string name)
        {
            monoBehaviour.gameObject.CheckMember(member, name);
        }

        public static void CheckMember(this MonoBehaviour monoBehaviour, System.Object[] members, string name)
        {
            monoBehaviour.gameObject.CheckMember(members, name);
        }
    }

    public static class GameObjectExtension
    {
        public static T GetComponentEx<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null) {
                gameObject.SetActive(false);
                throw new MemberNotFoundException(typeof(T).FullName, gameObject);
            }
            return component;
        }

        public static void CheckMember(this GameObject gameObject, System.Object member, string name)
        {
            if (member == null) {
                gameObject.SetActive(false);
                throw new MemberNotFoundException(name, gameObject);
            }
        }

        public static void CheckMember(this GameObject gameObject, System.Object[] members, string name)
        {
            if (members == null) {
                gameObject.SetActive(false);
                throw new MemberNotFoundException(name, gameObject);
            }
        }
    }
}