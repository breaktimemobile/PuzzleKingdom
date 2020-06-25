using Assets.Scripts.Utils;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    internal class AdsManager
    {
        public enum AdType
        {
            Stimulate,
            GotoMainPage,
            GetTask,
            GetAchiev,
            ResetLife,
            MultiAwards,
            UseProp,
            Refresh,
            Continue,
            Finish,
            ForceGround,
            Skin,
            ResetLife2
        }

        private sealed class __c__DisplayClass11_0
        {
            public AdsManager __4__this;

            public int awardValue;

            public AdsManager.AdType type;

            public Action finishCallback;

            public Action cancelCallback;

            internal void _Play_b__0()
            {
                this.__4__this.m_isWatch = false;
                GM.GetInstance().IsPlayVedioAds = false;
                this.__4__this.SaveVedioTime();
                GM.GetInstance().AddDiamond(this.awardValue, true);
                TaskData.GetInstance().Add(100105, 1, true);
                //				AppsflyerUtils.TrackVedio((int)this.type);
            }

            internal void _Play_b__2()
            {
                this.__4__this.m_isWatch = false;
                GM.GetInstance().IsPlayVedioAds = false;
                this.__4__this.SaveVedioTime();
                GM.GetInstance().AddDiamond(this.awardValue, true);
                TaskData.GetInstance().Add(100105, 1, true);
                //		AppsflyerUtils.TrackVedio((int)this.type);
            }

            internal void _Play_b__4()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                GM.GetInstance().AddDiamond(this.awardValue, true);
                Action expr_23 = this.finishCallback;
                if (expr_23 != null)
                {
                    expr_23();
                }
                //	AppsflyerUtils.TrackVedio((int)this.type);
            }

            internal void _Play_b__5()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.cancelCallback;
                if (expr_11 == null)
                {
                    return;
                }
                expr_11();
            }

            internal void _Play_b__6()
            {
                GM.GetInstance().AddDiamond(this.awardValue, true);
                Action expr_18 = this.finishCallback;
                if (expr_18 != null)
                {
                    expr_18();
                }
                //AppsflyerUtils.TrackVedio((int)this.type);
            }

            internal void _Play_b__7()
            {
                Action expr_06 = this.cancelCallback;
                if (expr_06 != null)
                {
                    expr_06();
                }
                GM.GetInstance().IsPlayVedioAds = false;
            }

            internal void _Play_b__8()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.finishCallback;
                if (expr_11 != null)
                {
                    expr_11();
                }
                //AppsflyerUtils.TrackVedio((int)this.type);
            }

            internal void _Play_b__9()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.cancelCallback;
                if (expr_11 == null)
                {
                    return;
                }
                expr_11();
            }

            internal void _Play_b__10()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.finishCallback;
                if (expr_11 != null)
                {
                    expr_11();
                }
                //AppsflyerUtils.TrackVedio((int)this.type);
            }

            internal void _Play_b__11()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.cancelCallback;
                if (expr_11 == null)
                {
                    return;
                }
                expr_11();
            }

            internal void _Play_b__12()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.finishCallback;
                if (expr_11 != null)
                {
                    expr_11();
                }
                //AppsflyerUtils.TrackVedio((int)this.type);
            }

            internal void _Play_b__13()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.cancelCallback;
                if (expr_11 == null)
                {
                    return;
                }
                expr_11();
            }

            internal void _Play_b__14()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.finishCallback;
                if (expr_11 != null)
                {
                    expr_11();
                }
                //AppsflyerUtils.TrackVedio((int)this.type);
            }

            internal void _Play_b__15()
            {
                GM.GetInstance().IsPlayVedioAds = false;
                Action expr_11 = this.cancelCallback;
                if (expr_11 == null)
                {
                    return;
                }
                expr_11();
            }

            internal void _Play_b__16()
            {
                Action expr_06 = this.finishCallback;
                if (expr_06 == null)
                {
                    return;
                }
                expr_06();
            }

            internal void _Play_b__17()
            {
                Action expr_06 = this.finishCallback;
                if (expr_06 == null)
                {
                    return;
                }
                expr_06();
            }
        }

        [Serializable]
        private sealed class __c
        {
            public static readonly AdsManager.__c __9 = new AdsManager.__c();

            public static Action __9__11_1;

            public static Action __9__11_3;

            internal void _Play_b__11_1()
            {
                GM.GetInstance().IsPlayVedioAds = false;
            }

            internal void _Play_b__11_3()
            {
                GM.GetInstance().IsPlayVedioAds = false;
            }
        }

        private float m_stimulateTimer;

        private long m_preVedioTime;

        private bool m_isWatch;

        private long m_preInsertTime;

        private static AdsManager m_instance;

        public bool IsWatch
        {
            get
            {
                return this.m_isWatch;
            }
        }

        
        public long M_preVedioTime
        {
            get
            {
                return this.m_preVedioTime;
            }

            set
            {
                m_preVedioTime = value;
            }
        }

        public static void Initialize()
        {
            AdsManager.m_instance = new AdsManager();
            AdsManager.m_instance.m_preVedioTime = AdsManager.m_instance.GetPreVedioTime();
            AdsManager.m_instance.m_preInsertTime = 0L;
            AdsManager.m_instance.SaveInsertTime();
        }

        public static AdsManager GetInstance()
        {
            if (AdsManager.m_instance == null)
            {
                AdsManager.m_instance = new AdsManager();
            }
            return AdsManager.m_instance;
        }

        public void Update()
        {
            if (this.m_isWatch)
            {
                return;
            }
            this.RefreshRewardTime();
            int num = (int)Math.Ceiling((double)(600f - this.m_stimulateTimer));
            if (num <= 0)
            {
                this.m_isWatch = true;
                this.m_stimulateTimer = 0f;
               DotManager.GetInstance().CheckAds();
            }
            GlobalEventHandle.EmitAdsHandle(string.Format("{0:D2}:{1:D2}", num / 60, num % 60), this.m_isWatch);
        }

        public void Play(AdsManager.AdType type, Action finishCallback = null, Action cancelCallback = null, int awardValue = 5, Action unLoadCallfunc = null)
        {
            switch (type)
            {
                case AdsManager.AdType.Stimulate:

                    //이미 시청줌
                    if (!this.m_isWatch)
                    {
                        //ToastManager.Show("TXT_NO_50013", true);
                        Debug.Log("이미시청");
                        return;
                    }

                    if (!AdsControl.Instance.Check_Rewaed())
                    {
                        //ToastManager.Show("TXT_NO_50013", true);
                        Debug.Log("아 없다고....");
                        return;
                    }

                    Debug.Log("아님 뭐냐....");

                    GM.GetInstance().IsPlayVedioAds = true;

                    this.m_isWatch = false;

                    AdsControl.Instance.reward_Type = Reward_Type.stimulate;
                    AdsControl.Instance.awardValue = awardValue;
              
                    AdsControl.Instance.ShowRewardedAd();

                    TaskData.GetInstance().Add(100105, 1, true);
                    AdsManager.GetInstance().SaveVedioTime();
                    AdsManager.GetInstance().M_preVedioTime = AdsManager.GetInstance().GetPreVedioTime();

                    GM.GetInstance().IsPlayVedioAds = false;
                    Debug.Log("끝....");

                    //AppsflyerUtils.TrackVedioError((int)type);
                    if (unLoadCallfunc != null)
                    {
                        unLoadCallfunc();
                        return;
                    }
                    break;
                case AdsManager.AdType.GotoMainPage:
                case AdsManager.AdType.GetTask:
                case AdsManager.AdType.GetAchiev:
                case AdsManager.AdType.Refresh:
                case AdsManager.AdType.Continue:
                case AdsManager.AdType.Finish:
                case AdsManager.AdType.ForceGround:
                    if (!this.isShowAds())
                    {
                        Action expr_2F6 = finishCallback;
                        if (expr_2F6 == null)
                        {
                            return;
                        }
                        expr_2F6();
                        return;
                    }
                    else
                    {
                        if (AdsUtil.isInsertISAdsLoaded())
                        {
                            AdsUtil.showInsertISAds(delegate
                            {
                                Action expr_06 = finishCallback;
                                if (expr_06 == null)
                                {
                                    return;
                                }
                                expr_06();
                            }, null);
                            return;
                        }
                        if (AdsUtil.isInsertAdsLoaded())
                        {
                            AdsUtil.showInsertAds(delegate
                            {
                                Action expr_06 = finishCallback;
                                if (expr_06 == null)
                                {
                                    return;
                                }
                                expr_06();
                            }, null);
                            return;
                        }
                        Action expr_33D = finishCallback;
                        if (expr_33D == null)
                        {
                            return;
                        }
                        expr_33D();
                    }
                    break;
                case AdsManager.AdType.ResetLife:
                case AdsManager.AdType.UseProp:
                case AdsManager.AdType.ResetLife2:
                    //AppsflyerUtils.TrackClickVedio((int)type);
                    GM.GetInstance().IsPlayVedioAds = true;
                    if (AdsUtil.isRewardAdsLoaded())
                    {
                        AdsUtil.showRewardAds(delegate
                        {
                            GM.GetInstance().IsPlayVedioAds = false;
                            Action expr_11 = finishCallback;
                            if (expr_11 != null)
                            {
                                expr_11();
                            }
                        //AppsflyerUtils.TrackVedio((int)type);
                    }, delegate
                    {
                        GM.GetInstance().IsPlayVedioAds = false;
                        Action expr_11 = cancelCallback;
                        if (expr_11 == null)
                        {
                            return;
                        }
                        expr_11();
                    });
                        return;
                    }
                    if (AdsUtil.isRewardISAdsLoaded())
                    {
                        AdsUtil.showRewardISAds(delegate
                        {
                            GM.GetInstance().IsPlayVedioAds = false;
                            Action expr_11 = finishCallback;
                            if (expr_11 != null)
                            {
                                expr_11();
                            }
                        //AppsflyerUtils.TrackVedio((int)type);
                    }, delegate
                    {
                        GM.GetInstance().IsPlayVedioAds = false;
                        Action expr_11 = cancelCallback;
                        if (expr_11 == null)
                        {
                            return;
                        }
                        expr_11();
                    });
                        return;
                    }
                    ToastManager.Show("TXT_NO_50014", true);
                    GM.GetInstance().IsPlayVedioAds = false;
                    ///AppsflyerUtils.TrackVedioError((int)type);
                    if (unLoadCallfunc != null)
                    {
                        unLoadCallfunc();
                        return;
                    }
                    break;
                case AdsManager.AdType.MultiAwards:
                    //AppsflyerUtils.TrackClickVedio((int)type);
                    GM.GetInstance().IsPlayVedioAds = true;

                    AdsControl.Instance.ShowRewardedAd();

                    GM.GetInstance().AddDiamond(awardValue * 2, true);

                    DialogManager.GetInstance().Close(null);

                    GM.GetInstance().IsPlayVedioAds = false;
                    return;
                case AdsManager.AdType.Skin:
                    ///AppsflyerUtils.TrackClickVedio((int)type);
                    GM.GetInstance().IsPlayVedioAds = true;
                    if (AdsUtil.isRewardAdsLoaded())
                    {
                        AdsUtil.showRewardAds(delegate
                        {
                            GM.GetInstance().IsPlayVedioAds = false;
                            Action expr_11 = finishCallback;
                            if (expr_11 != null)
                            {
                                expr_11();
                            }
                        ///	AppsflyerUtils.TrackVedio((int)type);
                    }, delegate
                    {
                        GM.GetInstance().IsPlayVedioAds = false;
                        Action expr_11 = cancelCallback;
                        if (expr_11 == null)
                        {
                            return;
                        }
                        expr_11();
                    });
                        return;
                    }
                    if (AdsUtil.isRewardISAdsLoaded())
                    {
                        AdsUtil.showRewardISAds(delegate
                        {
                            GM.GetInstance().IsPlayVedioAds = false;
                            Action expr_11 = finishCallback;
                            if (expr_11 != null)
                            {
                                expr_11();
                            }
                        ///	AppsflyerUtils.TrackVedio((int)type);
                    }, delegate
                    {
                        GM.GetInstance().IsPlayVedioAds = false;
                        Action expr_11 = cancelCallback;
                        if (expr_11 == null)
                        {
                            return;
                        }
                        expr_11();
                    });
                        return;
                    }
                    ToastManager.Show("TXT_NO_50014", true);
                    GM.GetInstance().IsPlayVedioAds = false;
                    ////AppsflyerUtils.TrackVedioError((int)type);
                    if (unLoadCallfunc != null)
                    {
                        unLoadCallfunc();
                        return;
                    }
                    break;
                default:
                    return;
            }
        }

        public void PlayTransiformGroundAds()
        {
            if (GM.GetInstance().IsFirstFinishGame())
            {
                return;
            }
            if (GM.GetInstance().IsPlayVedioAds)
            {
                return;
            }
            if (this.GetTimeStamp() - this.m_preInsertTime < 300L)
            {
                return;
            }
            this.SaveInsertTime();
            this.Play(AdsManager.AdType.ForceGround, null, null, 5, null);
        }

        public bool isShowAds()
        {
            return !GoodsManager.GetInstance().isPuchasedAds();
        }

        public void RefreshRewardTime()
        {
            this.m_stimulateTimer = (float)(this.GetTimeStamp() - this.m_preVedioTime);
        }

        public long GetPreVedioTime()
        {
            string @string = DataManager.Instance.state_Player.LocalData_PreVedioTime;
            if (@string.Equals("nil"))
            {
                return 0L;
            }
            return long.Parse(@string);
        }

        public void SaveVedioTime()
        {
            this.m_preVedioTime = this.GetTimeStamp();
            DataManager.Instance.state_Player.LocalData_PreVedioTime = this.m_preVedioTime.ToString();
            DataManager.Instance.Save_Player_Data();
        }

        public long GetPreInsertTime()
        {
            string @string = DataManager.Instance.state_Player.LocalData_PreInsertTime;
            if (@string.Equals("nil"))
            {
                return 0L;
            }
            return long.Parse(@string);
        }

        public void SaveInsertTime()
        {
            this.m_preInsertTime = this.GetTimeStamp();
            DataManager.Instance.state_Player.LocalData_PreInsertTime = this.m_preInsertTime.ToString();
            DataManager.Instance.Save_Player_Data();
        }

        public long GetTimeStamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000L) / 10000000L;
        }
    }
}
