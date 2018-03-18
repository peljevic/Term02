using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SpatialSlur.SlurCore;

/*
 * Notes
 */
 
namespace RC3.Unity.Examples.ReinforcedWalks
{
    /// <summary>
    /// 
    /// </summary>
    public class EdgeObject : RC3.Unity.EdgeObject
    {
        #region Static
        private const float _minWeight = 1f;
        private const float _minScale = 0.01f;

        #endregion


        private float _scale;
        private float _weight;


        /// <summary>
        /// 
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                OnSetScale();
            }
        }

        public float Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                OnSetWeight();
            }
        }

        private void OnSetWeight()
        {
            if (_weight < _minWeight)
            {
                if (gameObject.activeSelf)
                    gameObject.SetActive(false);
            }
            else
            {
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);


                var weight = transform.localScale;
                weight.x = weight.z = _weight;
                transform.localScale = weight;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void OnSetScale()
        {
            if (_scale < _minScale)
            {
                if (gameObject.activeSelf)
                    gameObject.SetActive(false);
            }
            else
            {
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);

             
            }
            
            var scale = transform.localScale;
            scale.x = scale.z = _scale;
            transform.localScale = scale;
        }
    }
}