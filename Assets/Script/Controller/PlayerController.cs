using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : Singleton<PlayerController>
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

	#region public field
	public FrameShoot FrameShoot=>this.frameShoot;
	#endregion

	#region function monobehaviour
	private void Start()
	{
		this.Initialize();
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
		if(ScoreManager.Instance.IsHaveTurn())
		{
			/// trừ điểm nó đi
			ScoreManager.Instance.MoveSub();
            this.frameShoot.currentCell.Move(speed * 300);
			this.frameShoot.currentCell.gameObject.transform.SetParent(null);
			this.frameShoot.currentCell.SetFxVisible(false);
			this.frameShoot.currentCell=null;		
		}
	}
	///
	public Vector3 GetPositionMouse()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;
		return pos;
	}
	#endregion
}
