using UnityEngine;

[ExecuteInEditMode]
public class CatchButton : MonoBehaviour
{
	#region SERIALIZE FIELDS

	[SerializeField] private Rect activeZoneRect;
	//Rect "чувствительной" зоны.

	[SerializeField] [Range (0, 100)] private float activeZoneOffset;
	//Отступ "во внутрь" "чувствительной" зоны, соответствующий кнопке за которой игрок "охотится".

	[SerializeField] private GUIStyle activeZoneStyle;
	//Стиль для отрисовки "чувствительной" зоны.

	[SerializeField] private GUIStyle buttonStyle;
	//Стиль для отрисовки кнопки.

	[SerializeField] private bool isDebugMode = true;
	//Флажок для переключения режимов работы. По умолчанию "истина", что соответствует отображению элементов режима отладки.

	#endregion

	#region PRIVATE FIELDS

	private float upDelta, downDelta, leftDelta, rightDelta, minDelta;
	//приватные переменные для определения дельты смещения

	#endregion

	#region UNITY EVENTS

	private void OnGUI ()
	{
		if (isDebugMode) {
			GUI.Box (activeZoneRect, "", activeZoneStyle);
		}

		if (GUI.Button (GetButtonRect (activeZoneRect, activeZoneOffset), "CATCH ME!", buttonStyle)) {
			//Выйграл!! Показать эффект.
			print ("CatchButton " + "Win!");
		}

		CheckMousePosition (GetMousePosition (), activeZoneRect);
	}

	#endregion

	#region PRIVATE METHODS

	private void CheckMousePosition (Vector2 mousePosition, Rect rect)
	{
		if (!IsPointOnRect (mousePosition, rect)) {
			return;
		}

		GetDeltas (mousePosition, rect);
		MoveButton ();

	}

	private void MoveButton ()
	{
		if (minDelta == upDelta) {

			activeZoneRect = new Rect (activeZoneRect.x, activeZoneRect.y + upDelta, activeZoneRect.width, activeZoneRect.height);
			return;
		}

		if (minDelta == downDelta) {

			activeZoneRect = new Rect (activeZoneRect.x, activeZoneRect.y - downDelta, activeZoneRect.width, activeZoneRect.height);
			return;
		}

		if (minDelta == leftDelta) {
					
			activeZoneRect = new Rect (activeZoneRect.x + leftDelta, activeZoneRect.y, activeZoneRect.width, activeZoneRect.height);
			return;
		}

		if (minDelta == rightDelta) {

			activeZoneRect = new Rect (activeZoneRect.x - rightDelta, activeZoneRect.y, activeZoneRect.width, activeZoneRect.height);
			return;
		}
	}

	private void GetDeltas (Vector2 mousePosition, Rect rect)
	{
		upDelta = mousePosition.y - rect.yMin;
		downDelta = rect.yMax - mousePosition.y;
		leftDelta = mousePosition.x - rect.xMin;
		rightDelta = rect.xMax - mousePosition.x;
		minDelta = Mathf.Min (upDelta, downDelta, leftDelta, rightDelta);
	}

	private bool IsPointOnRect (Vector2 point, Rect rect)
	{
		return point.x > rect.xMin && point.x < rect.xMax && point.y > rect.yMin && point.y < rect.yMax;
	}

	private Rect GetButtonRect (Rect activeZoneRect, float offSet)
	{
		return new Rect (activeZoneRect.x + offSet, activeZoneRect.y + offSet, activeZoneRect.width - 2 * offSet, activeZoneRect.height - 2 * offSet);
	}

	private Vector2 GetMousePosition ()
	{
		var temp = Input.mousePosition;
		return new Vector2 (temp.x, Screen.height - temp.y);
	}

	#endregion

}
