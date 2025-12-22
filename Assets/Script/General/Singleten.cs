using UnityEngine;

/// <summary>
/// Base Singleton cho MonoBehaviour
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<T>();

				if (_instance == null)
				{
					Debug.LogError(
						$"Singleton<{typeof(T)}> not found in scene!"
					);
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}
}
