using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region serilize field
    /// <summary>
    /// prefap của  square
    /// </summary>
    [SerializeField]
    private GameObject SquarePrefap;
	/// <summary>
	/// prefap của  square
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapGreen;
	/// <summary>
	/// prefap của  square
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapRed;
	/// <summary>
	/// số dòng
	/// </summary>
	[SerializeField]
	private int Row;
	/// <summary>
	/// số cột
	/// </summary>
	[SerializeField]
    private int Col;
    /// <summary>
    /// space giữa các item trong grid
    /// </summary>
    [SerializeField]
    private float space;
    #endregion

    #region private field
    /// <summary>
    /// grid quản lý các ô
    /// </summary>
    private SquareBox[,] GridContainer;

    /// <summary>
    /// size width của prefap
    /// </summary>
    float width ;

	/// <summary>
	/// size height của prefap
	/// </summary>
	float height;
	#endregion

	#region function
	private void Awake()
    {
        this.GridContainer= new SquareBox[Row, Col];
        SpriteRenderer spriteRenderer = SquarePrefap.GetComponent<SpriteRenderer>();
        this.width= spriteRenderer.sprite.bounds.size.x;
		this.height = spriteRenderer.sprite.bounds.size.y;
        Debug.Log("size của w:"+width);
		Debug.Log("size của h:"+height);
	}
	private void Start()
	{
        Initialize();
	}
    private void Initialize()
    {   
        for (int i = 0; i < Row; i++)
        {
            for(int j = 0; j < Col; j++)
            {
                GameObject gridItem = Instantiate(SquarePrefap,this.transform);
                float posX= j *width ;
				float posy = i*-height;
				gridItem.transform.position = new Vector2(posX,posy);
			}    
        }
		//for (int i = 0; i < Row; i++)
		//{
		//	for (int j = 0; j < Col; j++)
		//	{
		//		GameObject gridItem = Instantiate(SquarePrefap, this.transform);
		//		float posX = j * space;
		//		float posy = i * -space;
		//		gridItem.transform.position = new Vector2(posX, posy + 0.05f);
		//	}
		//}
		float posGridx = -3;
        float posGridy = 3;
        this.transform.position = new Vector2(-1.9f, 3 );
    }

	#endregion
}
