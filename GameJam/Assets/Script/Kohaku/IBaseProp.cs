using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseProp
{
    public void OnHpChanged(int value);
    void OnAtkChanged(int value);
}

namespace BaseSystem.Property
{
    public class BaseProp : MonoBehaviour
    {
        protected List<object> targets = new List<object>();

        int _hp;
        int _atk;

        protected int hp
        {
            get => _hp;
            set
            {
                _hp = value;
                callHpCallback(value);
            }
        }
        protected int atk
        {
            get => _atk;
            set
            {
                _atk = value;
                callAtkCallback(value);
            }
        }

        /// <summary>
        /// �R�[���o�b�N�Ώۂ������ɂ��邱�ƂŃC���^�[�t�F�[�X�ɒ�`����Ă���֐����Ă�ł���܂��I
        /// </summary>
        /// <param name="target"></param>
        public void SetCallback(object target)
        {
            targets.Add(target);
        }

        void callHpCallback(int value)
        {
            foreach (var target in targets) (target as IBaseProp).OnHpChanged(value);
        }

        void callAtkCallback(int value)
        {
            foreach (var target in targets) (target as IBaseProp).OnAtkChanged(value);
        }


    }
}