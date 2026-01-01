using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	#region singleten
	private static ScoreManager _instance;
    public static ScoreManager Instance=>_instance;
	#endregion

	#region private field
	/// <summary>
	/// Dữ liệu thông tin điểm số
	/// </summary>
	private LevelConfig _levelConfig;
	/// <summary>
	/// Yêu cầu của bàn chơi
	/// </summary>
	private List<Target> targets;

    /// <summary>
    /// tổng số lượt còn lại trong bàn này
    /// </summary>
    private int TotalRemainTurn;
    #endregion

    #region public field
    /// <summary>
    /// Dữ liệu thông tin điểm số
    /// </summary>
    public LevelConfig levelConfig {
        get { return this._levelConfig; }
        set { this._levelConfig = value; }
    }
	#endregion

	#region function logic
	public void Awake()
	{
        // gán singleten
        _instance = this;
	}

	/// <summary>
	/// set dữ liệu cho manager này
	/// </summary>
	/// <param name="data"></param>
	public void SetData(LevelConfig data)
	{
		this._levelConfig = data;
		this.targets = data.targets;
		this.TotalRemainTurn = data.numberInARound;
    }
	/// <summary>
	/// add điểm 
	/// trả về số ô được cộng thêm
	/// </summary>
	public int AddPoint(int TotalAdd,ETypeBlock type, out int totalRemainPoint)
	{
		totalRemainPoint = 0;
		if (type==ETypeBlock.NONE)
		{
			return 0;
        }	
		int squareAddMore = TotalAdd / 10;
		if (squareAddMore < 1)
		{
			return 0;
		}
		int totalReturn= 3 + squareAddMore - 1;
        // set điểm hco target
        foreach (Target target in this.targets)
		{
			if((ETypeBlock)target.type == type)
			{
				target.countNeed -= TotalAdd;
				if(target.countNeed < 0)
				{
					target.countNeed = 0;
				}
                totalRemainPoint= target.countNeed;
                break;
			}
        }
        //// cập nhật lại gjá trị
        //UIManager.Instance.uICoreHub.SetTargetItem(type, totalRemainPoint);
		Debug.Log("Đã cộng thêm điểm cho target loại "+ type.ToString()+" Số ô cộng thêm là "+ totalReturn);
        return totalReturn;
    }
	public bool IsEnoughToMove()
	{
		return false;
	}
	/// <summary>
	/// còn có thể bắn được không 
	/// </summary>
	/// <returns></returns>
	public bool IsHaveTurn()
	{
		return this.TotalRemainTurn > 0;
    }
    /// <summary>
    /// Trừ đi một lượt di chuyển
    /// </summary> 
    public void MoveSub()
	{
		this.TotalRemainTurn -= 1;
		// cập nhật lại giao diện
		UIManager.Instance.uICoreHub.SetNumberRemain(this.TotalRemainTurn);	
    }
    /// <summary>
    /// Trừ đi một lượt di chuyển
    /// </summary> 
    public int GetNumberTurnRemain()
    {
       return this.TotalRemainTurn;
    }
	public bool CheckWin()
	{
		bool checkWin = true;
        foreach (Target target in this.targets)
		{
			if(target.countNeed > 0)
			{
				return false;
			}
			checkWin = true;
        }
		// win rồi thì update level
		LevelManager.Instance.UpdateLevel();
		this.ShowWin();
        return checkWin;
	}
    public void ShowLose()
    {
        UIManager.Instance.uIPopupController.ShowLosePopup();
    }
    public void ShowWin()
    {
        UIManager.Instance.uIPopupController.ShowWinPopup();
    }
	
    #endregion

}
