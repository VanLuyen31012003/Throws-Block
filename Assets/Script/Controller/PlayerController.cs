using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	#region serilize field	
	/// <summary>
	/// chiều rộng của thanh slide
	/// </summary>
	[SerializeField]
	private float width;

	/// <summary>
	/// tốc độ của cell
	/// </summary>
	[SerializeField]
	private float speed = 0.01f;

	/// <summary>
	/// frame shoot quản lý di chuyển ô
	/// </summary>
	[SerializeField]
	private FrameShoot frameShoot;
	#endregion

	#region function monobehaviour
	private void Start()
	{
		this.Initialize();
	}
	private void Update()
	{
		this.frameShoot.Move();
		this.Shoot();
	}
	#endregion

	#region Function logic
	/// <summary>
	/// Khởi tạo 
	/// </summary>
	private void Initialize()
	{
		SpriteRenderer sprite = this.gameObject.GetComponent<SpriteRenderer>();
		this.width = sprite.bounds.size.x / 2;
		this.frameShoot.maxWidth = this.width;
		this.frameShoot.speed = this.speed;
		this.frameShoot.SpawnBulletSquare();
	}	

	/// <summary>
	/// Bắn các ô bay lên
	/// </summary>
	public void Shoot()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			//GameManager.Instance.GridManager.totalSquareWillAdd=this.frameShoot.cell.lstBlock.Count;
			//foreach (GameObject gameObj in this.frameShoot.cell.lstBlock)
			//{
			//	gameObj.GetComponent<Square>().Move(speed * 100);
			//	gameObj.transform.SetParent(null);
			//}
			this.frameShoot.currentCell.Move(speed * 100);
			this.frameShoot.currentCell.gameObject.transform.SetParent(null);
			this.frameShoot.currentCell=null;
			/// sinh lại để bắn tiếp
			this.frameShoot.SpawnBulletSquare();
		}
	}
	#endregion
}
