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
	private Vector3 _positionFrameOffset;
	private Camera _camera;
	#endregion

	#region function Monobehaviour
	private void OnMouseDown()
	{
		this._positionFrameOffset = this.transform.position - this.GetPositionMouse();
		this._positionFrameOffset.z = 0;
	}
	private void OnMouseDrag()
	{
		Vector3 pos = this.GetPositionMouse() + this._positionFrameOffset;
		// nếu pos vượt quá độ dài chia nửa thì đ cho di chuyển nữa
		if (Math.Abs(pos.x) < maxWidth)
		{
			this.transform.position = new Vector3(pos.x, this.transform.position.y);
		}
	}
	private void OnMouseUp()
	{
		PlayerController.Instance.Shoot();
	}
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
		this.ETypeBlockRandom = (ETypeBlock)UnityEngine.Random.Range(1, 5);
		this.count = UnityEngine.Random.Range(1, 8);
		Debug.Log("random ra giá trị:" + (int)ETypeBlockRandom + count);
		this.currentCell.SpawnBlock(ETypeBlockRandom, count);
		//---------------- lần 2
		ETypeBlock typeSecond;
		this.count = UnityEngine.Random.Range(1, 8);
		do
		{
			typeSecond =(ETypeBlock)UnityEngine.Random.Range(1, 5); 
		} while (typeSecond == this.ETypeBlockRandom);
		this.currentCell.SpawnBlock(typeSecond, count);

	}
	public Vector3 GetPositionMouse()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;
		return pos;
	}
	#endregion
}
