using System;
using Chronos;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using R0.Static;
using UnityEngine;
using UnityEngine.Events;

namespace Vacuname
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Timeline))]
    public class Character : MonoBehaviour
    {
        [TabGroup("配置"), AssetsOnly, InlineEditor(InlineEditorModes.GUIOnly)]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f),LabelWidth(180),LabelText("移动设置"), SerializeField]
        public MoveAttribute moveAttribute;
        public float defaultScale=1;//图片初始的朝向，如果朝向为左则为-1

        #region 技能字典
        private Dictionary<string, UnityAction> skillDic;
        public Dictionary<string, UnityAction> GetSkillDic()
        {
            if (skillDic == null) skillDic = new Dictionary<string, UnityAction>();
            return skillDic;
        }
        public void TrigerSkill(string name)
        {
            if (skillDic.ContainsKey(name))
            {
                skillDic[name].Invoke();
            }
            else
                Debug.Log("无此技能："+name);
        }
        #endregion

        #region 反馈字典
        [SerializeField,TabGroup("反馈"), InlineEditor(InlineEditorModes.GUIOnly)]
        protected FeedbackDictionary feedbacks;
        public void TryPlayFeedback(string n)
        {
            feedbacks.TryPlay(n);
        }
        #endregion

        #region 参与运动计算需要的参数
        #region protected bool controllable
        protected bool controllable;
        public void SetControllable(bool a)
        {
            controllable = a;
        }
        #endregion
        protected float curAcceleraTime;
        [HideInInspector]public JumpState jumpState;
        #region protected float towardDirection;
        protected float towardDirection;
        public float GetTowardDirection()
        {
            return towardDirection;
        }
        public void SetTowardDirection(float dire)
        {
            dire.Normalize();
            towardDirection = dire;
            transform.localScale = SpriteTool.SetScaleDirection(transform.localScale, dire * defaultScale);
        }
        #endregion
        #endregion

        [HideInInspector]public Timeline time;//有问题，在编辑器编辑提示什么ASSET，不过暂时没出毛病
        [HideInInspector]public Rigidbody2D rd;
        [HideInInspector]public Animator anima;
        [TabGroup("配置")]
        // public SkeletonAnimation s_anima;
        public SkeletonAnimation sm_anima;

        protected virtual void Awake()
        {
            time = GetComponent<Timeline>();
            rd = GetComponent<Rigidbody2D>();
            TryGetComponent<Animator>(out anima);
            // if (anima == null) transform.Find("Spine").TryGetComponent<Animator>(out anima);
            // transform.Find("Spine1").TryGetComponent(out s_anima);
            curAcceleraTime = 0;
            jumpState = JumpState.fall;
            controllable = true;
            towardDirection = defaultScale;
        }
        public virtual void Jump()
        {
            if (jumpState == JumpState.ground)
            {
                time.rigidbody2D.velocity = new Vector2(time.rigidbody2D.velocity.x, moveAttribute.jumpStrength);
                jumpState = JumpState.jump;
            }
        }
        public virtual void Move(float input,bool setDirectly=false)
        {
            if (!controllable)
                return;
            float curSpeed;
            if (setDirectly)//直接设置速度的情况
            {
                curSpeed = input;
                input.Normalize();
                SetTowardDirection(input);
                time.rigidbody2D.velocity = new Vector2(curSpeed, time.rigidbody2D.velocity.y);
                anima?.SetFloat("Move", Mathf.Abs(curSpeed));
                return;
            }

            //标准化input
            input.Normalize();

            if (input != 0 && Math.Abs(towardDirection - input) > Const.IdleTolerance)
            {
                SetTowardDirection(input);
                /*moveDirection = input;
                Vector3 temp = transform.localScale;
                temp.x = moveDirection*Mathf.Abs(temp.x);
                transform.localScale = temp;*/
            }

            curSpeed = time.rigidbody2D.velocity.x;
            moveAttribute.GetCurSpeed(input, ref curSpeed, ref curAcceleraTime);
            time.rigidbody2D.velocity = new Vector2(curSpeed, time.rigidbody2D.velocity.y);

            anima?.SetFloat("Move", Mathf.Abs(curSpeed));
        }
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer==LayerMask.NameToLayer("Ground"))
                if (collision.transform.position.y < transform.position.y)
                {
                    jumpState = JumpState.ground;
                }
        }
    }
}
