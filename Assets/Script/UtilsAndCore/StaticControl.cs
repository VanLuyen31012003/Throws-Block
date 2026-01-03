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
    #endregion

}
