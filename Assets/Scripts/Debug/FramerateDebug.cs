using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DashAttack.Debug{
	
	public class FramerateDebug : MonoBehaviour
	{
	    [SerializeField] private float refreshRate { get; set; } = 0.25f;
	
	    private Text Text { get; set; }
	    private float Timer { get; set; }
	
	
	    private void Start()
	    {
	        Text = GetComponent<Text>();
	    }
	
	    private void Update()
	    {
	        Timer += Time.deltaTime;
	        if (Timer >= refreshRate)
	        {
	            Text.text = $"framerate: {1 / Time.deltaTime}";
	            Timer = 0;
	        }
	    }
	}
	
}
