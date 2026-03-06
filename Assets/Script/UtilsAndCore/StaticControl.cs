using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticControl 
{
	#region Key Prefap
	/// <summary>
	/// key để dùng cho việc lấy ra level hiện tại
	/// </summary>
	public static string KEY_LEVEL="level";
	/// <summary>
	/// key để dùng cho việc lấy ra số lượt sử dụng suffle còn lại
	/// </summary>
	public static string KEY_SHUFFLE = "shuffle";
	/// <summary>
	/// key để dùng cho việc lấy ra số lượt sử dụng rocket còn lại
	/// </summary>
	public static string KEY_ROKET = "rocket";
	/// <summary>
	/// key để dùng cho việc lấy ra số lượt sử dụng bowling còn lại
	/// </summary>
	public static string KEY_BOWLING = "bowling";
	#endregion

	#region Value Time Dotween
	/// <summary>
	/// giá trị thời gian thực hiện anim của dotween
	/// </summary>
	public static float TIME_DOTWEEN_DURATION_ANIM = 0.2f;
    /// <summary>
    /// giá trị thời gian thực hiện anim gối đầu của dotween
    /// </summary>
    public static float TIME_DOTWEEN_CONTINUOUS_ANIM = 0.1f;
	/// <summary>
	/// giá trị thời gian thực hiện anim scale cho object
	/// </summary>
	public static float TIME_DOTWEEN_SCALE_ANIM = 0.2f;
	#endregion

	#region Value Divide Translate
	/// <summary>
	/// Giá trị chia để dịch square
	/// </summary>
	public static int VALUE_DEVIDE = 6;

	/// <summary>
	/// Giá trị để dịch cell lên y 1 đoạn
	/// </summary>
	public static int VALUE_TRANSLATETOP = 60;
    #endregion

    #region Key For EventSystem
    public static string ActionWhenStartPlay = "ActionWhenStartPlay"; // key để dùng gọi các sự kiện khi mà anim chơi đang thực thi
	public static string ActionWhenEndPlay = "ActionWhenEndAnim"; // key để dùng gọi các sự kiện khi mà anim chơi đã kết thúc
    public static string ActionWhenStartUsingSp = "ActionWhenStartUsingSp"; // key để dùng gọi các sự kiện khi mà sử dụng sự hỗ trợ đang thực thi
    public static string ActionWhenEndUsingSp = "ActionWhenEndUsingSp"; // key để dùng gọi các sự kiện khi mà sử dụng sự hỗ trợ kết thúc

    #endregion
}
