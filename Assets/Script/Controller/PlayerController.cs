using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	#region serilize field
	/// <summary>
	/// dữ liệu của cell
	/// </summary>
	[SerializeField]
	private Cell cell;

	/// <summary>
	/// tốc độ của cell
	/// </summary>
	[SerializeField]
	private float speed=0.01f;
		
	/// <summary>
	/// chiều rộng của cell
	/// </summary>
	[SerializeField]
	private float width;
	#endregion

	#region
	/// <summary>
	/// loại block sẽ được tạo ra
	/// </summary>
	private ETypeBlock ETypeBlockRandom;

	/// <summary>
	/// số block sẽ được tạo ra
	/// </summary>
	private int count;
	#endregion

	#region function monobehaviour
	private void Start()
	{
		SpriteRenderer sprite = this.gameObject.GetComponent<SpriteRenderer>();
		this.width=sprite.bounds.size.x/2;
		this.ETypeBlockRandom = (ETypeBlock)UnityEngine.Random.Range(1, 4);
		this.count = UnityEngine.Random.Range(1, 8);
		Debug.Log("random ra giá trị:" + (int)ETypeBlockRandom+ count);
		this.cell.SpawnBlock(ETypeBlockRandom,count);	
	}
	private void Update()
	{
		float move = Input.GetAxisRaw("Horizontal");
		if(move!=0)
		{
			Vector2 posCache = this.cell.transform.position;
			// nếu pos vượt quá độ dài chia nửa thì đ cho di chuyển nữa
			if (Math.Abs(posCache.x + move * speed) < width)
			{
				this.cell.transform.position = new Vector2(posCache.x + move * speed, posCache.y);
			}	
		}
		this.Shoot();
	}
	public void Shoot()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			foreach(GameObject gameObj in this.cell.lstBlock )
			{
			   gameObj.GetComponent<Square>().Move(speed*100);
				
			}	
		}	
	}
	#endregion
}
