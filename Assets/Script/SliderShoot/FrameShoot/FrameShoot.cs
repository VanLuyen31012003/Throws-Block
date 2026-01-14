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
	public float speed = 0.01f;
	public Cell currentCell;
	#endregion

	#region Function Logic
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
	#endregion
}
