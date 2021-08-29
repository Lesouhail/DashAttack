using System.Collections;
using System.Collections.Generic;
using DashAttack.Characters.Movements.Dash;
using DashAttack.Utility;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DashAttack.VFX
{
    public class ChromaAberration : MonoBehaviour
    {
        [SerializeField] private float time = 0.5f;
        [SerializeField] private float chromaIntensity = 1f;
        [SerializeField] private float paniniIntensity = 1f;

        private DashMovement PlayerDash { get; set; }
        private float Counter { get; set; }
        private float ChromaSpeed => chromaIntensity / time;
        private float PaniniSpeed => paniniIntensity / time;

        private ChromaticAberration chroma;
        private PaniniProjection panini;

        private void Start()
        {
            PlayerDash = GameObject.Find("Player").GetComponent<DashMovement>();

            var profile = GetComponent<Volume>().profile;
            profile.TryGet(out chroma);
            profile.TryGet(out panini);

            PlayerDash.Subscribe(DashState.Dashing, StateCallBack.OnStateEnter, () => StartCoroutine(Effect()));
        }

        private IEnumerator Effect()
        {
            Counter = 0;
            chroma.intensity.value = chromaIntensity;
            panini.distance.value = paniniIntensity;
            while (Counter < time)
            {
                Counter += Time.deltaTime;
                chroma.intensity.value -= ChromaSpeed * Time.deltaTime;
                panini.distance.value -= PaniniSpeed * Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }
}