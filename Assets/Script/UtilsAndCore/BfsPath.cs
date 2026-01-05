using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//node các ô cạnh bên
class MergePathState
{
	public Cell cell;
	public int total;
	public List<Cell> path;
}
public static class BfsPath
{
	/// <summary>
	/// Thuật toán bfs sẽ duyệt toàn bộ đường đi rồi sẽ lấy ra đường đi có thể ăn được nhiều ô nhất
	/// </summary>
	/// <param name="startCell"></param>
	/// <returns></returns>
	public static List<Cell> FindBestMergePathBFS(Cell startCell,ETypeBlock type)
	{
		Queue<MergePathState> queue = new Queue<MergePathState>();
		List<Cell> bestPath = new List<Cell>();
		int maxTotal = startCell.GetTotalSuareSameTypeInCell(type);

		queue.Enqueue(new MergePathState
		{
			cell = startCell,
			total = maxTotal,
			path = new List<Cell> { startCell }
		});

		while (queue.Count > 0)
		{
			var state = queue.Dequeue();

			foreach (var next in GameManager.Instance.GridManager.GetMergeableAroundCells(state.cell, type))
			{
				if (state.path.Contains(next))
					continue; 

				int newTotal = state.total + next.GetTotalSuareSameTypeOntop();

				var newPath = new List<Cell>(state.path);
				newPath.Add(next);

				if (newTotal > maxTotal)
				{
					maxTotal = newTotal;
					bestPath = newPath;
				}

				queue.Enqueue(new MergePathState
				{
					cell = next,
					total = newTotal,
					path = newPath
				});
			}
		}

		return bestPath;
	}

}
