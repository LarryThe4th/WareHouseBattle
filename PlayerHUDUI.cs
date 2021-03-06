﻿using UnityEngine;
using System.Collections;
using System;

namespace BoxHound.UI {
    public class PlayerHUDUI : UIBase
    {
        #region Private variable
        // ---------------- Private variable ---------------
        [SerializeField]
        private LoaclHealthBarUI m_HealthBar;

        public LoaclHealthBarUI GetHealthBar {
            get { return m_HealthBar; }
        }

        [SerializeField]
        private WeaponInfoUI m_WeaponInfo;

        public WeaponInfoUI GetWeaponInfo
        {
            get { return m_WeaponInfo; }
        }

        [SerializeField]
        private CorssHairUI m_CorssHair;

        public CorssHairUI GetCorssHair
        {
            get { return m_CorssHair; }
        }

        [SerializeField]
        private TimerUI m_Timer;

        public TimerUI GetTimer
        {
            get { return m_Timer; }
        }

        [SerializeField]
        private KillComfirmation m_KillComfirmation;
        public KillComfirmation GetKillComfirmation {
            get { return m_KillComfirmation; }
        }

        [SerializeField]
        private FloatingDamageNumberUI m_FloatingDamageNumber;
        public FloatingDamageNumberUI GetFloatingDamageNumber
        {
            get { return m_FloatingDamageNumber; }
        }
        #endregion

        public override void InitUI()
        {
            base.InitUI();

            Properties = new UIProperties(
                UIframework.UIManager.SceneUIs.PlayerHUDUI, 
                UIframework.UIManager.DisplayUIMode.Normal, 
                UIframework.UIManager.UITypes.AboveBlurEffect, 
                true, true);

#if UNITY_EDITOR
            if (!m_HealthBar) Debug.Log("m_HealthBar is null");
            if (!m_WeaponInfo) Debug.Log("m_WeaponInfo is null");
            if (!m_CorssHair) Debug.Log("m_CorssHair is null");
            if (!GetTimer) Debug.Log("GetTimer is null");
            if (!m_KillComfirmation) Debug.Log("m_KillComfirmation is null");
            if (!m_FloatingDamageNumber) Debug.Log("m_FloatingDamageNumber is null");
#endif
            // Maybe i should make a base class for these guys...ummm
            m_HealthBar.Init();
            m_WeaponInfo.Init();
            m_CorssHair.Init();
            m_Timer.Init();
            m_KillComfirmation.Init();
            m_FloatingDamageNumber.Init();
        }

        public override void ShowUI()
        {
            // base.ShowUI();

            ShowIngameInfo(true);
        }

        public override void HideUI()
        {
            // base.HideUI();

            ShowIngameInfo(false);
        }

        public void ShowIngameInfo(bool show) {
            m_HealthBar.ShowHealthBar(show);
            m_WeaponInfo.ShowWeaponInfo(show);
            m_CorssHair.ShowCorssHair(show);
            m_Timer.ShowTimer(show);
        }

        private void OnPlayerSpawn() {
            ShowIngameInfo(true);
        }

        private void OnGameMenuOpen(bool pause) {
            ShowIngameInfo(!pause);
        }

        public override void EventRegister(bool reigist)
        {
            base.EventRegister(reigist);

            if (reigist)
            {
                MessageBroadCastManager.PlayerStartGameEvent += OnPlayerSpawn;
                MessageBroadCastManager.GamePauseEvent += OnGameMenuOpen;
            }
            else {
                MessageBroadCastManager.PlayerStartGameEvent -= OnPlayerSpawn;
                MessageBroadCastManager.GamePauseEvent -= OnGameMenuOpen;
            }
        }

        public override void SetLanguage(GameLanguageManager.SupportedLanguage language)
        {
            // Empty
        }

        public override void Process()
        {
            base.Process();
            // Update timer
            m_Timer.UdpateTimer();
        }
    }
}
