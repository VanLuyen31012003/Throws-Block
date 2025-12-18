using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region singleten
	private static GameManager _instance;
	public static GameManager Instance => _instance;
	#endregion

	#region serilize field
	/// <summary>
	/// layer mask của cell để nhập
	/// </summary>
	[SerializeField]
	private LayerMask layerMaskCell;

	/// <summary>
	/// đối tượng quản lý lưới 
	/// </summary>
	[SerializeField]
	private GridManager gridManager;
	#endregion

	#region private and public field
	/// <summary>
	/// list parse dữ liệu từ json ra thông tin của level
	/// </summary>
	List<LevelConfig> levelConfigs = new List<LevelConfig>();

	/// <summary>
	///   GridManager
	/// </summary>
	public GridManager GridManager {
		get { return gridManager; }
	}

	/// <summary>
	/// Quanr lý cell index của layerMaskkCell
	/// </summary>
	public LayerMask LayerMaskCell
	{
		get { return layerMaskCell; }
	}
	#endregion

	#region function monobehaviour
	private void Awake()
    {
        this.Initialize();
	}

    private void Start()
    {
		gridManager.Initialize(levelConfigs[0]);
        //set up điểm yêu cầu cho setData
        UIManager.Instance.uICoreHub.SetData(levelConfigs[0]);
	}
	#endregion

	#region function logic
	/// <summary>
	/// Hàm khởi tạo
	/// </summary>
	private void Initialize()
    {
		_instance = this;
		this.LoadInfoLevelConfigs();
	}
	/// <summary>
	/// hàm đọc dữ liệu từ config parse vào levelConfigs
	/// </summary>
	private void LoadInfoLevelConfigs()
	{
        LevelConfig config = new LevelConfig()
        {
            level = 1,
            rows = 6,
            cols = 6,
            numberInARound = 10,
            targets = new List<Target>()
            {
                new Target(){ type=1, countNeed = 2},
				new Target(){ type=2, countNeed = 3},
				new Target(){ type=3, countNeed = 1},
				new Target(){ type=4, countNeed = 10},
			},
            cellDataConfigs = new List<CellDataConfig>()
            {
		        // ===== ROW 0 =====
		        new CellDataConfig(){ row=0, column=0, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=3, count=1 },
                    new SquareBoxDataConfig(){ type=1, count=1 }, // LINK
		        }},
                new CellDataConfig(){ row=0, column=1, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=4, count=1 },
                    new SquareBoxDataConfig(){ type=2, count=1 },
                }},
                new CellDataConfig(){ row=0, column=2, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=1, count=1 },
                    new SquareBoxDataConfig(){ type=3, count=1 },
                }},
                new CellDataConfig(){ row=0, column=3, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=2, count=1 },
                    new SquareBoxDataConfig(){ type=4, count=1 },
                }},
                new CellDataConfig(){ row=0, column=4, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=3, count=1 },
                    new SquareBoxDataConfig(){ type=1, count=1 },
                }},
                new CellDataConfig(){ row=0, column=5, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=4, count=1 },
                    new SquareBoxDataConfig(){ type=2, count=1 },
                }},

		        // ===== ROW 1 =====
		        new CellDataConfig(){ row=1, column=0, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=2, count=1 },
                    new SquareBoxDataConfig(){ type=2, count=1 },
                }},
                new CellDataConfig(){ row=1, column=1, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=1, count=1 },
                    new SquareBoxDataConfig(){ type=3, count=1 },
                }},
                new CellDataConfig(){ row=1, column=2, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=4, count=1 },
                    new SquareBoxDataConfig(){ type=4, count=1 },
                }},
                new CellDataConfig(){ row=1, column=3, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=3, count=1 },
                    new SquareBoxDataConfig(){ type=1, count=1 },
                }},
                new CellDataConfig(){ row=1, column=4, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=2, count=1 },
                    new SquareBoxDataConfig(){ type=2, count=1 },
                }},
                new CellDataConfig(){ row=1, column=5, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=1, count=1 },
                    new SquareBoxDataConfig(){ type=3, count=1 },
                }},

		        // ===== ROW 2 =====
		        new CellDataConfig(){ row=2, column=0, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=4, count=1 },
                    new SquareBoxDataConfig(){ type=3, count=1 },
                }},
                new CellDataConfig(){ row=2, column=1, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=1, count=1 },
                    new SquareBoxDataConfig(){ type=4, count=1 },
                }},
                new CellDataConfig(){ row=2, column=2, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=2, count=1 },
                    new SquareBoxDataConfig(){ type=1, count=1 },
                }},
                new CellDataConfig(){ row=2, column=3, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=3, count=1 },
                    new SquareBoxDataConfig(){ type=2, count=1 },
                }},
                new CellDataConfig(){ row=2, column=4, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=4, count=1 },
                    new SquareBoxDataConfig(){ type=3, count=1 },
                }},
                new CellDataConfig(){ row=2, column=5, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=1, count=1 },
                    new SquareBoxDataConfig(){ type=4, count=1 },
                }},

		        // ===== ROW 3 =====
		        new CellDataConfig(){ row=3, column=0, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=2, count=1 },
                    new SquareBoxDataConfig(){ type=4, count=1 },
                }},
                new CellDataConfig(){ row=3, column=1, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=3, count=1 },
                    new SquareBoxDataConfig(){ type=1, count=1 },
                }},
                new CellDataConfig(){ row=3, column=2, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=4, count=1 },
                    new SquareBoxDataConfig(){ type=2, count=1 },
                }},
                new CellDataConfig(){ row=3, column=3, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=1, count=1 },
                    new SquareBoxDataConfig(){ type=3, count=1 },
                }},
                new CellDataConfig(){ row=3, column=4, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=2, count=1 },
                    new SquareBoxDataConfig(){ type=4, count=1 },
                }},
                new CellDataConfig(){ row=3, column=5, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=3, count=1 },
                    new SquareBoxDataConfig(){ type=1, count=1 },
                }},

		        // ===== ROW 4 =====
		        new CellDataConfig(){ row=4, column=0, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=4, count=1 },
                    new SquareBoxDataConfig(){ type=1, count=1 },
                }},
                new CellDataConfig(){ row=4, column=1, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=2, count=1 },
                    new SquareBoxDataConfig(){ type=2, count=1 },
                }},
                new CellDataConfig(){ row=4, column=2, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=3, count=1 },
                    new SquareBoxDataConfig(){ type=3, count=1 },
                }},
                new CellDataConfig(){ row=4, column=3, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=4, count=1 },
                    new SquareBoxDataConfig(){ type=4, count=1 },
                }},
                new CellDataConfig(){ row=4, column=4, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=1, count=1 },
                    new SquareBoxDataConfig(){ type=1, count=1 },
                }},
                new CellDataConfig(){ row=4, column=5, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    new SquareBoxDataConfig(){ type=2, count=1 },
                    new SquareBoxDataConfig(){ type=2, count=1 },
                }},

		        // ===== ROW 5 =====
		        new CellDataConfig(){ row=5, column=0, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    //new SquareBoxDataConfig(){ type=3, count=1 },
                    //new SquareBoxDataConfig(){ type=2, count=1 },
                }},
                new CellDataConfig(){ row=5, column=1, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    //new SquareBoxDataConfig(){ type=4, count=1 },
                    //new SquareBoxDataConfig(){ type=3, count=1 },
                }},
                new CellDataConfig(){ row=5, column=2, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    //new SquareBoxDataConfig(){ type=1, count=1 },
                    //new SquareBoxDataConfig(){ type=4, count=1 },
                }},
                new CellDataConfig(){ row=5, column=3, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    //new SquareBoxDataConfig(){ type=2, count=1 },
                    //new SquareBoxDataConfig(){ type=1, count=1 },
                }},
                new CellDataConfig(){ row=5, column=4, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    //new SquareBoxDataConfig(){ type=3, count=1 },
                    //new SquareBoxDataConfig(){ type=2, count=1 },
                }},
                new CellDataConfig(){ row=5, column=5, squareBoxDataConfigs = new List<SquareBoxDataConfig>(){
                    //new SquareBoxDataConfig(){ type=4, count=1 },
                    //new SquareBoxDataConfig(){ type=3, count=1 },
                }},
            }
        };


        levelConfigs.Add(config);
	}

	#endregion
}
