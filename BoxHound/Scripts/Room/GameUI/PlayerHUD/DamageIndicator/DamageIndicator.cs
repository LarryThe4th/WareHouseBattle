﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace BoxHound
{
    /// <summary>
    /// All the information about the indicator cache in here.
    /// </summary>
    public class DamageIndicatorInfo
    {
        // The ID of the attacker who response for the attack.
        // This ID is generated by photon server as the PhotonPlayer.ID so its unique in each room.
        // According to photon network API document, this ID is -1 until it's assigned by server.
        public int AttackerID = -1;
        // The direction of the attack.
        public Vector3 Direction = Vector3.zero;
        // The color of the indicator's sprite.
        public Color IndicatorSpriteColor = Color.red;
        // The size of the indicator.
        public Vector2 IndicatorSize = Vector2.one;
        // How long will the indicator last on screen.
        public float IndicatorDisplayDuration = 4;
        // The pivot size of the indicator.
        public readonly float IndicatorPivotSize = 20;

        // The constructor of the damage indicator.
        public DamageIndicatorInfo(Vector3 direction) {
            Direction = direction;
        }
    }

    /// <summary>
    /// The damage indicator itself.
    /// </summary>
    public class DamageIndicator : PoolObject
    {
        #region Private variables
        // The ID of the attacker who response for the attack.
        // This ID is generated by photon server as the PhotonPlayer.ID so its unique in each room.
        // According to photon network API document, this ID is -1 until it's assigned by server.
        private int m_AttackerID = -1;
        // The reference of the indicator info.
        private DamageIndicatorInfo m_Info = new DamageIndicatorInfo(Vector3.zero);
        // The Transform of the indicator's sprite.
        //[SerializeField]
        //private RectTransform m_IndicatorTransform;
        // The transform of the indicator's pivot.
        // How long will the indicator last on screen.
        private float m_IndicatorDisplayDuration = 0;
        [SerializeField]
        private RectTransform m_IndicatorPivot;
        // The UI group.
        [SerializeField]
        private CanvasGroup m_GroupUI;

        // The reference of the indicator manager.
        private DamageIndicatorManager m_Manager = null;
        #endregion

        public int GetAttackerID {
            get { return m_AttackerID; }
        }

        public DamageIndicatorInfo GetInfo {
            get { return m_Info; }
        }

        public override void Init() {
            m_GroupUI = GetComponent<CanvasGroup>();
            m_GroupUI.alpha = 0;

            m_IndicatorDisplayDuration = 0;
            m_Info = new DamageIndicatorInfo(Vector3.zero);
            m_AttackerID = -1;
        }

        /// <summary>
        /// When create a new indicator of update a indicator, use this method.
        /// </summary>
        /// <param name="info">All the information that the indicator should know about.</param>
        /// <param name="manager">The indicator's manager.</param>
        /// <param name="update">If the indicator just been created, no need to update.</param>
        public void UpdateIndicator(DamageIndicatorInfo info, DamageIndicatorManager manager) {
            // Apply settings.
            m_Info = info;
            m_Manager = manager;

            // Apply the attacker.
            m_AttackerID = info.AttackerID;

            m_IndicatorDisplayDuration = m_Info.IndicatorDisplayDuration;

            m_GroupUI.alpha = 1;
            // Stop fading out.
            StopAllCoroutines();
            // Restart the indicator fade out process.
            StartCoroutine(Fadeout());
        }

        /// <summary>
        /// Countdown the timer and when time's up, fadeout the indicator and destory it.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Fadeout() {
            // Wait until time's up.
            yield return new WaitForSeconds(m_IndicatorDisplayDuration);
            // While the indicator is still visible.
            while (m_GroupUI.alpha > 0)
            {
                m_GroupUI.alpha -= 0.1f;
                yield return null;
            }
            // Befor remove this indicator, reset it's attacker ID
            m_Info.AttackerID = -1;
            m_AttackerID = m_Info.AttackerID;
            // When fadeout is finished, then remove indicator from list in manager.
            m_Manager.RemoveIndicator(this);
        }

        /// <summary>
        /// When a "new" indicator has been called
        /// </summary>
        public override void OnObjectReuse(params object[] options)
        {
            // Apply settings.
            m_Info = (DamageIndicatorInfo)options[0];
            m_Manager = (DamageIndicatorManager)options[1];

            // Reset the attacker ID.
            m_AttackerID = m_Info.AttackerID;

            m_IndicatorDisplayDuration = m_Info.IndicatorDisplayDuration;

            // Reset transparency.
            m_GroupUI.alpha = 1;

            // Start the indicator.
            StartCoroutine(Fadeout());
        }
    }
}
