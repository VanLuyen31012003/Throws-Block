using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour
{
	/// <summary>
	/// loại block
	/// </summary>
    public ETypeBlock typeBlock;

	/// <summary>
	/// rigid quản lý tác động vật lý
	/// </summary>
    private Rigidbody2D rb;

	private void Awake()
	{
		this.rb = GetComponent<Rigidbody2D>();	
	}

	private void Update()
	{
	}
    public void Move(float speed)
	{
		Vector2 veloCache = rb.velocity;
		this.rb.velocity = new Vector2(veloCache.x, speed);
	}

}
