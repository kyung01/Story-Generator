using System.Collections;
using UnityEngine;

public class InWorldTextFeedback : MonoBehaviour
{
	
	 [SerializeField] public TMPro.TextMeshPro text;
	public float risingSpeed ;
	public float duration;
	float durationElapsed = 0;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		this.transform.position += new Vector3(0, risingSpeed * Time.deltaTime,0);
		durationElapsed += Time.deltaTime;
		float alpha = Mathf.Max(0, 1- durationElapsed / duration);

		text.color = new Color(1, 1, 1, alpha);

	}
}