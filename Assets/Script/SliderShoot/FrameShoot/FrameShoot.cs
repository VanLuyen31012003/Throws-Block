using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameShoot : MonoBehaviour
{
	#region Serilize Field
	/// <summary>
	/// prefap của cell
	/// </summary>
	[SerializeField]
	public Cell cell;
	#endregion

	#region Private Field
	/// <summary>
	/// loại block sẽ được tạo ra
	/// </summary>
	private ETypeBlock ETypeBlockRandom;

	/// <summary>
	/// số block sẽ được tạo ra
	/// </summary>
	private int count;
	#endregion

	#region Public Field
	public float maxWidth;
	public float speed = 0.01f;
	public Cell currentCell;

	#endregion

	#region Function Logic
	/// <summary>
	/// Di chuyển
	/// </summary>
	public void Move()
    {
		float move = Input.GetAxisRaw("Horizontal");
		if (move != 0)
		{
			Vector2 posCache = this.gameObject.transform.position;
			// nếu pos vượt quá độ dài chia nửa thì đ cho di chuyển nữa
			if (Math.Abs(posCache.x + move * speed) < maxWidth)
			{
				this.gameObject.transform.position = new Vector2(posCache.x + move * speed, posCache.y);
			}
		}
	}
	/// <summary>
	/// Sinh các square đạn bắn
	/// </summary> 
	public void SpawnBulletSquare()
	{
		this.currentCell = Instantiate(this.cell, transform);
		this.currentCell.gameObject.SetActive(true);
		this.ETypeBlockRandom = (ETypeBlock)UnityEngine.Random.Range(1, 4);
		this.count = UnityEngine.Random.Range(1, 8);
		Debug.Log("random ra giá trị:" + (int)ETypeBlockRandom + count);
		this.currentCell.SpawnBlock(ETypeBlockRandom, count);
	}
	#endregion
}
